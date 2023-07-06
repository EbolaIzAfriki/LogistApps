using DiplomAdminEditonBeta.Matrix_Models;
using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiplomAdminEditonBeta.Views.PagesTask
{
    /// <summary>
    /// Логика взаимодействия для TarifsAndPointsPage.xaml
    /// </summary>
    public partial class TarifsAndPointsPage : Page
    {
        public MatrixModel TarifMatrix { get; set; }

        private bool SaveProfile = false;
        public int LastColumns = 0, LastRows = 0;

        public static bool IsAllTarifFill = false;

        MainWorkOnTaskForm MainWorkOnTaskForm;
        private JsonSerializerSettings JsonSettings;
        public TarifsAndPointsPage(MainWorkOnTaskForm mainWorkOnTaskForm)
        {
            InitializeComponent();
            JsonSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };
            MainWorkOnTaskForm = mainWorkOnTaskForm;
        }
        public void UpdateMatrix()
        {
            if (SaveProfile)
            {
                LastColumns = TarifMatrix.Columns;
                LastRows = TarifMatrix.Rows;

                TarifMatrix = new MatrixModel() { Columns = VendorsAndConsumptionsPage.CountConsumptions + 2, Rows = VendorsAndConsumptionsPage.CountVendors + 2 };

                TarifsMatrixLV.DataContext = null;
                TarifsMatrixLV.ItemsSource = null;
            }
            else
            {
                TarifMatrix = new MatrixModel() { Columns = VendorsAndConsumptionsPage.CountConsumptions + 2, Rows = VendorsAndConsumptionsPage.CountVendors + 2 };
                LastColumns = TarifMatrix.Columns;
                LastRows = TarifMatrix.Rows;
                SaveProfile = true;
            }
            FillMatrix();
            TarifsMatrixLV.DataContext = TarifMatrix;
            TarifsMatrixLV.ItemsSource = TarifMatrix.Items;
        }

        public void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int Counter = 0;
            bool Flag = true, IsEnable = true;
            for (int i = 1; i < TarifMatrix.Rows - 1; i++)
            {
                for (int j = 1; j < TarifMatrix.Columns - 1; j++)
                {
                    int position = TarifMatrix.Columns * i + j;
                    if (Flag)
                    {
                        if (int.Parse(TarifMatrix.Items[position].Value) != transportationCosts[Counter].Value)
                        {
                            transportationCosts[Counter].Value = int.Parse(TarifMatrix.Items[position].Value);
                            TCPMessege tCPMessege = new TCPMessege(4, 8, new List<int>() { transportationCosts[Counter].Id, transportationCosts[Counter].Value});
                            if (!ClientTCP.SendMessege(tCPMessege))
                            {
                                MainForm.ReturnToAutorization();
                                return;
                            }
                            Flag = false;
                        }
                    }
                    if(transportationCosts[Counter].Value == 0)
                    {
                        IsEnable = false;
                    }
                    Counter++;
                }
            }
            IsAllTarifFill = IsEnable;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                labalFoc.Focus();
            }
        }
        List<TransportationCost> transportationCosts = new List<TransportationCost>();

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        public void FillMatrix()
        {
            TarifMatrix = new MatrixModel() { Columns = VendorsAndConsumptionsPage.CountConsumptions + 2, Rows = VendorsAndConsumptionsPage.CountVendors + 2 };
            TarifsMatrixLV.DataContext = null;
            TarifsMatrixLV.ItemsSource = null;
            TCPMessege tCPMessege;
            //Наконец Заполнение поставки
            int CurrentPositionCellInDatabase = -1, PositonInTransCost = 0;
            if (MainWorkOnTaskForm.DBTask.TransportationCost.Count > 0)
            {
                transportationCosts = MainWorkOnTaskForm.DBTask.TransportationCost.OrderBy(p => p.IdPosition).ToList();

                //Вывод (Создание новыых поставок) в соответствии с изменениями
                if(MainWorkOnTaskForm.DBTask.CountColumn != TarifMatrix.Columns-2 || MainWorkOnTaskForm.DBTask.CountRow != TarifMatrix.Rows-2)
                {
                    int CountTransCost = transportationCosts.Count-1;
                    List<TransportationCost> SaveTransportationCosts = new List<TransportationCost>();
                    List<TransportationCost> DeleteTransportationCosts = new List<TransportationCost>();

                    for (int i = 1; i < TarifMatrix.Rows - 1; i++)
                    {
                        for (int j = 1; j < TarifMatrix.Columns - 1; j++)
                        {
                            CurrentPositionCellInDatabase++;
                            int position = TarifMatrix.Columns * i + j;

                            if (j == MainWorkOnTaskForm.DBTask.CountColumn+1 || i == MainWorkOnTaskForm.DBTask.CountRow+1 || !SaveProfile)
                            {
                                foreach(TransportationCost transportationCost in transportationCosts)
                                {
                                    if(transportationCost.IdPosition >= CurrentPositionCellInDatabase)
                                    {
                                        transportationCost.IdPosition++;
                                    }
                                }
                                SaveTransportationCosts.Add(new TransportationCost() { Value = 0, IdPosition = CurrentPositionCellInDatabase });
                                TarifMatrix.Items[position].Value = "0";

                                continue;
                            }
                            if (PositonInTransCost >= transportationCosts.Count)
                            {
                                SaveTransportationCosts.Add(new TransportationCost() { Value = 0, IdPosition = CurrentPositionCellInDatabase });
                                TarifMatrix.Items[position].Value = "0";
                                continue;
                            }
                            DeleteTransportationCosts.Add(transportationCosts[PositonInTransCost]);
                            TarifMatrix.Items[position].Value = transportationCosts[PositonInTransCost].Value.ToString();
                            PositonInTransCost++;
                        }
                        if (MainWorkOnTaskForm.DBTask.CountColumn > TarifMatrix.Columns - 2)
                        {
                            int p = MainWorkOnTaskForm.DBTask.CountColumn.Value;
                            PositonInTransCost  +=  p - (TarifMatrix.Columns - 2);
                        }

                    }
                    MainWorkOnTaskForm.DBTask.CountColumn = TarifMatrix.Columns - 2;
                    MainWorkOnTaskForm.DBTask.CountRow = TarifMatrix.Rows - 2;
                    tCPMessege = new TCPMessege(41, 8, new List<int> { MainWorkOnTaskForm.DBTask.Id, (int)MainWorkOnTaskForm.DBTask.CountRow, (int)MainWorkOnTaskForm.DBTask.CountColumn });
                    if (!ClientTCP.SendMessege(tCPMessege))
                    {
                        MainForm.ReturnToAutorization();
                        return;
                    }
                    Thread.Sleep(500);
                    if ((transportationCosts.Count+SaveTransportationCosts.Count) == (TarifMatrix.Rows - 2) * (TarifMatrix.Columns - 2))
                    {
                        string TCListString = JsonConvert.SerializeObject(transportationCosts, Formatting.Indented, JsonSettings);
                        tCPMessege = new TCPMessege(5, 8, new List<string>() { MainWorkOnTaskForm.DBTask.Id.ToString(), TCListString });
                        if (!ClientTCP.SendMessege(tCPMessege))
                        {
                            MainForm.ReturnToAutorization();
                            return;
                        }
                        Thread.Sleep(1000);


                        transportationCosts.AddRange(SaveTransportationCosts);
                        transportationCosts = transportationCosts.OrderBy(p => p.IdPosition).ToList();

                        TCListString = JsonConvert.SerializeObject(transportationCosts, Formatting.Indented, JsonSettings);
                        tCPMessege = new TCPMessege(3, 8, new List<string>() { MainWorkOnTaskForm.DBTask.Id.ToString(), TCListString });
                        tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                        if (tCPMessege == null)
                        {
                            MainForm.ReturnToAutorization();
                            return;
                        }
                        transportationCosts = JsonConvert.DeserializeObject<List<TransportationCost>>(tCPMessege.Entity);
                        MainWorkOnTaskForm.DBTask.TransportationCost = transportationCosts;
                    }
                    else
                    {
                        int n = 0;
                        foreach (TransportationCost transportationCost in DeleteTransportationCosts)
                        {
                            transportationCost.IdPosition = n;
                            n++;
                        }

                        string TCListString = JsonConvert.SerializeObject(transportationCosts, Formatting.Indented, JsonSettings);
                        tCPMessege = new TCPMessege(5, 8, new List<string>() { MainWorkOnTaskForm.DBTask.Id.ToString(), TCListString });
                        if (!ClientTCP.SendMessege(tCPMessege))
                        {
                            MainForm.ReturnToAutorization();
                            return;
                        }
                        Thread.Sleep(1000);

                        DeleteTransportationCosts.AddRange(SaveTransportationCosts);
                        DeleteTransportationCosts = DeleteTransportationCosts.OrderBy(p => p.IdPosition).ToList();

                        TCListString = JsonConvert.SerializeObject(DeleteTransportationCosts, Formatting.Indented, JsonSettings);
                        tCPMessege = new TCPMessege(3, 8, new List<string>() { MainWorkOnTaskForm.DBTask.Id.ToString(), TCListString });
                        tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                        if (tCPMessege == null)
                        {
                            MainForm.ReturnToAutorization();
                            return;
                        }
                        transportationCosts = JsonConvert.DeserializeObject<List<TransportationCost>>(tCPMessege.Entity);
                        MainWorkOnTaskForm.DBTask.TransportationCost = transportationCosts;
                    }
                }
                else
                {
                    transportationCosts = transportationCosts.OrderBy(p => p.IdPosition).ToList();
                    for (int i = 1; i < TarifMatrix.Rows - 1; i++)
                    {
                        for (int j = 1; j < TarifMatrix.Columns - 1; j++)
                        {
                            CurrentPositionCellInDatabase++;
                            int position = TarifMatrix.Columns * i + j;
                            TarifMatrix.Items[position].Value = transportationCosts[CurrentPositionCellInDatabase].Value.ToString();
                        }
                    }
                }
                TarifsMatrixLV.DataContext = TarifMatrix;
                TarifsMatrixLV.ItemsSource = TarifMatrix.Items;
            }
            else
            {
                transportationCosts = new List<TransportationCost>();
                for (int i = 1; i < TarifMatrix.Rows - 1; i++)
                {
                    for (int j = 1; j < TarifMatrix.Columns - 1; j++)
                    {
                        CurrentPositionCellInDatabase++;
                        int position = TarifMatrix.Columns * i + j;
                        transportationCosts.Add(new TransportationCost() { Value = 0, IdPosition = CurrentPositionCellInDatabase});
                        TarifMatrix.Items[position].Value = "0";
                    }
                }
                MainWorkOnTaskForm.DBTask.CountColumn = TarifMatrix.Columns - 2;
                MainWorkOnTaskForm.DBTask.CountRow = TarifMatrix.Rows - 2;
                tCPMessege = new TCPMessege(41, 8, new List<int> { MainWorkOnTaskForm.DBTask.Id, (int)MainWorkOnTaskForm.DBTask.CountRow, (int)MainWorkOnTaskForm.DBTask.CountColumn });
                if (!ClientTCP.SendMessege(tCPMessege))
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                Thread.Sleep(500);

                

                string TCListString = JsonConvert.SerializeObject(transportationCosts, Formatting.Indented, JsonSettings);
                tCPMessege = new TCPMessege(3, 8, new List<string>() { MainWorkOnTaskForm.DBTask.Id.ToString(), TCListString });
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                transportationCosts = JsonConvert.DeserializeObject<List<TransportationCost>>(tCPMessege.Entity);
                MainWorkOnTaskForm.DBTask.TransportationCost = transportationCosts;
            }

            TarifMatrix.Items[0].Value = "Наименование пунктов";
            TarifMatrix.Items[0].IsNotEnable = true;
            TarifMatrix.Items[TarifMatrix.Items.Count - 1].IsNotEnable = true;

            TarifMatrix.Items[TarifMatrix.Columns - 1].Value = "Запасы";
            TarifMatrix.Items[TarifMatrix.Columns - 1].IsNotEnable = true;

            TarifMatrix.Items[TarifMatrix.Columns * (TarifMatrix.Rows - 1)].Value = "Потребности";
            TarifMatrix.Items[TarifMatrix.Columns * (TarifMatrix.Rows - 1)].IsNotEnable = true;

            List<Point> ConsumersNames = MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 1).ToList();
            List<Point> VendorsNames = MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 2).ToList();

            //Заполнение наименований
            for (int i = 1; i < TarifMatrix.Columns - 1; i++)
            {
                TarifMatrix.Items[i].Value = ConsumersNames[i - 1].Name;
                TarifMatrix.Items[i].IsNotEnable = true;
            }
            for (int j = 1; j < TarifMatrix.Rows - 1; j++)
            {

                TarifMatrix.Items[TarifMatrix.Columns * j].Value = VendorsNames[j - 1].Name;
                TarifMatrix.Items[TarifMatrix.Columns * j].IsNotEnable = true;
            }

            //Заполнение состояние складов
            for (int i = 1; i < TarifMatrix.Columns - 1; i++)
            {
                TarifMatrix.Items[TarifMatrix.Columns * (TarifMatrix.Rows - 1) + i].Value = ConsumersNames[i - 1].ProductCount.ToString();
                TarifMatrix.Items[TarifMatrix.Columns * (TarifMatrix.Rows - 1) + i].IsNotEnable = true;
            }
            for (int j = 2; j < TarifMatrix.Rows; j++)
            {
                TarifMatrix.Items[TarifMatrix.Columns * j - 1].Value = VendorsNames[j - 2].ProductCount.ToString();
                TarifMatrix.Items[TarifMatrix.Columns * j - 1].IsNotEnable = true;
            }
        }
    }
}
