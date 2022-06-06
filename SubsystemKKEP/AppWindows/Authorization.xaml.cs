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
using SubsystemKKEP.Classes;

namespace SubsystemKKEP.AppWindows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        /// <summary>
        /// Загрузка окна
        /// </summary>
        public Authorization()
        {
            InitializeComponent();
            AuthorizationWindow = authorizationWindow;
        }

        /// <summary>
        /// Свойство для управления окном Авторизации
        /// </summary>
        public static Window AuthorizationWindow { get; set; }

        /// <summary>
        /// При нажатии кнопки мыши - пароль показывается 
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSeePassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordLoginManagement.SeePasswordPreviewMouseDown(TbPassword, PbPassword);
        }

        /// <summary>
        /// При отпуске кнопки мыши - пароль скрывается
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSeePassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PasswordLoginManagement.SeePasswordPreviewMouseUp(TbPassword, PbPassword);
        }

        /// <summary>
        /// По клику на кнопку - вход в ЛК
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            var login = TbLogin.Text;
            var password = PbPassword.Password;
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            {
                if (!PasswordLoginManagement.AuthorizationUser(login, password))
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                var errors = "";
                if (string.IsNullOrWhiteSpace(login))
                {
                    errors += "Введите логин\n";
                }
                if (string.IsNullOrWhiteSpace(password))
                {
                    errors += "Введите пароль";
                }
                MessageBox.Show($"{errors}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
