using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EmailView.Dtos;
using EmailView.Models.Account;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.AspNetCore.Components;

namespace EmailView.DataServices
{
    public class IMAPService
    {
        private static readonly ImapClient Client = new ImapClient();
        public static ICredentials Credentials;
        protected string _email;

        static Task CurrentTask = Task.FromResult(true);


        const MessageSummaryItems SummaryItems = MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.Flags | MessageSummaryItems.BodyStructure | MessageSummaryItems.Headers;
        public List<MessageInfo> messages = new List<MessageInfo>();
        IMailFolder folder;

        public IMAPService()
        {
            Client.Disconnected += OnClientDisconnected;
        }

        #region Connection and Authentication

        public bool IsAuthenticated { get { return Client.IsAuthenticated; } }

        public async Task Login(Login model)
        {
            await SignInClicked(model.Username, model.Password);
        }

        public async Task SignInClicked(string emailAddress, string emailPassword, int port = 993, bool useSsl = true)
        {
            var options = SecureSocketOptions.StartTlsWhenAvailable;
            // var host = serverCombo.Text.Trim ();
            var host = ImapServerFromDomain(emailAddress);
            Console.WriteLine($"Address = {emailAddress}, Host = {host}");
            // var passwd = passwordTextBox.Text;
            // var user = loginTextBox.Text;
            // int port;

            // if (!string.IsNullOrEmpty (portCombo.Text))
            // 	port = int.Parse (portCombo.Text);
            // else
            // 	port = 0; // default

            Credentials = new NetworkCredential(emailAddress, emailPassword);
            _email = emailAddress;

            if (useSsl)
                options = SecureSocketOptions.SslOnConnect;

            try
            {
                await ReconnectAsync(host, port, options);
            }
            catch (Exception ex)
            {
                // string msg = "Failed to Authenticate to server. If you are using GMail, then you probably " +
                //     "need to go into your GMail settings to enable \"less secure apps\" .\n\n" +
                //     "For a real Mail application, you'll want to add support for obtaining the " +
                //     "user's OAuth2 credentials to prevent the need for user's to enable this. ";
                Console.WriteLine(ex.Message);
                Client.Disconnect(false);
                throw;
            }
        }

        static async void OnClientDisconnected(object sender, DisconnectedEventArgs e)
        {
            if (e.IsRequested)
                return;

            await ReconnectAsync(e.Host, e.Port, e.Options);
        }

        public static async Task ReconnectAsync(string host, int port, SecureSocketOptions options)
        {
            // Note: for demo purposes, we're ignoring SSL validation errors (don't do this in production code)
            Client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            await Client.ConnectAsync(host, port, options);

            // Note: Since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            Client.AuthenticationMechanisms.Remove("XOAUTH2");

            await Client.AuthenticateAsync(Credentials);

            if (Client.Capabilities.HasFlag(ImapCapabilities.UTF8Accept))
                await Client.EnableUTF8Async();

            // Console.WriteLine($"IsConnected = {Client.IsConnected} / IsAuthenticated = {Client.IsAuthenticated}");
            CurrentTask = Task.FromResult(Client.IsConnected && Client.IsAuthenticated);
        }

        public async Task Logout()
        {
            await Client.DisconnectAsync(true);
        }

        #endregion Connection and Authentication

        public async Task<List<MessageInfo>> GetMessagesAsync()
        {
            try
            {
                if (Client.IsAuthenticated)
                {
                    OpenFolder(null);

                    messages.Clear();

                    if (!folder.IsOpen)
                    {
                        await folder.OpenAsync(FolderAccess.ReadOnly);
                    }

                    if (folder.Count > 0)
                    {
                        int daysBack = 7;
                        var query = SearchQuery.DeliveredAfter(DateTime.UtcNow.AddDays(daysBack * -1));
                        // var orderBy = new[] { OrderBy.ReverseArrival };
                        var uids = await folder.SearchAsync(query);
                        var summaries = await folder.FetchAsync(uids, SummaryItems);
                        AddMessageSummaries(summaries);
                    }

                    // folder.CountChanged += CountChanged;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Client.Disconnect(false);
                throw;
            }
            return messages;
        }

        private string ImapServerFromDomain(string emailAddress)
        {
            string domain = emailAddress.Substring(emailAddress.IndexOf('@') + 1);
            switch (domain)
            {
                case "outlook.com":
                    return "outlook.office365.com";  // port 993
                case "gmail.com":
                    return "imap.gmail.com";
                case "yahoo.com":
                    return "imap.mail.yahoo.com";
                case "icloud.com":
                    return "imap.mail.me.com";
                default:
                    return "imap-mail.outlook.com";  // port 993
            }
        }

        public void OpenFolder(IMailFolder folder)
        {
            if (folder == null)
            {
                this.folder = Client.Inbox;
            }
            else
            {

                if (this.folder == folder)
                    return;

                // if (this.folder != null) {
                // 	this.folder.MessageFlagsChanged -= MessageFlagsChanged;
                // 	this.folder.MessageExpunged -= MessageExpunged;
                // 	this.folder.CountChanged -= CountChanged;
                // }

                // folder.MessageFlagsChanged += MessageFlagsChanged;
                // folder.MessageExpunged += MessageExpunged;

                this.folder = folder;
            }
        }

        void AddMessageSummaries(IEnumerable<IMessageSummary> summaries)
        {
            foreach (var summary in summaries)
            {
                var info = new MessageInfo(summary);
                // Console.WriteLine($"Adding: {info.Summary.Envelope.Subject}");
                messages.Add(info);
            }
        }
    }
}