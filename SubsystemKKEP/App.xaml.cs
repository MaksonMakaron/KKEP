using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SubsystemKKEP.Classes;

namespace SubsystemKKEP
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Получение контекста БД
        /// </summary>
        public static readonly DB_KKEPEntities DataBase = new DB_KKEPEntities();
    }
}
