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

namespace SubsystemKKEP.AppPages
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public Account()
        {
            InitializeComponent();
            TbName.Text = InterfaceManagement.ManagementUser.UserName;
            TbRole.Text = InterfaceManagement.ManagementUser.Role.RoleName;
            TbLogIn.Text = InterfaceManagement.LogInUser.DateLogIn.ToString("G");
        }

        /// <summary>
        /// По клику на кнопку - возврат на предыдущую страницу
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new PasswordChange());
        }
    }
}
