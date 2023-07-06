using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiplomAdminEditonBeta.Views.PagesTask
{
    /// <summary>
    /// Логика взаимодействия для ChosePostAndConsumptions.xaml
    /// </summary>
    public partial class VendorsAndConsumptionsPage : Page
    {
        public static int CountVendors = 2, CountConsumptions = 2;
        public static bool IsModificate = true;

        public  static List<Client> ChosedClient, NotChosedClients;
        private List<int> IdClients;

        public static bool IsClientsChose = false;

        PointsPage PointsPage;
        MainWorkOnTaskForm MainWorkOnTaskForm;
        private bool IsInitializaFinished = false;
        public VendorsAndConsumptionsPage(PointsPage pointsPage, MainWorkOnTaskForm mainWorkOnTaskForm)
        {
            InitializeComponent();
            PointsPage = pointsPage;
            MainWorkOnTaskForm = mainWorkOnTaskForm;

            //Инициализируем список выбранных не выбранных клиентов а так же массив id выбранных клиентов
            ChosedClient = MainWorkOnTaskForm.DBTask.Point.Select(p => p.Client).Distinct().ToList();
            IdClients = ChosedClient.Select(d => d.Id).ToList();
            NotChosedClients = mainWorkOnTaskForm.MainForm.clientPage.Clients.Where(p => !IdClients.Contains(p.Id)).ToList();

            //Получаем число потребителей и поставщиков и заполняем текстовые поля
            CountVendors = MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 2).ToList().Count;
            CountConsumptions = MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 1).ToList().Count;
            CountConsumptionsTB.Text = CountConsumptions.ToString();
            CountVendorsTB.Text = CountVendors.ToString();


            ChoseConsumprionDataGrid.ItemsSource = ChosedClient.Where(p => p.TypeId == 1).ToList();
            AllConsumptionsDataGrid.ItemsSource = NotChosedClients.Where(p => p.TypeId == 1).ToList();

            ChoseVendorDataGrid.ItemsSource = ChosedClient.Where(p => p.TypeId == 2).ToList();
            AllVendorsDatagrid.ItemsSource = NotChosedClients.Where(p => p.TypeId == 2).ToList();
            IsInitializaFinished = true;
            EnablePointPage();
        }

        public void UpdateDataGridConsumptions()
        {
            ChoseConsumprionDataGrid.ItemsSource = null;
            AllConsumptionsDataGrid.ItemsSource = null;

            ChoseConsumprionDataGrid.ItemsSource = ChosedClient.Where(p => p.TypeId == 1).ToList();
            AllConsumptionsDataGrid.ItemsSource = NotChosedClients.Where(p => p.TypeId == 1).ToList();
        }

        public void UpdateDataGridVendors()
        {
            ChoseVendorDataGrid.ItemsSource = null;
            AllVendorsDatagrid.ItemsSource = null;

            ChoseVendorDataGrid.ItemsSource = ChosedClient.Where(p => p.TypeId == 2).ToList();
            AllVendorsDatagrid.ItemsSource = NotChosedClients.Where(p => p.TypeId == 2).ToList();
        }

        private void AddConsumprionButton_Click(object sender, RoutedEventArgs e)
        {
            if (AllConsumptionsDataGrid.SelectedItem == null)
                return;

            ChosedClient.Add(AllConsumptionsDataGrid.SelectedItem as Client);
            NotChosedClients.Remove(AllConsumptionsDataGrid.SelectedItem as Client);

            IdClients = ChosedClient.Select(d => d.Id).ToList();
            UpdateDataGridConsumptions();
            EnablePointPage();
        }

        private void DeleteConsumptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChoseConsumprionDataGrid.SelectedItem == null)
                return;
            if(ChosedClient.Where(p => p.TypeId == 1).ToList().Count < 2)
            {
                MessageBox.Show("У заявки должена быть по крайней мере одна компания-потребитель!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NotChosedClients.Add(ChoseConsumprionDataGrid.SelectedItem as Client);
            ChosedClient.Remove(ChoseConsumprionDataGrid.SelectedItem as Client);

            IdClients = ChosedClient.Select(d => d.Id).ToList();

            Client client = ChoseConsumprionDataGrid.SelectedItem as Client;
            Client DifferentClient = ChosedClient.FirstOrDefault(p => p.TypeId == 1);
            ChooseClientsPointTask(client.Id, DifferentClient.Id);

            UpdateDataGridConsumptions();
            EnablePointPage();
        }

        private void AddVendorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AllVendorsDatagrid.SelectedItem == null)
                return;

            ChosedClient.Add(AllVendorsDatagrid.SelectedItem as Client);
            NotChosedClients.Remove(AllVendorsDatagrid.SelectedItem as Client);

            IdClients = ChosedClient.Select(d => d.Id).ToList();
            UpdateDataGridVendors();
            EnablePointPage();
        }

        private void ChooseClientsPointTask(int deleteClientId, int NewClientId)
        {
            if (MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 1).ToList().Count > 0)
            {
                TCPMessege tCPMessege = new TCPMessege(1, 5, new List<int> {MainWorkOnTaskForm.DBTask.Id, deleteClientId, NewClientId });
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 0)
                {
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(tCPMessege.Entity), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    MainWorkOnTaskForm.Close();
                }
                else
                {
                    MainWorkOnTaskForm.DBTask = JsonConvert.DeserializeObject<Task>(tCPMessege.Entity);
                }
            }
        }

        public void UpdateChosedVendorsDG()
        {
            ChoseConsumprionDataGrid.ItemsSource = null;
            ChoseVendorDataGrid.ItemsSource = null;
            ChoseConsumprionDataGrid.ItemsSource = ChosedClient.Where(p => p.TypeId == 1).ToList();
            ChoseVendorDataGrid.ItemsSource = ChosedClient.Where(p => p.TypeId == 2).ToList();
        }

        private void DeleteVendorButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChoseVendorDataGrid.SelectedItem == null)
                return;
            if (ChosedClient.Where(p => p.TypeId == 2).ToList().Count < 2)
            {
                MessageBox.Show("У заявки должена быть по крайней мере одна компания-поставщик!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NotChosedClients.Add(ChoseVendorDataGrid.SelectedItem as Client);
            ChosedClient.Remove(ChoseVendorDataGrid.SelectedItem as Client);

            IdClients = ChosedClient.Select(d => d.Id).ToList();

            Client client = ChoseVendorDataGrid.SelectedItem as Client;
            Client DifferentClient = ChosedClient.FirstOrDefault(p => p.TypeId == 2);

            ChooseClientsPointTask(client.Id, DifferentClient.Id);

            UpdateDataGridVendors();
            EnablePointPage();
        }

        private void UpdatePointList(int TypeClient, int CountClient)
        {
            IsModificate = true;

            int CountPointInTask = MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == TypeClient).ToList().Count; // Число точек потребления в задаче
            if (CountPointInTask == CountClient)
                return;
            TCPMessege tCPMessege;
            if (CountPointInTask < CountClient) // Добавление точек
            {
                List<int> vs = new List<int>() { MainWorkOnTaskForm.DBTask.Id, ChosedClient.FirstOrDefault(p => p.TypeId == TypeClient).Id, CountClient - CountPointInTask};
                tCPMessege = new TCPMessege(3, 5, vs);
            }
            else
            {
                List<int> vs = new List<int>() { MainWorkOnTaskForm.DBTask.Id, ChosedClient.FirstOrDefault(p => p.TypeId == TypeClient).Id, CountClient - CountPointInTask};
                tCPMessege = new TCPMessege(5, 5, vs);
            }
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            if (tCPMessege.CodeOperation == 0)
            {
                MessageBox.Show(JsonConvert.DeserializeObject<string>(tCPMessege.Entity), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWorkOnTaskForm.Close();
            }
            else
            {
                MainWorkOnTaskForm.DBTask = JsonConvert.DeserializeObject<Task>(tCPMessege.Entity);
            }
            EnablePointPage();
        }

        private void CountConsumptionsTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsInitializaFinished)
                return;
            if (string.IsNullOrEmpty(CountConsumptionsTB.Text))
            {
                MessageBox.Show("Эта строка не может быть пустой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                CountConsumptionsTB.Text = CountConsumptions.ToString();
                return;
            }
            int num;
            if (int.TryParse(CountConsumptionsTB.Text, out num))
            {
                if(num == 0)
                {
                    OffConnRB();
                    return;
                }
                if (num == CountConsumptions)
                {
                    EnablePointPage();
                    return;
                }

                if (num > 100)
                {
                    if (MessageBox.Show($"Использование большого количества пунктов может привести к неправильной работе программы. Вы уверены что хотите создать" +
                        $" {num} пунктов?", "Удаление клиента", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                CountConsumptions = num;
                UpdatePointList(1, CountConsumptions);
            }
        }

        private void CountVendorsTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsInitializaFinished)
                return;
            if (string.IsNullOrEmpty(CountVendorsTB.Text))
            {
                MessageBox.Show("Эта строка не может быть пустой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                CountVendorsTB.Text = CountVendors.ToString();
                return;
            }
            int num;
            if (int.TryParse(CountVendorsTB.Text, out num))
            {
                if (num == 0)
                {
                    OffConnRB();
                    return;
                }
                if (num == CountVendors)
                {
                    EnablePointPage();
                    return;
                }
                
                if (num > 100)
                {
                    if (MessageBox.Show($"Использование большого количества пунктов может привести к неправильной работе программы. Вы уверены что хотите создать" +
                        $" {num} пунктов?", "Удаление клиента", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                CountVendors = num;
                UpdatePointList(2, CountVendors);
            }
        }

        private void CountConsumptionsTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void CountVendorsTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void CountVendorsTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                labalFoc.Focus();
            }
        }

        private void CountConsumptionsTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                labalFoc.Focus();
            }
        }


        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            MainWorkOnTaskForm.MainForm.clientPage.UpdateClientDatagrid();
            NotChosedClients = MainWorkOnTaskForm.MainForm.clientPage.Clients.Where(p => !IdClients.Contains(p.Id)).ToList();

            AllConsumptionsDataGrid.ItemsSource = null;
            AllVendorsDatagrid.ItemsSource = null;
            AllConsumptionsDataGrid.ItemsSource = NotChosedClients.Where(p => p.TypeId == 1).ToList();
            AllVendorsDatagrid.ItemsSource = NotChosedClients.Where(p => p.TypeId == 2).ToList();

            SearchConsTB_TextChanged(null, null);
            SearchVendorTB_TextChanged(null, null);
        }

        private void SearchConsTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchConsTB.Text == "")
            {
                AllConsumptionsDataGrid.ItemsSource = null;
                AllConsumptionsDataGrid.ItemsSource = NotChosedClients.Where(p => p.TypeId == 1).ToList();

                return;
            }
            AllConsumptionsDataGrid.ItemsSource = null;
            AllConsumptionsDataGrid.ItemsSource = NotChosedClients.Where(p => p.CompanyName.ToLower().Contains(SearchConsTB.Text.ToLower()) && p.TypeId == 1);
        }

        private void SearchVendorTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchVendorTB.Text == "")
            {
                AllVendorsDatagrid.ItemsSource = null;
                AllVendorsDatagrid.ItemsSource = NotChosedClients.Where(p => p.TypeId == 2).ToList();
                return;
            }
            AllVendorsDatagrid.ItemsSource = null;
            AllVendorsDatagrid.ItemsSource = NotChosedClients.Where(p => p.CompanyName.ToLower().Contains(SearchVendorTB.Text.ToLower()) && p.TypeId == 2);
        }

        private void EnablePointPage()
        {
            if (MainWorkOnTaskForm.DBTask == null)
                return;
            if (MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 1).ToList().Count > 0 && MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 2).ToList().Count > 0 && ChoseVendorDataGrid.Items.Count > 0 && ChoseConsumprionDataGrid.Items.Count > 0)
            {
                MainWorkOnTaskForm.PointsRB.IsEnabled = true;
                IsClientsChose = true;
                PointsPage.UpdateDataGrid();

                if(!MainWorkOnTaskForm.DBTask.Point.Select(p => p.ProductCount).Contains(0))
                {
                    MainWorkOnTaskForm.ConstrainRB.IsEnabled = true;
                    MainWorkOnTaskForm.TarifsRB.IsEnabled = true;
                }
                else
                {
                    MainWorkOnTaskForm.TarifsRB.IsEnabled = false;
                    MainWorkOnTaskForm.ConstrainRB.IsEnabled = false;
                }
            }
            else
            {
                OffConnRB();
            }
        } 

        private void OffConnRB()
        {
            MainWorkOnTaskForm.PointsRB.IsEnabled = false;
            MainWorkOnTaskForm.TarifsRB.IsEnabled = false;
            MainWorkOnTaskForm.ConstrainRB.IsEnabled = false;
            IsClientsChose = false;
        }
    }
}
