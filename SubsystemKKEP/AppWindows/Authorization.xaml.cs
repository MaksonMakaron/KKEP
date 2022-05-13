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
            InterfaceManagement.SeePasswordPreviewMouseDown(TbPassword, PbPassword);
        }

        /// <summary>
        /// При отпуске кнопки мыши - пароль скрывается
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSeePassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            InterfaceManagement.SeePasswordPreviewMouseUp(TbPassword, PbPassword);
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
                AuthorizationUser(login, password);
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

       

        /// <summary>
        /// Открытие окна Администратора
        /// </summary>
        /// <param name="ConcreteUser">пользователь, который авторизовался</param>
        private static void OpenAdministratorWindow(User ConcreteUser)
        {
            InterfaceManagement.ManagementUser = ConcreteUser;
            RecordLogIn(ConcreteUser);
            AdministratorWindow administratorWindow = new AdministratorWindow();
            administratorWindow.Show();
            InterfaceManagement.ManagementWindow = administratorWindow;
            InterfaceManagement.ManagementWindow.Title = $"Администратор. {InterfaceManagement.ManagementUser.UserName}";
        }

        /// <summary>
        /// Открытие окна Преподавателя
        /// </summary>
        /// <param name="ConcreteUser">пользователь, который авторизовался</param>
        private static void OpenTeacherWindow(User ConcreteUser)
        {
            InterfaceManagement.ManagementUser = ConcreteUser;
            RecordLogIn(ConcreteUser);
            TeacherWindow teacherWindow = new TeacherWindow();
            teacherWindow.Show();
            InterfaceManagement.ManagementWindow = teacherWindow;
            InterfaceManagement.ManagementWindow.Title = $"Преподаватель. {InterfaceManagement.ManagementUser.UserName}";
        }

        /// <summary>
        /// Открытие окна Отделения
        /// </summary>
        /// <param name="ConcreteUser">пользователь, который авторизовался</param>
        private static void OpenDepartmentWindow(User ConcreteUser)
        {
            InterfaceManagement.ManagementUser = ConcreteUser;
            RecordLogIn(ConcreteUser);
            DepartmentWindow departmentWindow = new DepartmentWindow();
            departmentWindow.Show();
            InterfaceManagement.ManagementWindow = departmentWindow;
            InterfaceManagement.ManagementWindow.Title = $"Отделение. {InterfaceManagement.ManagementUser.UserName}";
            RecordLogIn(ConcreteUser);
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="login">логин пользователя</param>
        /// <param name="password">пароль пользователя</param>
        private static void AuthorizationUser(string login, string password)
        {
            var users = App.DataBase.Users.ToList();
            var authorization = false;
            password = InterfaceManagement.CreateSHA512(password).ToLower();
            for (int i = 0; i < users.Count; i++)
            {
                if (login == users[i].Login && password == users[i].Password)
                {
                    authorization = true;
                    switch (users[i].Role.RoleName)
                    {
                        case "Администратор":
                            OpenAdministratorWindow(users[i]);
                            AuthorizationWindow.Close();
                            break;
                        case "Преподаватель":
                            OpenTeacherWindow(users[i]);
                            AuthorizationWindow.Close();
                            break;
                        case "Отделение":
                            OpenDepartmentWindow(users[i]);
                            AuthorizationWindow.Close();
                            break;
                        default:
                            MessageBox.Show("Ошибка авторизации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
            }
            if (!authorization)
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Запись входа в систему
        /// </summary>
        /// <param name="ConcreteUser">пользователь, который авторизовался</param>
        private static void RecordLogIn(User ConcreteUser)
        {
            LogIn logIn = new LogIn
            {
                User = ConcreteUser,
                DateLogIn = DateTime.Now
            };
            App.DataBase.LogIns.Add(logIn);
            App.DataBase.SaveChanges();
            InterfaceManagement.LogInUser = logIn;
        }
    }
}
