using SubsystemKKEP.Classes;
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

namespace SubsystemKKEP.AppPages.Administrator
{
    /// <summary>
    /// Логика взаимодействия для AppointmentsPage.xaml
    /// </summary>
    public partial class AppointmentsPage : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public AppointmentsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обновление списка назначений
        /// </summary>
        private void UpdateAppointment()
        {
            var currentAppointments = App.DataBase.Appointments.ToList();

            if (CmbSortGroup.SelectedIndex > 0)
            {
                currentAppointments = currentAppointments.Where(p => p.Group == CmbSortGroup.SelectedItem as Group).ToList();
            }
            currentAppointments = currentAppointments.Where(p => p.User.UserName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();
            if (currentAppointments.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }
            DGridAppointments.ItemsSource = currentAppointments;
        }

        /// <summary>
        /// При загрузке страницы - обновление данных
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var groups = App.DataBase.Groups.ToList();
            groups.Insert(0, new Group
            {
                GroupName = "Все группы"
            });
            CmbSortGroup.ItemsSource = groups;
            CmbSortGroup.SelectedIndex = 0;
            UpdateAppointment();
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу с добавлением назначения
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new AppointmentAddEditPage(null));
        }

        /// <summary>
        /// При нажатии на кнопку - удаление назначения
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnDeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            var appointmentRemoving = DGridAppointments.SelectedItems.Cast<Appointment>().ToList();
            if(appointmentRemoving.Count == 0)
            {
                MessageBox.Show($"Выберите пользователей", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"Вы точно хотите удалить {appointmentRemoving.Count} назначений?",
                "Подтверждение удаления", MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.DataBase.Appointments.RemoveRange(appointmentRemoving);
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateAppointment();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// При изменении содержимого TextBox - обновление списка назначений
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAppointment();
        }

        /// <summary>
        /// При изменении выбранной группы - обновление списка назначений
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAppointment();
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу с редактированием назначения
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnEditAppointment_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new AppointmentAddEditPage((sender as Button).DataContext as Appointment));
        }
    }
}
