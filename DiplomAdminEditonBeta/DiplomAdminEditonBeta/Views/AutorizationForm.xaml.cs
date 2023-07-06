using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace DiplomAdminEditonBeta
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AutorizationForm : Window
    {
        public AutorizationForm()
        {
            InitializeComponent();
        }
        private void EntryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ClientTCP.IsConnected)
                {
                    ClientTCP.Connect();
                    ClientTCP.IsConnected = true;
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
            TCPMessege tCPMessege = new TCPMessege(2, 1, new User() { Login = LoginTB.Text, Password = PasswordTB.Password });
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if(tCPMessege == null)
            {
                MessageBox.Show("Сервер не отвечает!");
                ClientTCP.IsConnected = false;
                return;
            }
            if(tCPMessege.CodeOperation == 0)
            {
                MessageBox.Show("Неверно введен логин или пароль!");
                return;
            }
            if(tCPMessege.CodeOperation == -1)
            {
                MessageBox.Show("Пользователь уже авторизован в системе!");
                return;
            }
            User user = JsonConvert.DeserializeObject<User>(tCPMessege.Entity);
            MainForm mainForm = new MainForm(user);
            mainForm.Show();
            Close();
        }

        private void PasswordTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            TBPS.Text = PasswordTB.Password;
        }
    }
}
