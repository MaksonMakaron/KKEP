﻿using SubsystemKKEP.Classes;
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
    /// Логика взаимодействия для AppointmentAddEditPage.xaml
    /// </summary>
    public partial class AppointmentAddEditPage : Page
    {
        /// <summary>
        /// Текущее назначение
        /// </summary>
        private Appointment currentAppointment = new Appointment();

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="appointment">выбранное назначение</param>
        public AppointmentAddEditPage(Appointment appointment)
        {
            InitializeComponent();
            if (appointment != null)
            {
                currentAppointment = appointment;
            }
            CmbDiscipline.ItemsSource = App.DataBase.Disciplines.OrderBy(p => p.DisciplineName).ToList();
            CmbUser.ItemsSource = App.DataBase.Users.Where(p => p.Role.RoleName == "Преподаватель").OrderBy(p => p.UserName).ToList();
            CmbGroup.ItemsSource = App.DataBase.Groups.OrderBy(p => p.GroupNumber).ToList();
            DataContext = currentAppointment;
        }

        /// <summary>
        /// При нажатии на кнопку - переход на предыдущую страницу
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }

        /// <summary>
        /// При нажатии на кнопку - сохранение информации
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (CmbDiscipline.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите дисциплину");
            }
            if (CmbUser.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите преподавателя");
            }
            if (CmbGroup.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите группу");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (currentAppointment.IdUser == 0)
            {
                try
                {
                    App.DataBase.Appointments.Add(currentAppointment);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            try
            {
                App.DataBase.SaveChanges();
                MessageBox.Show("Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                InterfaceManagement.ManagementPage.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
