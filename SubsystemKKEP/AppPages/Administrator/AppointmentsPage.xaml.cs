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
        public AppointmentsPage()
        {
            InitializeComponent();
        }

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

        private void BtnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new AppointmentAddEditPage(null));
        }

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

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAppointment();
        }

        private void CmbSortGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAppointment();
        }

        private void BtnEditAppointment_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new AppointmentAddEditPage((sender as Button).DataContext as Appointment));
        }
    }
}
