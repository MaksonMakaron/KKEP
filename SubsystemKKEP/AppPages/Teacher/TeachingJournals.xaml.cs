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
using SubsystemKKEP.Classes;

namespace SubsystemKKEP.AppPages.Teacher
{
    /// <summary>
    /// Логика взаимодействия для TeachingJournals.xaml
    /// </summary>
    public partial class TeachingJournals : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public TeachingJournals()
        {
            InitializeComponent();
            DGridDisciplines.ItemsSource = App.DataBase.Appointments.
                Where(p => p.User.Id == InterfaceManagement.ManagementUser.Id).ToList();
            if (DGridDisciplines.Items.Count == 0)
            {
                MessageBox.Show($"У вас нет дисциплин. Обратитесть к администратору", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                DGridDisciplines.IsEnabled = false;
                TbSearch.IsEnabled = false;
            }
        }

        /// <summary>
        /// Обновление списка дисциплин
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var disciplines = App.DataBase.Appointments.Where(p => p.User.Id == InterfaceManagement.ManagementUser.Id).ToList();

            disciplines = disciplines.Where(p => p.Group.GroupName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            DGridDisciplines.ItemsSource = disciplines;
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            var selectedJournal = (sender as Button).DataContext as Appointment;
            InterfaceManagement.ManagementHeaderText.Text = selectedJournal.Group.GroupName;
            InterfaceManagement.ManagementPage.Navigate(new GroupJournal(selectedJournal));
        }
    }
}
