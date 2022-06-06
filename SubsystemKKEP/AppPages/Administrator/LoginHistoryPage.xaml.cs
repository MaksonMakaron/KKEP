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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SubsystemKKEP.AppPages.Administrator
{
    /// <summary>
    /// Логика взаимодействия для LoginHistoryPage.xaml
    /// </summary>
    public partial class LoginHistoryPage : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public LoginHistoryPage()
        {
            InitializeComponent();
            var roles = App.DataBase.Roles.ToList();
            roles.Insert(0, new Role
            {
                RoleName = "Все роли"
            });
            CmbSortRole.ItemsSource = roles;
            CmbSortRole.SelectedIndex = 0;
        }

        /// <summary>
        /// Обновление истории входа
        /// </summary>
        private void UpdateHistory()
        {
            var history = App.DataBase.LogIns.ToList();

            if (CmbSortRole.SelectedIndex > 0)
            {
                history = history.Where(p => p.User.IdRole == (CmbSortRole.SelectedItem as Role).Id).ToList();
            }
            if (DpDateHistorySorting.SelectedDate != null)
            {
                history = history.Where(p => p.DateLogIn.Date == DpDateHistorySorting.SelectedDate).ToList();
            }

            history = history.Where(p => p.User.UserName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();
            if (history.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }
            DGridLoginsHistory.ItemsSource = history.OrderByDescending(p => p.DateLogIn);
        }

        /// <summary>
        /// При изменении выбора роли - сортировка по этой роли
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateHistory();
        }

        /// <summary>
        /// При изменении выбранной даты - сортировка по этой дате
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void DpDateHistorySorting_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateHistory();
        }

        /// <summary>
        /// Поиск по пользователю (ФИО) при изменении текст в TextBox
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHistory();
        }

        /// <summary>
        /// При нажатии на кнопку - переход на предыдущую страницу
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }
    }
}
