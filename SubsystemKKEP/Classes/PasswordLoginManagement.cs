using SubsystemKKEP.AppWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SubsystemKKEP.Classes
{
    /// <summary>
    /// Класс для управления логинами и паролями
    /// </summary>
    public class PasswordLoginManagement
    {
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        /// <returns>true - авторизация прошла успешно, false - авторзиация не удалась</returns>
        public static bool AuthorizationUser(string login, string password)
        {
            var users = App.DataBase.Users.ToList();
            password = CreateSHA512(password).ToLower();
            
            for (int i = 0; i < users.Count; i++)
            {
                if (login == users[i].Login && password == users[i].Password)
                {
                    switch (users[i].Role.RoleName)
                    {
                        case "Администратор":
                            OpenAdministratorWindow(users[i]);
                            break;
                        case "Преподаватель":
                            OpenTeacherWindow(users[i]);
                            break;
                        case "Отделение":
                            OpenDepartmentWindow(users[i]);
                            break;
                        default:
                            MessageBox.Show("Ошибка авторизации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Открытие окна Администратора
        /// </summary>
        /// <param name="сoncreteUser">пользователь, который авторизовался</param>
        private static void OpenAdministratorWindow(User сoncreteUser)
        {
            InterfaceManagement.ManagementUser = сoncreteUser;
            RecordLogIn(сoncreteUser);
            AdministratorWindow administratorWindow = new AdministratorWindow();
            administratorWindow.Show();
            InterfaceManagement.ManagementWindow = administratorWindow;
            InterfaceManagement.ManagementWindow.Title = $"Администратор. {InterfaceManagement.ManagementUser.UserName}";
        }

        /// <summary>
        /// Открытие окна Преподавателя
        /// </summary>
        /// <param name="сoncreteUser">пользователь, который авторизовался</param>
        private static void OpenTeacherWindow(User сoncreteUser)
        {
            InterfaceManagement.ManagementUser = сoncreteUser;
            RecordLogIn(сoncreteUser);
            TeacherWindow teacherWindow = new TeacherWindow();
            teacherWindow.Show();
            InterfaceManagement.ManagementWindow = teacherWindow;
            InterfaceManagement.ManagementWindow.Title = $"Преподаватель. {InterfaceManagement.ManagementUser.UserName}";
        }

        /// <summary>
        /// Открытие окна Отделения
        /// </summary>
        /// <param name="сoncreteUser">пользователь, который авторизовался</param>
        private static void OpenDepartmentWindow(User сoncreteUser)
        {
            InterfaceManagement.ManagementUser = сoncreteUser;
            RecordLogIn(сoncreteUser);
            DepartmentWindow departmentWindow = new DepartmentWindow();
            departmentWindow.Show();
            InterfaceManagement.ManagementWindow = departmentWindow;
            InterfaceManagement.ManagementWindow.Title = $"Отделение. {InterfaceManagement.ManagementUser.UserName}";
            RecordLogIn(сoncreteUser);
        }

        /// <summary>
        /// Запись входа в систему
        /// </summary>
        /// <param name="concreteUser">пользователь, который авторизовался</param>
        private static void RecordLogIn(User concreteUser)
        {
            LogIn logIn = new LogIn
            {
                User = concreteUser,
                DateLogIn = DateTime.Now
            };
            try
            {
                App.DataBase.LogIns.Add(logIn);
                App.DataBase.SaveChanges();
                InterfaceManagement.LogInUser = logIn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// При нажатии кнопки мыши - пароль показывается 
        /// </summary>
        /// <param name="tbPassword">элемент textbox</param>
        /// <param name="pbPassword">элемент passwordbox</param>
        public static void SeePasswordPreviewMouseDown(TextBox tbPassword, PasswordBox pbPassword)
        {
            tbPassword.Text = pbPassword.Password;
            pbPassword.Visibility = Visibility.Hidden;
            tbPassword.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// При нажатии кнопки мыши - пароль скрывается 
        /// </summary>
        /// <param name="tbPassword">элемент textbox</param>
        /// <param name="pbPassword">элемент passwordbox</param>
        public static void SeePasswordPreviewMouseUp(TextBox tbPassword, PasswordBox pbPassword)
        {
            pbPassword.Password = tbPassword.Text;
            tbPassword.Visibility = Visibility.Hidden;
            pbPassword.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Создание зашифрованного пароля
        /// </summary>
        /// <param name="input">входная строка для шифрования</param>
        /// <returns></returns>
        public static string CreateSHA512(string input)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = shaM.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        /// <summary>
        /// Поиск повторяющегося логина
        /// </summary>
        /// <param name="login">логин для поиска</param>
        /// <returns>true - логин не найден, false - логин найден</returns>
        public static bool SearchLoginDontRepeat(string login)
        {
            var users = App.DataBase.Users.ToList();

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login == login)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка совпадения введенного пароля со старым
        /// </summary>
        /// <param name="enteredPassword">введеный пароль</param>
        /// <returns></returns>
        public static bool PasswordCheck(string enteredPassword)
        {
            var users = App.DataBase.Users.ToList();
            enteredPassword = CreateSHA512(enteredPassword).ToLower();
            for (int i = 0; i < users.Count; i++)
            {
                if (enteredPassword == users[i].Password)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Смена пароля
        /// </summary>
        /// <param name="newPassword">новый пароль</param>
        public static void ChangePassword(string newPassword)
        {
            User user = App.DataBase.Users.Where(p => p.Id == InterfaceManagement.ManagementUser.Id).FirstOrDefault();
            user.Password = PasswordLoginManagement.CreateSHA512(newPassword);
            App.DataBase.SaveChanges();
        }
    }
}
