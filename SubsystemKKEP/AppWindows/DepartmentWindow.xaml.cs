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
using System.Windows.Shapes;

namespace SubsystemKKEP.AppWindows
{
    /// <summary>
    /// Логика взаимодействия для DepartmentWindow.xaml
    /// </summary>
    public partial class DepartmentWindow : Window
    {
        /// <summary>
        /// Загрузка окна
        /// </summary>
        public DepartmentWindow()
        {
            InitializeComponent();
            InterfaceManagement.ManagementPage = DepartmentFrame;
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
        /// По клику происходит переход на страницу с успеваемостью
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAcademicPerformance_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Успеваемость";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Analis.png"));
        }

        /// <summary>
        /// По клику происходит переход на страницу аккаунта
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementHeaderText.Text = "Личный кабинет";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Book.png"));
        }
    }
}
