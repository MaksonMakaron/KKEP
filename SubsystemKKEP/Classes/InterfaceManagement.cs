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
    /// Класс для управления интерфейсом
    /// </summary>
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
        /// Выход из аккаунта. Очистка свойств
        /// </summary>
        /// <returns>true - пользователь вышел, false - пользователь не вышел</returns>
        public static void SignOut()
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            ManagementUser = null;
            ManagementWindow = null;
            LogInUser = null;
        }
    }
}
