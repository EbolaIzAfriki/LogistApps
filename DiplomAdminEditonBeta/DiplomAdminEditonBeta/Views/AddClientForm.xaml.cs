using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DiplomAdminEditonBeta.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddClientForm.xaml
    /// </summary>
    public partial class AddClientForm : Window
    {
        ClientPage ClientPage;
        public AddClientForm(ClientPage clientPage)
        {
            InitializeComponent();
            ClientPage = clientPage;
        }
        private void AddBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(AddressTB.Text.Replace(" ", "")) || string.IsNullOrEmpty(CompanyNameTB.Text.Replace(" ", "")) || string.IsNullOrEmpty(EmailTB.Text.Replace(" ", "")) || string.IsNullOrEmpty(CompanyTypeCB.Text.Replace(" ", "")))
                {
                    MessageBox.Show("Не все поля заполнены!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Client client = new Client() { Address = AddressTB.Text.Trim(' '), CompanyName = CompanyNameTB.Text.Trim(' '), Email = EmailTB.Text.Trim(' '), TypeId = (CompanyTypeCB.SelectedIndex + 1), Point = new List<Point>()};
                TCPMessege tCPMessege = new TCPMessege(3, 2, client);
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    Close();
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 1)
                {
                    ClientPage.Clients.Add(JsonConvert.DeserializeObject<Client>(tCPMessege.Entity));
                    ClientPage.UpdateClientDatagridFromList();
                    Close();
                }
                else
                {
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(tCPMessege.Entity));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
