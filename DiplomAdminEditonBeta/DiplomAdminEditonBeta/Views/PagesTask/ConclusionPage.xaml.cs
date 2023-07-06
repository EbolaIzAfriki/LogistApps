using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DiplomAdminEditonBeta.Views.PagesTask
{
    /// <summary>
    /// Логика взаимодействия для ConclusionPage.xaml
    /// </summary>
    public partial class ConclusionPage : Page
    {
        public ConclusionPage()
        {
            InitializeComponent();
        }

        private Random random = new Random();

        private string Check()
        {
            string ErrorString = "";
            if (!VendorsAndConsumptionsPage.IsClientsChose)
            {
                ErrorString += "\nКлиенты или число пунктов не выбрано!";
            }
            if (!PointsPage.IsAllProductCountFill)
            {
                ErrorString += "\nЧисло товаров указано не у всех пунктов!";
            }
            if (!NeedServisesAndChoseCarrierPage.IsCarrierChose)
            {
                ErrorString += "\nПеревозчик не выбран!";
            }
            if (!TarifsAndPointsPage.IsAllTarifFill)
            {
                ErrorString += "\nМатрица тарифов заполнена не полностью!";
            }
            return ErrorString;
        }
        private bool AddAdress = true;
        private void GenerateConlusionBut_Click(object sender, RoutedEventArgs e)
        {
            string ErrorString = Check();
            if (ErrorString != "")
            {
                ErrorString = ErrorString.Substring(1);
                MessageBox.Show(ErrorString, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!PointsPage.IsAllAddressFill)
            {
                if (MessageBox.Show("У пунктов снабжения не все адреса заполнены, что может привести к неполному выводу. Вы действительно хотите продолжить?", "Предупреджение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
                AddAdress = false;
            }


            int[,] transpCostM, matrPost, transpCostMn;
            //Матрица булей для определения путей
            bool[,] matrPostB;

            List<Point> CopyVendors = MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 2).ToList(), CopyConsumers = MainWorkOnTaskForm.DBTask.Point.Where(p => p.Client.TypeId == 1).ToList();

            //Список пунктов поставщиков
            List<int> vendors = CopyVendors.Select(p => p.ProductCount).ToList();
            //Список пунктов потребителей
            List<int> consumers = CopyConsumers.Select(p => p.ProductCount).ToList();

            int CountFailure = 0;

            int sumCons = 0, sumVend = 0, row = vendors.Count, column = consumers.Count;

            for (int j = 0; j < row; j++)
            {
                sumVend += vendors[j];
            }

            for (int j = 0; j < column; j++)
            {
                sumCons += consumers[j];
            }
            int n = 0;
            List<TransportationCost> MatrixTarifs = MainWorkOnTaskForm.DBTask.TransportationCost.OrderBy(p => p.IdPosition).ToList();
            List<int> TranspCost = MatrixTarifs.Select(p => p.Value).ToList();
            int MaxTarif = 0;
            bool IsManyItemsVendors = true;
            //Равновесная задача
            if (sumVend == sumCons)
            {
                matrPost = new int[row, column];
                matrPostB = new bool[row, column];
                transpCostM = new int[row, column];

                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        transpCostM[i, j] = TranspCost[n];
                        matrPost[i, j] = 0;
                        if (MaxTarif < TranspCost[n])
                        {
                            MaxTarif = TranspCost[n];
                        }
                        n++;
                    }
                }
            }
            else
            {
                //Неравновесная задача
                if (sumVend < sumCons)
                {
                    IsManyItemsVendors = false;
                    vendors.Add(sumCons - sumVend);
                    row++;
                    transpCostM = new int[row, column];
                    matrPost = new int[row, column];
                    matrPostB = new bool[row, column];

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < column; j++)
                        {
                            if (i == row - 1)
                            {
                                transpCostM[i, j] = 0;
                            }
                            else
                            {
                                transpCostM[i, j] = TranspCost[n];
                                if (MaxTarif < TranspCost[n])
                                {
                                    MaxTarif = TranspCost[n];
                                }
                                n++;
                            }
                            matrPost[i, j] = 0;

                        }
                    }
                }
                else
                {
                    consumers.Add(sumVend - sumCons);
                    column++;
                    transpCostM = new int[row, column];
                    matrPost = new int[row, column];
                    matrPostB = new bool[row, column];

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < column; j++)
                        {
                            if (j == column-1)
                            {
                                transpCostM[i, j] = 0;
                            }
                            else
                            {
                                transpCostM[i, j] = TranspCost[n];
                                if (MaxTarif < TranspCost[n])
                                {
                                    MaxTarif = TranspCost[n];
                                }
                                n++;
                            }
                            matrPost[i, j] = 0;
                        }
                    }
                    CopyConsumers.Add(new Point());
                }
            }

            //Работа с ограничениями

            List<Constraint> constraints = MainWorkOnTaskForm.DBTask.Constraint.Select(p => new Constraint() { IdPoints = p.IdPoints, TypeConstraintId = p.TypeConstraintId, ProductCount = p.ProductCount }).ToList();

            foreach (Constraint constraint in constraints)
            {
                if (constraint.IdPoints == null)
                    continue;
                List<string> ListPoints = constraint.IdPoints.Split('&').ToList();
                if (ListPoints.Contains("-1"))
                    continue;
                int IdVendorPoint = int.Parse(ListPoints[0]), IdConsumerPoint = int.Parse(ListPoints[1]);

                int minC = vendors[IdVendorPoint - 1];
                if (minC > consumers[IdConsumerPoint - 1])
                {
                    minC = consumers[IdConsumerPoint - 1];
                }
                if (minC < constraint.ProductCount)
                    constraint.ProductCount = minC;

                if (constraint.TypeConstraintId == 0)
                {
                    vendors[IdVendorPoint - 1] = (int)(vendors[IdVendorPoint - 1] - constraint.ProductCount);
                    consumers[IdConsumerPoint - 1] = (int)(consumers[IdConsumerPoint - 1] - constraint.ProductCount);
                    continue;
                }
                if (constraint.TypeConstraintId == 2)
                {
                    vendors[IdVendorPoint - 1] = (int)(vendors[IdVendorPoint - 1] - constraint.ProductCount);
                    consumers[IdConsumerPoint - 1] = (int)(consumers[IdConsumerPoint - 1] - constraint.ProductCount);
                    transpCostM[IdVendorPoint - 1, IdConsumerPoint - 1] += MaxTarif*3;
                    continue;
                }
                if (constraint.TypeConstraintId == 1)
                {
                    n = 0;
                    TranspCost.Clear();
                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < column; j++)
                        {
                            TranspCost.Add(transpCostM[i, j]);
                        }
                    }

                    consumers.Add((int)constraint.ProductCount);
                    column++;
                    transpCostM = new int[row, column];
                    matrPost = new int[row, column];
                    matrPostB = new bool[row, column];
                    n = 0;

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < column; j++)
                        {
                            if (j == column - 1)
                            {
                                transpCostM[i, j] = transpCostM[i, IdConsumerPoint-1];
                            }
                            else
                            {
                                transpCostM[i, j] = TranspCost[n];
                                n++;
                            }
                            matrPost[i, j] = 0;
                        }
                    }
                    consumers[IdConsumerPoint-1] -= ((int)constraint.ProductCount);
                    CopyConsumers.Add(CopyConsumers[IdConsumerPoint - 1]);
                    transpCostM[IdVendorPoint-1, IdConsumerPoint-1] = MaxTarif*3;
                    continue;
                }
            }

            transpCostMn = (int[,])transpCostM.Clone();

            //Построение начального опорного плана
            int min, vendorsIndex, consumersIndex, bazis = 0;
            while (sumVend != 0 && sumCons != 0)
            {
                //Ищет минимальный элемент
                min = -1; vendorsIndex = 0; consumersIndex = 0;
                for (int i = 0; i < row; i++)
                    for (int j = 0; j < column; j++)
                    {
                        {
                            if (matrPost[i, j] != 0 || transpCostM[i, j] < 1)
                                continue;
                            if ((min == -1 || min > transpCostM[i, j]) && vendors[i] > 0 && consumers[j] > 0)
                            {
                                min = transpCostM[i, j];
                                vendorsIndex = i;
                                consumersIndex = j;
                                continue;
                            }
                        }
                    }

                if (min == -1)
                {
                    for (int i = 0; i < vendors.Count; i++)
                    {
                        if (vendors[i] != 0)
                        {
                            vendorsIndex = i;
                        }
                    }
                    for (int i = 0; i < consumers.Count; i++)
                    {
                        if (consumers[i] != 0)
                        {
                            consumersIndex = i;
                        }
                    }
                }

                if (vendors[vendorsIndex] == consumers[consumersIndex])
                {
                    matrPost[vendorsIndex, consumersIndex] = vendors[vendorsIndex];
                    vendors[vendorsIndex] = 0;
                    consumers[consumersIndex] = 0;
                    for (int i = 0; i < column; i++)
                    {
                        transpCostM[vendorsIndex, i] = -1;
                    }
                }
                else
                {
                    if (vendors[vendorsIndex] > consumers[consumersIndex])
                    {
                        matrPost[vendorsIndex, consumersIndex] = consumers[consumersIndex];
                        vendors[vendorsIndex] -= consumers[consumersIndex];
                        consumers[consumersIndex] = 0;
                        for (int i = 0; i < row; i++)
                        {
                            transpCostM[i, consumersIndex] = -1;
                        }
                    }
                    else
                    {
                        matrPost[vendorsIndex, consumersIndex] = vendors[vendorsIndex];
                        consumers[consumersIndex] -= vendors[vendorsIndex];
                        vendors[vendorsIndex] = 0;
                        for (int i = 0; i < column; i++)
                        {
                            transpCostM[vendorsIndex, i] = -1;
                        }
                    }
                }
                sumVend = 0;
                sumCons = 0;
                for (int j = 0; j < vendors.Count; j++)
                {
                    sumVend += vendors[j];
                }
                for (int j = 0; j < consumers.Count; j++)
                {
                    sumCons += consumers[j];
                }
                bazis++;
            }
            transpCostM = (int[,])transpCostMn.Clone();

            int badTryFindPotencials = 0;
            restart:
            transpCostMn = (int[,])transpCostM.Clone();
            bool badmark;
            do
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        ///Замена здесь
                        if (matrPost[i, j] == 0 && transpCostMn[i, j] >= 0)
                        {
                            transpCostMn[i, j] = -1;
                            matrPostB[i, j] = false;
                        }
                        else
                        {
                            matrPostB[i, j] = true;
                        }
                    }
                }

                bool Virogdenniy = false;
                badmark = false;
                //Введение случайной клетки в базис
                if (vendors.Count + consumers.Count - 1 != bazis)
                {
                    Virogdenniy = true;
                    int randomI, randomJ;
                    while (true)
                    {
                        randomI = random.Next(0, row);
                        randomJ = random.Next(0, column);
                        if (matrPost[randomI, randomJ] == 0 && transpCostMn[randomI, randomJ] == -1)
                        {
                            transpCostMn[randomI, randomJ] = transpCostM[randomI, randomJ];
                            matrPostB[randomI, randomJ] = true;
                            break;
                        }
                    }
                }

                //Поиск потенциалов
                int[] Us = new int[row];
                int[] Vs = new int[column];

                bool[] Usb = new bool[row];
                bool[] Vsb = new bool[column];

                Usb[0] = true;
                Us[0] = 0;

                //Ищем первую строку
                for (int i = 0; i < column; i++)
                {
                    if (transpCostMn[0, i] != -1)
                    {
                        Vs[i] = transpCostMn[0, i];
                        Vsb[i] = true;
                    }
                }


                for (int i = 0; i < column; i++)
                {
                    if (Vsb[i] == true)
                    {
                        for (int j = 0; j < row; j++)
                        {
                            if (transpCostMn[j, i] != -1)
                            {
                                Us[j] = transpCostMn[j, i] - Vs[i];
                                Usb[j] = true;
                            }
                        }
                    }
                }

                bool searchCool = true;
                while (searchCool)
                {
                    searchCool = false;
                    if (Usb.Contains(false) || Vsb.Contains(false))
                    {
                        for (int i = 0; i < column; i++)
                        {
                            for (int j = 0; j < row; j++)
                            {
                                if (transpCostMn[j, i] != -1 && ((Usb[j] == true && Vsb[i] == false) || (Usb[j] == false && Vsb[i] == true)))
                                {
                                    if (Usb[j] == false)
                                    {
                                        Us[j] = transpCostMn[j, i] - Vs[i];
                                        Usb[j] = true;
                                        searchCool = true;
                                        if (!Usb.Contains(false) && !Vsb.Contains(false))
                                            break;
                                    }
                                    else
                                    {
                                        Vs[i] = transpCostMn[j, i] - Us[j];
                                        Vsb[i] = true;
                                        searchCool = true;
                                        if (!Usb.Contains(false) && !Vsb.Contains(false))
                                            break;
                                    }
                                }
                            }
                            if (!Usb.Contains(false) && !Vsb.Contains(false))
                                break;
                        }
                    }
                    else
                        break;
                }

                if (Usb.Contains(false) || Vsb.Contains(false))
                {
                    if (badTryFindPotencials == 20)
                    {
                        break;
                    }
                    badTryFindPotencials++;
                    goto restart;
                }

                //Рассчет оценок
                int[] position = new int[2];

                List<string> Marks = new List<string>();

                int maxmark = -1;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < column; j++)
                    {
                        if (transpCostMn[i, j] == -1)
                        {
                            int mark = Us[i] + Vs[j] - transpCostM[i, j];
                            if (mark > 0 && mark > maxmark)
                            {
                                maxmark = mark;
                                badmark = true;
                                position[0] = i;
                                position[1] = j;
                            }
                            Marks.Add(mark.ToString());
                        }
                        else
                            Marks.Add("");
                    }
                }

                //Поиск цикла (По квадрату)
                if (badmark)
                {
                    bool cycleReady = false;
                    int[,] positions = new int[4, 2] { { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 } };
                    positions[0, 0] = position[0];
                    positions[0, 1] = position[1];

                    for (int i = 0; i < row; i++)
                    {
                        //ищем Вертикаль
                        if (matrPostB[i, positions[0, 1]])
                        {
                            if (positions[0, 0] == i)
                                continue;
                            positions[1, 1] = positions[0, 1];
                            positions[1, 0] = i;
                            for (int j = 0; j < column; j++)
                            {
                                if (matrPostB[i, j])
                                {
                                    if (positions[1, 1] == j)
                                        continue;
                                    positions[2, 0] = i;
                                    positions[2, 1] = j;
                                    if (matrPostB[positions[0, 0], positions[2, 1]])
                                    {
                                        positions[3, 1] = positions[2, 1];
                                        positions[3, 0] = positions[0, 0];
                                        cycleReady = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (cycleReady)
                            break;
                    }
                    if (!cycleReady)
                    {
                        for (int j = 0; j < column; j++)
                        {
                            if (matrPostB[positions[0, 0], j])
                            {
                                if (positions[0, 1] == j)
                                    continue;
                                positions[1, 0] = positions[0, 0];
                                positions[1, 1] = j;
                                for (int i = 0; i < row; i++)
                                {
                                    if (matrPostB[i, j])
                                    {
                                        if (positions[1, 0] == i)
                                            continue;
                                        positions[2, 0] = i;
                                        positions[2, 1] = j;
                                        if (matrPostB[positions[0, 0], positions[2, 1]])
                                        {
                                            positions[3, 1] = positions[2, 1];
                                            positions[3, 0] = positions[0, 0];
                                            cycleReady = true;
                                            break;
                                        }
                                    }
                                }
                                if (cycleReady)
                                    break;
                            }
                        }

                    }
                    if (!cycleReady && Virogdenniy && CountFailure < 20)
                    {
                        CountFailure++;
                        goto restart;
                    }
                    if (!cycleReady)
                        break;


                    //Перераспределение поставок (По квадрату)
                    int minimum;

                    if (matrPost[positions[1, 0], positions[1, 1]] > matrPost[positions[3, 0], positions[3, 1]])
                    {
                        minimum = matrPost[positions[3, 0], positions[3, 1]];
                    }
                    else
                    {
                        minimum = matrPost[positions[1, 0], positions[1, 1]];
                    }

                    matrPost[positions[0, 0], positions[0, 1]] += minimum;
                    matrPost[positions[1, 0], positions[1, 1]] -= minimum;
                    matrPost[positions[2, 0], positions[2, 1]] += minimum;
                    matrPost[positions[3, 0], positions[3, 1]] -= minimum;
                }
            }
            while (badmark);

            foreach (Constraint constraint in constraints.Where(p => p.TypeConstraintId == 0 || p.TypeConstraintId == 2).ToList())
            {
                if (constraint.IdPoints == null)
                    continue;
                List<string> ListPoints = constraint.IdPoints.Split('&').ToList();
                if (ListPoints.Contains("-1"))
                    continue;
                int IdVendorPoint = int.Parse(ListPoints[0]), IdConsumerPoint = int.Parse(ListPoints[1]);
                matrPost[IdVendorPoint - 1, IdConsumerPoint - 1] += (int)constraint.ProductCount;
                if (constraint.TypeConstraintId == 2)
                {
                    transpCostM[IdVendorPoint - 1, IdConsumerPoint - 1] -= MaxTarif*3;
                }
            }

            string UnmetNeedsText = "";
            ConclusionTB.Text = "Для минимальной стоимости перевозки груза необходимо осуществить следующие поставки: \n";
            int Sum = 0, Count = 0, UnSum = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (matrPost[i, j] != 0)
                    {
                        if(transpCostM[i, j] == 0)
                        {
                            UnSum += matrPost[i, j];
                            if (IsManyItemsVendors)
                            {
                                UnmetNeedsText += $"       не удалось вывезти груз из пункта «{CopyVendors[i].Name}» в размере {matrPost[i, j]} единиц из-за недостатка запросов потребителей;\n";
                            }
                            else
                            {
                                UnmetNeedsText += $"       не удалось поставить груз в пункт «{CopyConsumers[j].Name}» в размере {matrPost[i, j]} единиц из-за недостатка товаров на складах;\n";
                            }
                            continue;
                        }
                        Sum += matrPost[i, j] * transpCostM[i, j];
                        if (AddAdress)
                        {
                            ConclusionTB.Text += $"       осуществить поставку груза из пункта «{CopyVendors[i].Name}» по адресу {CopyVendors[i].Address} в пункт «{CopyConsumers[j].Name}» по адресу {CopyConsumers[j].Address} в размере {matrPost[i, j]} единиц;\n";
                        }
                        else
                        {
                            ConclusionTB.Text += $"       осуществить поставку груза из пункта «{CopyVendors[i].Name}» в пункт «{CopyConsumers[j].Name}» в размере {matrPost[i, j]} единиц;\n";
                        }
                        Count++;
                    }
                }
            }

            ConclusionTB.Text += "\nСтоимость провоза: " + Sum.ToString();
            Count *= MainWorkOnTaskForm.DBTask.Carrier.ServiceCarrier.First(p => p.IdService == 1).Cost;
            ConclusionTB.Text += "\nСумма доставки перевозчика: " + Count;
            Sum += Count;
            if(MainWorkOnTaskForm.DBTask.Service.Count > 1)
            {
                ConclusionTB.Text += "\n\nРасходы на доп услуги: ";
                foreach(Service service in MainWorkOnTaskForm.DBTask.Service)
                {
                    Count = MainWorkOnTaskForm.DBTask.Carrier.ServiceCarrier.First(p => p.IdService == service.Id).Cost;
                    ConclusionTB.Text += $"\n       {service.Name}: " + Count;
                    Sum += Count;
                }
            }
            ConclusionTB.Text += $"\n\nИтого: " + Sum;
            ConclusionTB.Text += $"\n\nНеудовлетворенные потребности: \n" + UnmetNeedsText + "\nОбщая нехватка: " + UnSum;


            MainWorkOnTaskForm.DBTask.Conclusion = ConclusionTB.Text;
            MainWorkOnTaskForm.DBTask.Cost = Sum;
            MainWorkOnTaskForm.DBTask.Status.Name = "Обработана";
            MainWorkOnTaskForm.mainWorkStaticForm.StatusTB.Text = "Статус: " + MainWorkOnTaskForm.DBTask.Status.Name;

            TCPMessege tCPMessege = new TCPMessege(4, 4, new List<string> { MainWorkOnTaskForm.DBTask.Id.ToString(), ConclusionTB.Text, Sum.ToString()});
            if (!ClientTCP.SendMessege(tCPMessege))
            {
                MainForm.ReturnToAutorization();
                return;
            }
        }
    }
}
