using DiplomAdminEditonBeta.TCPModels;
using DiplomAdminEditonBeta.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiplomAdminEditonBeta
{
    /// <summary>
    /// Логика взаимодействия для RequestPage.xaml
    /// </summary>
    public partial class RequestPage : Page
    {
        private MainForm MainForm;
        public static List<Task> tasks;

        public RequestPage(MainForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            UpdateTaskList();
        }

        private void AddTaskBut_Click(object sender, RoutedEventArgs e)
        {
            MainWorkOnTaskForm mainWorkOnTaskForm = new MainWorkOnTaskForm(MainForm);
            mainWorkOnTaskForm.Show();
            MainForm.Visibility = Visibility.Hidden;
        }

        public void UpdateTaskList()
        {
            TasksDataGrid.ItemsSource = null;

            TCPMessege tCPMessege = new TCPMessege(1,4, null);
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            tasks = JsonConvert.DeserializeObject<List<Task>>(tCPMessege.Entity);

            TasksDataGrid.ItemsSource = tasks;
        }

        public void UpdateTasksFromList()
        {
            TasksDataGrid.ItemsSource = null;
            TasksDataGrid.ItemsSource = tasks;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TasksDataGrid.SelectedItem == null)
                    return;
                Task task = TasksDataGrid.SelectedItem as Task;
                if (MessageBox.Show("Вы действительно хотите удалить задачу?", "Удаление задачи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }

                if (task.Status.Id == 3)
                {
                    MessageBox.Show("Эта задача редактируется другим пользователем!!!");
                    return;
                }
                TCPMessege tCPMessege = new TCPMessege(1, 11, task.Id);
                tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
                if (tCPMessege == null)
                {
                    MainForm.ReturnToAutorization();
                    return;
                }
                if (tCPMessege.CodeOperation == 0)
                {
                    MessageBox.Show("Эта задача редактируется другим пользователем!!!");
                    SearchTB_TextChanged(null, null);
                    return;
                }

                tCPMessege = new TCPMessege(5, 4, task.Id);
                if (!ClientTCP.SendMessege(tCPMessege))
                {
                    MainForm.ReturnToAutorization();
                    return;
                }

                tasks.Remove(task);
                UpdateTasksFromList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MainForm.ReturnToAutorization();
                return;
            }
        }

        private void ClientsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TasksDataGrid.SelectedItem == null)
                return;
            Task task = TasksDataGrid.SelectedItem as Task;
            if(task.Status.Id == 3)
            {
                MessageBox.Show("Эта задача уже редактируется другим пользователем!!!");
                return;
            }
            TCPMessege tCPMessege = new TCPMessege(4,10, new List<int> {task.Id, 3});
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            if (tCPMessege.CodeOperation == 0)
            {
                Status status = JsonConvert.DeserializeObject<Status>(tCPMessege.Entity);
                task.Status = status;
                MessageBox.Show("Эта задача уже редактируется другим пользователем!!!");
                SearchTB_TextChanged(null, null);
                return;
            }
            MainWorkOnTaskForm mainWorkOnTaskForm = new MainWorkOnTaskForm(MainForm, task, tasks.IndexOf(task));
            mainWorkOnTaskForm.Show();
            MainForm.Visibility = Visibility.Hidden;
        }

        private void RefreshList_Click(object sender, RoutedEventArgs e)
        {
            UpdateTaskList();
            SearchTB_TextChanged(null, null);
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTB.Text == "")
            {
                UpdateTasksFromList();
                return;
            }
            TasksDataGrid.ItemsSource = null;
            TasksDataGrid.ItemsSource = tasks.Where(p => p.Status.Name.ToLower().Contains(SearchTB.Text.ToLower()) || p.CarrierName.ToLower().Contains(SearchTB.Text.ToLower()) || p.Clients.ToLower().Contains(SearchTB.Text.ToLower()) || p.User.Login.ToLower().Contains(SearchTB.Text.ToLower()));
        }
    }
}
