using DiplomAdminEditonBeta.TCPModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiplomAdminEditonBeta.Views.PagesTask
{
    /// <summary>
    /// Логика взаимодействия для ConstrainsPage.xaml
    /// </summary>
    public partial class ConstrainsPage : Page
    {
        public ConstrainsPage()
        {
            InitializeComponent();
            UpdateList();
        }

        private void ButtonAddConstrain_Click(object sender, RoutedEventArgs e)
        {
            TCPMessege tCPMessege = new TCPMessege(3, 9, MainWorkOnTaskForm.DBTask.Id);
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            Constraint сonstraint = JsonConvert.DeserializeObject<Constraint>(tCPMessege.Entity);
            MainWorkOnTaskForm.DBTask.Constraint.Add(сonstraint);
            UpdateList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            Constraint constraint = (Constraint)cmd.DataContext;
            TCPMessege tCPMessege = new TCPMessege(5, 9, constraint.Id);
            if (!ClientTCP.SendMessege(tCPMessege))
            {
                MainForm.ReturnToAutorization();
                return;
            }
            MainWorkOnTaskForm.DBTask.Constraint.Remove(constraint);
            UpdateList();
        }

        public void UpdateList()
        {
            ListBoxConstrain.ItemsSource = null;
            ListBoxConstrain.ItemsSource = MainWorkOnTaskForm.DBTask.Constraint.ToList();
        }
        private void ComboBoxVendors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender as ComboBox).IsMouseOver || (sender as ComboBox).SelectedItem == null)
                return;
            Constraint constraint = (Constraint)(sender as ComboBox).DataContext;
            Point point = (sender as ComboBox).SelectedItem as Point;
            TCPMessege tCPMessege = new TCPMessege(42, 9, new List<int> { constraint.Id, point.Position});
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            if (tCPMessege == null)
            {
                MainForm.ReturnToAutorization();
                return;
            }
            constraint.IdPoints = tCPMessege.Entity;
        }

        private void ComboBoxConsumers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender as ComboBox).IsMouseOver || (sender as ComboBox).SelectedItem == null)
                return;
            Constraint constraint = (Constraint)(sender as ComboBox).DataContext;
            Point point = (sender as ComboBox).SelectedItem as Point;
            TCPMessege tCPMessege = new TCPMessege(43, 9, new List<int> { constraint.Id, point.Position });
            tCPMessege = ClientTCP.SendMessegeAndGetAnswer(tCPMessege);
            constraint.IdPoints = tCPMessege.Entity;
        }

        private void ComboBoxConstrain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender as ComboBox).IsMouseOver || (sender as ComboBox).SelectedItem == null)
                return;

            Constraint constraint = (Constraint)(sender as ComboBox).DataContext;
            TypeConstraint typeConstraint = (sender as ComboBox).SelectedItem as TypeConstraint;

            TCPMessege tCPMessege = new TCPMessege(41, 9, new List<int> {constraint.Id, typeConstraint.Id});
            if (!ClientTCP.SendMessege(tCPMessege))
            {
                MainForm.ReturnToAutorization();
                return;
            }

            constraint.TypeConstraint = typeConstraint;
        }

        private void TextBoxCount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void TextBoxCount_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Constraint constraint = (Constraint)textBox.DataContext;
            int CountPro = int.Parse(textBox.Text);
            if (CountPro == 0)
                return;
            TCPMessege tCPMessege = new TCPMessege(44, 9, new List<int> { constraint.Id, CountPro });
            if (!ClientTCP.SendMessege(tCPMessege))
            {
                MainForm.ReturnToAutorization();
                return;
            }
            constraint.ProductCount = CountPro;
        }

        private void TextBoxCount_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(textBox), null);
                Keyboard.ClearFocus();
            }
        }
    }
}
