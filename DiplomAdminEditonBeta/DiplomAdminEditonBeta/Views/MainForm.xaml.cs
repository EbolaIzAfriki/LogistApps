using DiplomAdminEditonBeta.TCPModels;
using DiplomAdminEditonBeta.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace DiplomAdminEditonBeta
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {

        public CarriersPage CarriersPage;
        public ClientPage clientPage = new ClientPage();
        public RequestPage RequestPage;
        public User CurrentUser;
        public static MainForm mainForm;
        public MainForm(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            mainForm = this;
            RequestPage = new RequestPage(this);
            CarriersPage = new CarriersPage(this);
            MainFrame.Content = clientPage;
            LoadTypeConstraint();
        }

        private void LoadTypeConstraint()
        {
            TCPMessege tCPMessege = new TCPMessege(1, 10, null);
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            TypeConstraint.typeConstraints = JsonConvert.DeserializeObject<List<TypeConstraint>>(tCPMessege.Entity);
        }

        private void ClientsRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            MainFrame.Content = clientPage;
        }
        private void RequestsRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            MainFrame.Content = RequestPage;
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AutorizationForm autorizationForm = new AutorizationForm();
            autorizationForm.Show();
            Close();
            ClientTCP.Disconnect();
        }

        public static void ReturnToAutorization()
        {
            ClientTCP.IsConnected = false;
            MessageBox.Show("Соединение с сервером потеряно!");
            AutorizationForm autorizationForm = new AutorizationForm();
            autorizationForm.Show();
            mainForm.Close();
            if (MainWorkOnTaskForm.mainWorkStaticForm != null)
                MainWorkOnTaskForm.mainWorkStaticForm.Close();
        }
        private void CarriersRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            MainFrame.Content = CarriersPage;
        }
    }
}
