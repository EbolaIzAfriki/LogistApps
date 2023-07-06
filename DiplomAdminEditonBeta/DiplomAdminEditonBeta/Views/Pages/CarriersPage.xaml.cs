using DiplomAdminEditonBeta.TCPModels;
using DiplomAdminEditonBeta.Views.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DiplomAdminEditonBeta
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class CarriersPage : Page
    {
        public static List<Service> Services, AllServices;
        public static List<Carrier> Carriers;
        private MainForm MainForm;
        public CarriersPage(MainForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            TCPMessege tCPMessege = new TCPMessege(1, 35, null);
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            List<string> recivedStrings = JsonConvert.DeserializeObject<List<string>>(tCPMessege.Entity);

            Services = JsonConvert.DeserializeObject<List<Service>>(recivedStrings[0]);
            AllServices = JsonConvert.DeserializeObject<List<Service>>(recivedStrings[0]);
            Carriers = JsonConvert.DeserializeObject<List<Carrier>>(recivedStrings[1]);

            CarriersDataGrid.ItemsSource = Carriers;
        }

        public void UpdateCarriersDatagridFromList()
        {
            CarriersDataGrid.ItemsSource = null;
            CarriersDataGrid.ItemsSource = Carriers;
        }

        public void UpdateDataGrid()
        {
            CarriersDataGrid.ItemsSource = null;

            TCPMessege tCPMessege = new TCPMessege(1, 3, null);
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            Carriers = JsonConvert.DeserializeObject<List<Carrier>>(tCPMessege.Entity);

            CarriersDataGrid.ItemsSource = Carriers;
        }

        public void UpdateServicesDataGrids()
        {
            if (CurrentCarrier == null)
                return;
            if (CurrentCarrier == null)
                return;
            ServiceDataGrid.ItemsSource = null;
            ServiceDataGrid.ItemsSource = CurrentCarrier.ServiceCarrier.ToList();
            StaticServiceDataGrid.ItemsSource = null;
            StaticServiceDataGrid.ItemsSource = Services.Where(p => !CurrentCarrier.ServiceCarrier.Select(n => n.Service.Id).Contains(p.Id)).ToList();
        }
        private void AddBut_Click(object sender, RoutedEventArgs e)
        {
            AddCarrierForm addCarrierForm = new AddCarrierForm(this);
            addCarrierForm.ShowDialog();
        }

        private void DeleteCarriersBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CarriersDataGrid.SelectedItem == null)
                    return;
                Carrier carrier = CarriersDataGrid.SelectedItem as Carrier;
                if (MessageBox.Show($"Вы действительно хотите удалить перевозчика {carrier.Name}?", "Удаление перевозчика", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }

                TCPMessege tCPMessege = new TCPMessege(5, 3, carrier.Id);
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 0)
                {
                    MessageBox.Show("Данный перевозчик еще используется в задачах!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Carriers.Remove(carrier);
                UpdateCarriersDatagridFromList();
                ClearServiseDG();
                MainForm.RequestPage.UpdateTaskList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MainForm.ReturnToAutorization();
                return;
            }
        }

        private void ClearServiseDG()
        {
            Services = AllServices.ToList();
            StaticServiceDataGrid.ItemsSource = null;
            ServiceDataGrid.ItemsSource = null;
        }

        private DispatcherTimer dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(20) };
        private int position = 0;
        private void CarriersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (CarriersDataGrid.SelectedItem == null)
                    return;
                Carrier carrier = CarriersDataGrid.SelectedItem as Carrier;

                if (CarriersDataGrid.CurrentCell.Column == null)
                    return;
                int indexColumn = CarriersDataGrid.CurrentCell.Column.DisplayIndex;
                string value = "";
                position = Carriers.IndexOf(carrier);
                switch (indexColumn)
                {
                    case 1:
                        {
                            carrier.Name = carrier.Name.Trim(' ');
                            if (string.IsNullOrEmpty(carrier.Name))
                            {
                                carrier.Name = string.Empty;
                                UpdateCarriersDatagridFromList();
                                CarriersDataGrid.SelectedIndex = position;
                                MessageBox.Show("Строка пуста!!!");
                                return;
                            }
                            value = carrier.Name;
                            break;
                        }
                    case 2:
                        {
                            carrier.Address = carrier.Address.Trim(' ');
                            if (string.IsNullOrEmpty(carrier.Address))
                            {
                                carrier.Address = string.Empty;
                                UpdateCarriersDatagridFromList();
                                CarriersDataGrid.SelectedIndex = position;
                                MessageBox.Show("Строка пуста!!!");
                                return;
                            }
                            value = carrier.Address;
                            break;
                        }
                    case 3:
                        {
                            carrier.Phone = carrier.Phone.Trim(' ');
                            if (string.IsNullOrEmpty(carrier.Name))
                            {
                                carrier.Phone = string.Empty;
                                UpdateCarriersDatagridFromList();
                                CarriersDataGrid.SelectedIndex = position;
                                MessageBox.Show("Строка пуста!!!");
                                return;
                            }
                            value = carrier.Phone;
                            break;
                        }
                    case 4:
                        {
                            carrier.Email = carrier.Email.Trim(' ');
                            if (string.IsNullOrEmpty(carrier.Name))
                            {
                                carrier.Email = string.Empty;
                                UpdateCarriersDatagridFromList();
                                CarriersDataGrid.SelectedIndex = position;
                                MessageBox.Show("Строка пуста!!!");
                                return;
                            }
                            value = carrier.Email;
                            break;
                        }
                }
                TCPMessege tCPMessege = new TCPMessege(4, 3, new List<string> { carrier.Id.ToString(), indexColumn.ToString(), value });
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == -1)
                {
                    MessageBox.Show("Этот перевозчик был удален!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateDataGrid();
                    ClearServiseDG();
                    position = -1;
                }
                if (tCPMessege.CodeOperation == 0)
                {
                    List<string> vs = JsonConvert.DeserializeObject<List<string>>(tCPMessege.Entity);
                    MessageBox.Show("При изменении произошла ошибка: " + vs[0]);
                    position = Carriers.IndexOf(carrier);
                    vs = JsonConvert.DeserializeObject<List<string>>(vs[1]);
                    switch (int.Parse(vs[1]))
                    {
                        case 1:
                            {
                                Carriers[position].Name = vs[2];
                                break;
                            }
                        case 2:
                            {
                                Carriers[position].Address = vs[2];
                                break;
                            }
                        case 3:
                            {
                                Carriers[position].Phone = vs[2];
                                break;
                            }
                        case 4:
                            {
                                Carriers[position].Email = vs[2];
                                break;
                            }
                    }
                    UpdateCarriersDatagridFromList();
                }
                dispatcherTimer.Tick += Timertick;
                dispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MainForm.ReturnToAutorization();
                return;
            }
        }

        private void Timertick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            CarriersDataGrid.SelectedIndex = position;
        }

        private void CarriersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CarriersDataGrid.SelectedItem == null)
                {
                    ClearServiseDG();
                    return;
                }
                Carrier carrier = CarriersDataGrid.SelectedItem as Carrier;
                ServiceDataGrid.ItemsSource = carrier.ServiceCarrier.ToList();
                StaticServiceDataGrid.ItemsSource = Services.Where(p => !carrier.ServiceCarrier.Select(n => n.IdService).Contains(p.Id)).ToList();
                CurrentCarrier = carrier;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private Carrier CurrentCarrier;
        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CarriersDataGrid.SelectedItem == null || StaticServiceDataGrid.SelectedItem == null)
                    return;
                Service service = StaticServiceDataGrid.SelectedItem as Service;
                Carrier carrier = (Carrier)CarriersDataGrid.SelectedItem;

                TCPMessege tCPMessege = new TCPMessege(3, 35, new List<int>() { service.Id, carrier.Id });
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 0)
                {
                    MessageBox.Show("Этот перевозчик был удален!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateDataGrid();
                    ClearServiseDG();
                    return;
                }
                CurrentCarrier.ServiceCarrier.Add(new ServiceCarrier() { Carrier = CurrentCarrier, Cost = 0, Service = service });

                UpdateServicesDataGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MainForm.ReturnToAutorization();
                return;
            }
        }

        private void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CarriersDataGrid.SelectedItem == null || ServiceDataGrid.SelectedItem == null)
                    return;
                ServiceCarrier serviceCarrier = ServiceDataGrid.SelectedItem as ServiceCarrier;
                if(serviceCarrier.Service.Id == 1)
                {
                    MessageBox.Show("Нельзя удалить базовую услугу перевозки!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TCPMessege tCPMessege = new TCPMessege(5, 35, new List<int> { serviceCarrier.Service.Id, serviceCarrier.Carrier.Id });
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if(tCPMessege.CodeOperation == 0)
                {
                    MessageBox.Show("Этот перевозчик был удален!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateDataGrid();
                    ClearServiseDG();
                    return;
                }

                CurrentCarrier.ServiceCarrier.Remove(serviceCarrier);
                UpdateServicesDataGrids();
            }
            catch
            {
                MainForm.ReturnToAutorization();
                return;
            }
           
        }

        private int lastCost;
        private void ServiceDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ServiceCarrier serviceCarrier = ServiceDataGrid.SelectedItem as ServiceCarrier;
            if (serviceCarrier == null)
                return;
            lastCost = serviceCarrier.Cost;
        }

        private void MaskedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CarriersDataGrid_CellEditEnding(sender, null);
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTB.Text == "")
            {
                UpdateCarriersDatagridFromList();
                return;
            }
            CarriersDataGrid.ItemsSource = null;
            CarriersDataGrid.ItemsSource = Carriers.Where(p => p.Name.ToLower().Contains(SearchTB.Text.ToLower()) || p.Address.ToLower().Contains(SearchTB.Text.ToLower()) || p.Email.ToLower().Contains(SearchTB.Text.ToLower()) || p.Phone.ToLower().Contains(SearchTB.Text.ToLower()));
        }

        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
            ClearServiseDG();
            SearchTB_TextChanged(null, null);
        }

        private void ServiceDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (ServiceDataGrid.SelectedItem == null)
                    return;
                ServiceCarrier serviceCarrier = ServiceDataGrid.SelectedItem as ServiceCarrier;
                if (lastCost == serviceCarrier.Cost)
                    return;
                if (serviceCarrier.Cost < 0)
                {
                    serviceCarrier.Cost *= -1;
                }
                TCPMessege tCPMessege = new TCPMessege(4, 35, new List<int> { serviceCarrier.Service.Id, serviceCarrier.Carrier.Id, serviceCarrier.Cost });
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 0)
                {
                    MessageBox.Show("Этот перевозчик был удален!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateDataGrid();
                    ClearServiseDG();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MainForm.ReturnToAutorization();
                return;
            }
        }
    }
}
