using DiplomAdminEditonBeta.Views.PagesTask;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DiplomAdminEditonBeta.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWorkOnTaskForm.xaml
    /// </summary>
    public partial class MainWorkOnTaskForm : Window
    {
        public static MainWorkOnTaskForm mainWorkStaticForm;
        public MainForm MainForm;

        public static Task DBTask = null;
        private int PositionTask;
        public MainWorkOnTaskForm(MainForm mainForm, Task task, int positionTask)
        {
            DBTask = task;
            NeedServisesAndChoseCarrierPage = new NeedServisesAndChoseCarrierPage();
            CreateConstrainPage = new ConstrainsPage();
            PositionTask = positionTask;
            InitializeComponent();
            mainWorkStaticForm = this;
            MainForm = mainForm;
            VendorsAndConsumptionsPage.IsModificate = true;

            PointsPage = new PointsPage(this);
            VendorsAndConsumptionsPage = new VendorsAndConsumptionsPage(PointsPage, this);
            TarifsAndPointsPage = new TarifsAndPointsPage(this);

            StatusTB.Text = "Статус: " + DBTask.Status.Name;

            MainFrame.Content = VendorsAndConsumptionsPage;
            StageTB.Text = "Этап 1: Добавление участников транспортной задачи";

            if (DBTask.TransportationCost.Count != 0)
            {
                if (DBTask.Conclusion != "")
                    ConclusionPage.ConclusionTB.Text = DBTask.Conclusion;
                int Counter = 0;
                for (int i = 0; i < VendorsAndConsumptionsPage.CountVendors; i++)
                {
                    for (int j = 0; j < VendorsAndConsumptionsPage.CountConsumptions; j++)
                    {
                        if (DBTask.TransportationCost.ToList().Count == Counter)
                            return;
                        if (DBTask.TransportationCost.ToList()[Counter].Value == 0)
                        {
                            TarifsAndPointsPage.IsAllTarifFill = false;
                            return;
                        }
                        Counter++;
                    }
                }
                TarifsAndPointsPage.IsAllTarifFill = true;
            }
        }

    public MainWorkOnTaskForm(MainForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            mainWorkStaticForm = this;
            TCPMessege tCPMessege = new TCPMessege(3, 4, mainForm.CurrentUser.Id);
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            if (tCPMessege.CodeOperation == 0)
            {
                MessageBox.Show("При создании задачи произошла ошибка: " + JsonConvert.DeserializeObject<string>(tCPMessege.Entity), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }
            DBTask = JsonConvert.DeserializeObject<Task>(tCPMessege.Entity);
            RequestPage.tasks.Add(DBTask);
            PositionTask = RequestPage.tasks.Count()-1;
            CreateConstrainPage = new ConstrainsPage();
            StatusTB.Text = "Статус: " + DBTask.Status.Name;

            PointsPage = new PointsPage(this);
            VendorsAndConsumptionsPage = new VendorsAndConsumptionsPage(PointsPage, this);
            TarifsAndPointsPage = new TarifsAndPointsPage(this);

            MainFrame.Content = VendorsAndConsumptionsPage;
            StageTB.Text = "Этап 1: Добавление участников транспортной задачи";
            NeedServisesAndChoseCarrierPage = new NeedServisesAndChoseCarrierPage();
        }

        NeedServisesAndChoseCarrierPage NeedServisesAndChoseCarrierPage;
        TarifsAndPointsPage TarifsAndPointsPage;
        ConstrainsPage CreateConstrainPage;
        ConclusionPage ConclusionPage = new ConclusionPage();
        PointsPage PointsPage;
        public VendorsAndConsumptionsPage VendorsAndConsumptionsPage;

        private void VendorsAndConsumersRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            MainFrame.Content = VendorsAndConsumptionsPage;
            StageTB.Text = "Этап 1: Добавление участников транспортной задачи";
        }

        private void CarriersRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            MainFrame.Content = NeedServisesAndChoseCarrierPage;
            StageTB.Text = "Этап 4: Выбор перевозчика и оказываемых дополнительных услуг";
        }

        private void TarifsRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            if (VendorsAndConsumptionsPage.IsModificate)
            {
                TarifsAndPointsPage.UpdateMatrix();
                VendorsAndConsumptionsPage.IsModificate = false;
            }
            MainFrame.Content = TarifsAndPointsPage;
            StageTB.Text = "Этап 5: Настройка тарифов перевозок";
        }

        private void ConclusionRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            MainFrame.Content = ConclusionPage;
            StageTB.Text = "Этап 6: Оптимизация и оценка стоимости перевозок";
        }

        private void ConstrainRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            CreateConstrainPage.UpdateList();
            MainFrame.Content = CreateConstrainPage;
            StageTB.Text = "Этап 3: Настройка ограничений";
        }

        private void PointsRB_Checked(object sender, RoutedEventArgs e)
        {
            if (MainFrame == null)
                return;
            PointsPage.UpdateDataGrid();
            MainFrame.Content = PointsPage;
            StageTB.Text = "Этап 2: Настройка пунктов снабжения и потребления";
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {           
            if (ClientTCP.IsConnected == false)
            {
                return;
            }

            MainForm.Visibility = Visibility.Visible;
            int statusId;
            if (DBTask.Conclusion == null)
                statusId = 1;
            else
                statusId = 2;

            TCPMessege tCPMessege = new TCPMessege(4, 10, new List<int> { DBTask.Id, statusId });
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                mainWorkStaticForm = null;
                MainForm.ReturnToAutorization();
                return;
            }
            Status status = JsonConvert.DeserializeObject<Status>(tCPMessege.Entity);
            DBTask.Status = status;
            RequestPage.tasks[PositionTask] = DBTask;
            MainForm.RequestPage.UpdateTasksFromList();
            DBTask = null;
        }
    }
}
