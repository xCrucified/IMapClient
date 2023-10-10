using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using System;
using System.Windows;

namespace IMapClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string myEmailAddress;
        string accountPassword;
        private ImapClient client;

        public MainWindow(string login, string pass)
        {
            InitializeComponent();

            myEmailAddress = login;
            accountPassword = pass;
            client = new ImapClient();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListFolders();
        }
        private async void ListFolders()
        {
            try
            {
                await client.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(myEmailAddress, accountPassword);

                //listBox.Items.Clear();

                foreach (var fl in await client.GetFoldersAsync(client.PersonalNamespaces[0]))
                {
                    listBox.Items.Add(fl.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error!: {ex.Message}");
            }
            finally
            {
                if (client.IsConnected)
                {
                    client.Disconnect(true);
                }
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (client != null && client.IsConnected)
            {
                client.Disconnect(true);
            }
            Application.Current.Shutdown();
        }
    }
}
