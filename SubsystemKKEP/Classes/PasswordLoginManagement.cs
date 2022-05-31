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
        /// <param name="EnteredPassword">введеный пароль</param>
        /// <returns></returns>
        public static bool PasswordCheck(string EnteredPassword)
        {
            var users = App.DataBase.Users.ToList();
            EnteredPassword = PasswordLoginManagement.CreateSHA512(EnteredPassword).ToLower();
            for (int i = 0; i < users.Count; i++)
            {
                if (EnteredPassword == users[i].Password)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Смена пароля
        /// </summary>
        /// <param name="NewPassword">новый пароль</param>
        public static void ChangePassword(string NewPassword)
        {
            User user = App.DataBase.Users.Where(p => p.Id == InterfaceManagement.ManagementUser.Id).FirstOrDefault();
            user.Password = PasswordLoginManagement.CreateSHA512(NewPassword);
            App.DataBase.SaveChanges();
        }
    }
}
