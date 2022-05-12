using SubsystemKKEP.Classes;
using SubsystemKKEP.AppPages.Administrator;
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
using System.Windows.Shapes;

namespace SubsystemKKEP.AppWindows
{
    /// <summary>
    /// Логика взаимодействия для AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public AdministratorWindow()
        {
            InitializeComponent();
            InterfaceManagement.ManagementPage = AdministratorFrame;
            InterfaceManagement.ManagementPage.Navigate(new AppPages.Account());
            InterfaceManagement.ManagementHeaderText = TextBlockHeader;
            InterfaceManagement.ManagementImgHeader = ImgHeader;
        }

        /// <summary>
        /// По клику происходит переход на страницу аккаунта
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Личный кабинет";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/User.png"));
            InterfaceManagement.ManagementPage.Navigate(new AppPages.Account());
        }

        /// <summary>
        /// По клику на кнопку - выход из аккаунта
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSignOut_Click(object sender, RoutedEventArgs e)
        {
            if (InterfaceManagement.SignOut())
            {
                this.Close();
            }
        }

        /// <summary>
        /// По клику на кнопку - переход на страницу пользователей
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Пользователи";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Users.png"));
            InterfaceManagement.ManagementPage.Navigate(new UsersPage());
        }

        /// <summary>
        /// По клику на кнопку - переход на страницу студентов
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnStudents_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Студенты";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Students.png"));
            InterfaceManagement.ManagementPage.Navigate(new StudentsPage());
        }

        /// <summary>
        /// По клику на кнопку - переход на страницу групп
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnGroups_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Группы";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Group.png"));
            InterfaceManagement.ManagementPage.Navigate(new GroupsPage());
        }

        /// <summary>
        /// По клику на кнопку - переход на страницу дисциплин
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnDisciplines_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Дисциплины";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Disciplines.png"));
            InterfaceManagement.ManagementPage.Navigate(new DisciplinesPage());
        }

        /// <summary>
        /// По клику на кнопку - переход на страницу специальностей
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSpecialties_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Специальности";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Specilization.png"));
            InterfaceManagement.ManagementPage.Navigate(new SpecialtiesPage());
        }

        /// <summary>
        /// По клику на кнопку - переход на страницу списка назначений преподавателей
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAppointment_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Назначение преподавателей";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Naznashenie.png"));
            InterfaceManagement.ManagementPage.Navigate(new AppointmentsPage());
        }
    }
}
