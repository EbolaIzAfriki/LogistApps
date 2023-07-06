using DiplomAdminEditonBeta.TCPModels;
using DiplomAdminEditonBeta.Views.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DiplomAdminEditonBeta
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public List<Client> Clients = new List<Client>();
        public ClientPage()
        {
            DataContext = this;
            InitializeComponent();
            UpdateClientDatagrid();
        }

        public void UpdateClientDatagrid()
        {
            ClientsDataGrid.ItemsSource = null;

            TCPMessege tCPMessege = new TCPMessege(1,2,null);
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if(tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            Clients = JsonConvert.DeserializeObject<List<Client>>(tCPMessege.Entity);

            ClientsDataGrid.ItemsSource = Clients;
        }

        public void UpdateClientDatagridFromList()
        {
            ClientsDataGrid.ItemsSource = null;
            ClientsDataGrid.ItemsSource = Clients;
        }

        private void AddBut_Click(object sender, RoutedEventArgs e)
        {
            AddClientForm addClientForm = new AddClientForm(this);
            addClientForm.ShowDialog();
        }

        private void DeleteBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientsDataGrid.SelectedItem == null)
                    return;
                Client client = ClientsDataGrid.SelectedItem as Client;

                if (MessageBox.Show($"Вы действительно хотите удалить клиента {client.CompanyName}?", "Удаление клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }

                TCPMessege tCPMessege = new TCPMessege(5, 2, client.Id);
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 1)
                {
                    Clients.Remove(client);
                    UpdateClientDatagridFromList();
                }
                else
                {
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(tCPMessege.Entity), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MainForm.ReturnToAutorization();
                return;
            }
        }

        private DispatcherTimer dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(20) };
        private int position = 0;

        private void ClientsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem == null)
                return;
            Client client = ClientsDataGrid.SelectedItem as Client;
            if (ClientsDataGrid.CurrentCell.Column == null)
                return;
            int indexColumn = ClientsDataGrid.CurrentCell.Column.DisplayIndex;
            string value = "";
            position = Clients.IndexOf(client);
            switch (indexColumn)
            {
                case 1:
                    {
                        client.CompanyName = client.CompanyName.Trim(' ');
                        if (string.IsNullOrEmpty(client.CompanyName))
                        {
                            client.CompanyName = string.Empty;
                            UpdateClientDatagridFromList();
                            ClientsDataGrid.SelectedIndex = position;
                            MessageBox.Show("Строка пуста!!!");
                            return;
                        }
                        value = client.CompanyName;
                        break;
                    }
                case 2:
                    {
                        client.Address = client.Address.Trim(' ');
                        if (string.IsNullOrEmpty(client.Address))
                        {
                            client.Address = string.Empty;
                            UpdateClientDatagridFromList();
                            ClientsDataGrid.SelectedIndex = position;
                            MessageBox.Show("Строка пуста!!!");
                            return;
                        }
                        value = client.Address;
                        break;
                    }
                case 3:
                    {
                        client.Email = client.Email.Trim(' ');
                        if (string.IsNullOrEmpty(client.Email))
                        {
                            client.Email = string.Empty;
                            UpdateClientDatagridFromList();
                            ClientsDataGrid.SelectedIndex = position;
                            MessageBox.Show("Строка пуста!!!");
                            return;
                        }
                        value = client.Email;
                        break;
                    }
            }
            TCPMessege tCPMessege = new TCPMessege(4, 2, new List<string> { client.Id.ToString(), indexColumn.ToString(), value });
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MessageBox.Show("Ошибка на сервере!");
                MainForm.ReturnToAutorization();
                return;
            }

            if (tCPMessege.CodeOperation == -1)
            {
                MessageBox.Show("Этот клиент был удален!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateClientDatagrid();
                position = -1;
            }
            if (tCPMessege.CodeOperation == 0)
            {
                List<string> vs = JsonConvert.DeserializeObject<List<string>>(tCPMessege.Entity);
                MessageBox.Show("При изменении произошла ошибка: " + vs[0]);
                position = Clients.IndexOf(client);
                vs = JsonConvert.DeserializeObject<List<string>>(vs[1]);
                switch (int.Parse(vs[1]))
                {
                    case 1:
                        {
                            Clients[position].CompanyName = vs[2];
                            break;
                        }
                    case 2:
                        {
                            Clients[position].Address = vs[2];
                            break;
                        }
                    case 3:
                        {
                            Clients[position].Email = vs[2];
                            break;
                        }
                }
                UpdateClientDatagridFromList();
            }
            dispatcherTimer.Tick += Timertick;
            dispatcherTimer.Start();
        }

        private void Timertick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            ClientsDataGrid.SelectedIndex = position;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchTB.Text == "")
            {
                UpdateClientDatagridFromList();
                return;
            }
            ClientsDataGrid.ItemsSource = null;
            ClientsDataGrid.ItemsSource = Clients.Where(p => p.CompanyName.ToLower().Contains(SearchTB.Text.ToLower()) || p.Address.ToLower().Contains(SearchTB.Text.ToLower()) || p.Email.ToLower().Contains(SearchTB.Text.ToLower()));
        }

        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            UpdateClientDatagrid();
            SearchTB_TextChanged(null, null);
        }
    }
}
