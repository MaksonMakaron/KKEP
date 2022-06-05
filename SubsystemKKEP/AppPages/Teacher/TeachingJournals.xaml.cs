using SubsystemKKEP.Classes;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            UpdateJournals();
        }

        /// <summary>
        /// Обновление списка журналов
        /// </summary>
        public void UpdateJournals()
        {
            var appointments = App.DataBase.Appointments.
                Where(p => p.User.Id == InterfaceManagement.ManagementUser.Id && p.Group.IsArchive == false).ToList();
            if (appointments.Count == 0)
            {
                MessageBox.Show($"У вас нет дисциплин. Обратитесть к администратору", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                DGridDisciplines.IsEnabled = false;
                TbSearch.IsEnabled = false;
                return;
            }

            appointments = appointments.Where(p => p.Group.GroupName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();
            if (appointments.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }
            DGridDisciplines.ItemsSource = appointments;
        }

        /// <summary>
        /// При изменении содержимого TextBox - обновление списка журналов
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var disciplines = App.DataBase.Appointments.Where(p => p.User.Id == InterfaceManagement.ManagementUser.Id).ToList();

            disciplines = disciplines.Where(p => p.Group.GroupName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            DGridDisciplines.ItemsSource = disciplines;
        }

        /// <summary>
        /// При нажатии на кнопку - открытие страницы с журналом
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            var selectedJournal = (sender as Button).DataContext as Appointment;
            if (GetData.IsCountStudentsNotNull(selectedJournal.Group))
            {
                InterfaceManagement.ManagementHeaderText.Text = selectedJournal.Group.GroupName;
                InterfaceManagement.ManagementPage.Navigate(new GroupJournal(selectedJournal));
            }
            else
            {
                MessageBox.Show("Отсутствуют студенты в группе. Обратитесь к администратору", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }
    }
}
