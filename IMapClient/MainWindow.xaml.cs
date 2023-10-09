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
        private ImapClient client = new();
        
        public MainWindow(string login, string pass)
        {
            InitializeComponent();
            myEmailAddress = login;
            accountPassword = pass;
        }
         
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await client.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(myEmailAddress, accountPassword);

                foreach (var fl in client.GetFolders(client.PersonalNamespaces[0]))
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
