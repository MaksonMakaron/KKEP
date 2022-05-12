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
    public class InterfaceManagement
    {
        /// <summary>
        /// Свойство для управления окном пользователя
        /// </summary>
        public static Window ManagementWindow { get; set; }

        /// <summary>
        /// Свойство для управления пользователем
        /// </summary>
        public static User ManagementUser { get; set; }

        /// <summary>
        /// Свойство для управления фреймом (страницами)
        /// </summary>
        public static Frame ManagementPage { get; set; }

        /// <summary>
        /// Свойство для управления заголовком в окне
        /// </summary>
        public static TextBlock ManagementHeaderText { get; set; }

        /// <summary>
        /// Свойство для управления изображением в окне
        /// </summary>
        public static Image ManagementImgHeader { get; set; }

        /// <summary>
        /// Свойство для управления входом
        /// </summary>
        public static LogIn LogInUser { get; set; }

        /// <summary>
        /// При нажатии кнопки мыши - пароль показывается 
        /// </summary>
        /// <param name="TbPassword">элемент textbox</param>
        /// <param name="PbPassword">элемент passwordbox</param>
        public static void SeePasswordPreviewMouseDown(TextBox TbPassword, PasswordBox PbPassword)
        {
            TbPassword.Text = PbPassword.Password;
            PbPassword.Visibility = Visibility.Hidden;
            TbPassword.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// При нажатии кнопки мыши - пароль скрывается 
        /// </summary>
        /// <param name="TbPassword">элемент textbox</param>
        /// <param name="PbPassword">элемент passwordbox</param>
        public static void SeePasswordPreviewMouseUp(TextBox TbPassword, PasswordBox PbPassword)
        {
            PbPassword.Password = TbPassword.Text;
            TbPassword.Visibility = Visibility.Hidden;
            PbPassword.Visibility = Visibility.Visible;
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
        /// Выход из аккаунта. Очистка свойств
        /// </summary>
        /// <returns>true - пользователь вышел, false - пользователь не вышел</returns>
        public static bool SignOut()
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Authorization authorization = new Authorization();
                authorization.Show();
                ManagementUser = null;
                ManagementWindow = null;
                LogInUser = null;
                return true;
            }
            return false;
        }
    }
}
