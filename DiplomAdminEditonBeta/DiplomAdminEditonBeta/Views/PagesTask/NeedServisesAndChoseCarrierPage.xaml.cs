using DiplomAdminEditonBeta.TCPModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DiplomAdminEditonBeta.Views.PagesTask
{
    /// <summary>
    /// Логика взаимодействия для NeedServisesAndChoseCarrierPage.xaml
    /// </summary>
    public partial class NeedServisesAndChoseCarrierPage : Page
    {
        List<Service> ChosedService = new List<Service>(), NotChosedService;
        public static bool IsCarrierChose = false;
        public NeedServisesAndChoseCarrierPage()
        {
            InitializeComponent();
            NotChosedService = CarriersPage.AllServices.ToList();
            ChosedService = MainWorkOnTaskForm.DBTask.Service.ToList();
            foreach (Service service in ChosedService)
            {
                NotChosedService.RemoveAll(p => p.Id == service.Id);
            }
            if (MainWorkOnTaskForm.DBTask.Carrier != null)
            {
                CurrentCarrierTB.Text = "Текущий поставщик: " + MainWorkOnTaskForm.DBTask.Carrier.Name;
                IsCarrierChose = true;
            }
            else
            {
                IsCarrierChose = false;
            }
            UpdateDatagrids();
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListServiceDataGrid.SelectedItem == null)
                return;
            Service service = ListServiceDataGrid.SelectedItem as Service;
            if(MainWorkOnTaskForm.DBTask.Carrier != null)
            {
                if (!MainWorkOnTaskForm.DBTask.Carrier.ServiceCarrier.Select(p => p.Service.Id).ToList().Contains(service.Id))
                {
                    MessageBox.Show("У выбранного перевозчика отсутствует данная услуга!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            ChosedService.Add(service);
            NotChosedService.Remove(service);

            MainWorkOnTaskForm.DBTask.Service.Add(service);

            TCPMessege tCPMessege = new TCPMessege(3, 7, new List<int> { MainWorkOnTaskForm.DBTask.Id, service.Id });
            if (!ClientTCP.SendMessege(tCPMessege))
            {
                MainForm.ReturnToAutorization();
                return;
            }

            UpdateDatagrids();
        }

        private void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChoseServiceDataGrid.SelectedItem == null)
                return;
            Service service = ChoseServiceDataGrid.SelectedItem as Service;
            if(service.Id == 1)
            {
                MessageBox.Show("Нельзя удалить базовую услугу перевозки!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            NotChosedService.Add(service);
            ChosedService.Remove(service);

            MainWorkOnTaskForm.DBTask.Service.Remove(service);

            TCPMessege tCPMessege = new TCPMessege(5, 7, new List<int> { MainWorkOnTaskForm.DBTask.Id, service.Id});
            if (!ClientTCP.SendMessege(tCPMessege))
            {
                MainForm.ReturnToAutorization();
                return;
            }

            UpdateDatagrids();
        }

        private void CarriersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarriersDataGrid.SelectedItem == null)
                return;
            Carrier carrier = CarriersDataGrid.SelectedItem as Carrier;
            TCPMessege tCPMessege = new TCPMessege(4, 7, new List<int> { MainWorkOnTaskForm.DBTask.Id, carrier.Id });
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            if (tCPMessege.CodeOperation == 1)
            {
                MainWorkOnTaskForm.DBTask.Carrier = carrier;
                CurrentCarrierTB.Text = "Текущий поставщик: " + carrier.Name;
                IsCarrierChose = true;
            }
            else
            {
                MessageBox.Show("Перевозчик был удален из системы!");
                RefreshList_Click(null, null);
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTB.Text == "")
            {
                UpdateDatagrids();
                return;
            }
            CarriersDataGrid.ItemsSource = null;
            CarriersDataGrid.ItemsSource = carriers.Where(p => p.Name.ToLower().Contains(SearchTB.Text.ToLower()) || p.Address.ToLower().Contains(SearchTB.Text.ToLower()) || p.Email.ToLower().Contains(SearchTB.Text.ToLower()) || p.Phone.ToLower().Contains(SearchTB.Text.ToLower()) || p.ListService.ToLower().Contains(SearchTB.Text.ToLower()));
        }
        private List<Carrier> carriers;

        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            MainWorkOnTaskForm.mainWorkStaticForm.MainForm.CarriersPage.UpdateDataGrid();
            UpdateDatagrids();
            SearchTB_TextChanged(null, null);
        }

        public void UpdateDatagrids()
        {
            ChoseServiceDataGrid.ItemsSource = null;
            ListServiceDataGrid.ItemsSource = null;
            CarriersDataGrid.ItemsSource = null;
            ChoseServiceDataGrid.ItemsSource = ChosedService;
            ListServiceDataGrid.ItemsSource = NotChosedService;

            carriers = new List<Carrier>();
            foreach (Carrier carrier in CarriersPage.Carriers)
            {
                bool NotContains = false;
                List<int> services = carrier.ServiceCarrier.Select(p => p.IdService).ToList();
                foreach (Service service in ChosedService)
                {
                    if (!services.Contains(service.Id))
                    {
                        NotContains = true;
                        break;
                    }
                }
                if (NotContains)
                    continue;
                carriers.Add(carrier);
            }
            CarriersDataGrid.ItemsSource = carriers;
        }
    }
}
