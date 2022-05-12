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
using SubsystemKKEP.AppPages;
using SubsystemKKEP.AppPages.Teacher;
using SubsystemKKEP.Classes;

namespace SubsystemKKEP.AppWindows
{
    /// <summary>
    /// Логика взаимодействия для TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        /// <summary>
        /// Загрузка окна
        /// </summary>
        public TeacherWindow()
        {
            InitializeComponent();
            InterfaceManagement.ManagementPage = TeacherFrame;
            InterfaceManagement.ManagementPage.Navigate(new AppPages.Account());
            InterfaceManagement.ManagementHeaderText = TextBlockHeader;
            InterfaceManagement.ManagementImgHeader = ImgHeader;
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
        /// По клику на кнопку - открытие страницы аккаунта (личный кабинет)
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Личный кабинет";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/User.png"));
            InterfaceManagement.ManagementPage.Navigate(new Account());
        }

        /// <summary>
        /// По клику на кнопку - открытие страницы с учебными журналами
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnTeachingJournal_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Учебные журналы";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Book.png"));
            InterfaceManagement.ManagementPage.Navigate(new TeachingJournals());
        }
    }
}
