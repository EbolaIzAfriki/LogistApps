using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Windows;

namespace DiplomAdminEditonBeta.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddUserForm.xaml
    /// </summary>
    public partial class AddCarrierForm : Window
    {
        CarriersPage CarriersPage;
        public AddCarrierForm(CarriersPage carriersPage)
        {
            InitializeComponent();
            CarriersPage = carriersPage;
        }

        private void AddBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(AddressTB.Text.Replace(" ", "")) || string.IsNullOrEmpty(EmailTB.Text.Replace(" ", "")) || string.IsNullOrEmpty(NameTB.Text.Replace(" ", "")) || PhoneTB.Text.Contains('_'))
                {
                    MessageBox.Show("Не все поля заполнены!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                TCPMessege tCPMessege = new TCPMessege(3,3, new Carrier()
                {
                    Address = AddressTB.Text.Trim(),
                    Email = EmailTB.Text.Trim(),
                    Name = NameTB.Text.Trim(),
                    Phone = PhoneTB.Text
                });
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    Close();
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 0)
                {
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(tCPMessege.Entity), "Ошибка при создании", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                CarriersPage.Carriers.Add(JsonConvert.DeserializeObject<Carrier>(tCPMessege.Entity));
                CarriersPage.UpdateCarriersDatagridFromList();
                MessageBox.Show("Перевозчик был добавлен!");
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
