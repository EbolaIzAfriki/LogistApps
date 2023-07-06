using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace DiplomAdminEditonBeta.Views.PagesTask
{
    /// <summary>
    /// Логика взаимодействия для PointsPage.xaml
    /// </summary>
    public partial class PointsPage : Page
    {
        MainWorkOnTaskForm MainWorkOnTaskForm;
        public PointsPage(MainWorkOnTaskForm mainWorkOnTaskForm)
        {
            InitializeComponent();
            MainWorkOnTaskForm = mainWorkOnTaskForm;
            PointProductCheck();
            PointAddressCheck();
        }

       public void UpdateDataGrid()
        {
            PointsDataGrid.ItemsSource = null;
            PointsDataGrid.ItemsSource = MainWorkOnTaskForm.DBTask.Point.ToList();
        }

        public void PointProductCheck()
        {
            if (MainWorkOnTaskForm.DBTask.Point.Count == 0)
                return;
            if (!MainWorkOnTaskForm.DBTask.Point.Select(p => p.ProductCount).Contains(0))
            {
                MainWorkOnTaskForm.TarifsRB.IsEnabled = true;
                MainWorkOnTaskForm.ConstrainRB.IsEnabled = true;
                IsAllProductCountFill = true;
            }
            else
            {
                MainWorkOnTaskForm.TarifsRB.IsEnabled = false;
                MainWorkOnTaskForm.ConstrainRB.IsEnabled = false;
                IsAllProductCountFill = false;
            }
        }

        public void PointAddressCheck()
        {
            if (MainWorkOnTaskForm.DBTask.Point.Count == 0)
                return;
            if (!MainWorkOnTaskForm.DBTask.Point.Select(p => string.IsNullOrEmpty(p.Address)).Contains(true))
            {
                IsAllAddressFill = true;
            }
            else
            {
                IsAllAddressFill = false;
            }
        }

        public static bool IsAllProductCountFill = false, IsAllAddressFill = false;

        private int position = 0;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(20) };
        private void PointsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Point point = PointsDataGrid.SelectedItem as Point;
            if (point == null)
                return;
            if (PointsDataGrid.CurrentCell.Column == null)
                return;
            int indexColumn = PointsDataGrid.CurrentCell.Column.DisplayIndex;
            position = MainWorkOnTaskForm.DBTask.Point.Select(p => p.Id).ToList().IndexOf(point.Id);
            string value = "";
            switch (indexColumn)
            {
                case 2:
                    {
                        point.Name = point.Name.Trim(' ');
                        if (string.IsNullOrEmpty(point.Name))
                        {
                            point.Name = string.Empty;
                            UpdateDataGrid();
                            return;
                        }
                        value = point.Name;
                        break;
                    }
                case 3:
                    {
                        point.Address = point.Address.Trim(' ');
                        if (string.IsNullOrEmpty(point.Address))
                        {
                            point.Address = string.Empty;
                            UpdateDataGrid();
                            return;
                        }
                        value = point.Address;

                        break;
                    }
                case 4:
                    {
                        if (point.ProductCount.ToString() == "")
                        {
                            point.ProductCount = 0;
                            UpdateDataGrid();
                            return;
                        }
                        value = point.ProductCount.ToString();
                        PointProductCheck();
                        break;
                    }
            }
            TCPMessege tCPMessege = new TCPMessege(4, 6, new List<string> {point.Id.ToString(), indexColumn.ToString(), value});
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }

            if (tCPMessege.CodeOperation == 0)
            {
                List<string> vs = JsonConvert.DeserializeObject<List<string>>(tCPMessege.Entity);
                MessageBox.Show("При изменении произошла ошибка: " + vs[0]);
                vs = JsonConvert.DeserializeObject<List<string>>(vs[1]);
                switch (int.Parse(vs[1]))
                {
                    case 2:
                        {
                            MainWorkOnTaskForm.DBTask.Point.ToList()[position].Name = vs[2];
                            break;
                        }
                    case 3:
                        {
                            MainWorkOnTaskForm.DBTask.Point.ToList()[position].Address = vs[2];
                            break;
                        }
                    case 4:
                        {
                            MainWorkOnTaskForm.DBTask.Point.ToList()[position].ProductCount = Int32.Parse(vs[2]);
                            break;
                        }
                }
                UpdateDataGrid();
            }
            else
            {
                if (indexColumn == 3)
                {
                    PointAddressCheck();
                    VendorsAndConsumptionsPage.IsModificate = true;
                }
            }
            dispatcherTimer.Tick += Timertick;
            dispatcherTimer.Start();
        }

        private void Timertick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            PointsDataGrid.SelectedIndex = position;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        
        {
            try
            {
                if (!(sender as ComboBox).IsMouseOver || PointsDataGrid.SelectedItem == null)
                    return;
                Client client = (Client)(sender as ComboBox).SelectedItem;
                Point point = PointsDataGrid.SelectedItem as Point;

                TCPMessege tCPMessege = new TCPMessege(41, 6, new List<int> {point.Id,client.Id});
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 1)
                    return;
                if(tCPMessege.CodeOperation == 0)
                {
                    MainWorkOnTaskForm.MainForm.clientPage.UpdateClientDatagrid();
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(tCPMessege.Entity), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    VendorsAndConsumptionsPage.ChosedClient.Remove(client);
                    point.Client = VendorsAndConsumptionsPage.ChosedClient.First(p => p.TypeId == client.TypeId);
                    MainWorkOnTaskForm.VendorsAndConsumptionsPage.UpdateChosedVendorsDG();
                    UpdateDataGrid();
                }
                else
                {
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(tCPMessege.Entity), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTB.Text == "")
            {
                UpdateDataGrid();
                return;
            }
            PointsDataGrid.ItemsSource = null;
            PointsDataGrid.ItemsSource = MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.CompanyName.ToLower().Contains(SearchTB.Text.ToLower()) || p.Name.ToLower().Contains(SearchTB.Text.ToLower()) || p.Address.ToLower().Contains(SearchTB.Text.ToLower()));
        }
    }
}
