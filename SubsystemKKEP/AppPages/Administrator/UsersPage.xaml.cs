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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            var allRoles = App.DataBase.Roles.ToList();
            allRoles.Insert(0, new Role
            {
                RoleName = "Все пользователи"
            });
            CmbSortRole.ItemsSource = allRoles;
            CmbSortRole.SelectedIndex = 0;
            UpdateUsers();
        }


        private void UpdateUsers()
        {
            var currentUsers = App.DataBase.Users.ToList();
            if (CmbSortRole.SelectedIndex > 0)
            {
                currentUsers = currentUsers.Where(p => p.Role.RoleName == (CmbSortRole.SelectedItem as Role).RoleName).ToList();
            }

            currentUsers = currentUsers.Where(p => p.UserName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            if (currentUsers.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }
            DGridUsers.ItemsSource = currentUsers.OrderBy(p => p.UserName);
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу добавления пользователя
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new UserAddEdit(null));
        }

        /// <summary>
        /// При нажатии на кнопку - удаление пользователей
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var usersRemoving = DGridUsers.SelectedItems.Cast<User>().ToList();
            if (usersRemoving.Count == 0)
            {
                MessageBox.Show($"Выберите пользователей", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"Вы точно хотите удалить {usersRemoving.Count} пользователей?",
                "Подтверждение удаления", MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.DataBase.Users.RemoveRange(usersRemoving);
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу редактирования пользователя
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new UserAddEdit((sender as Button).DataContext as User));
        }

        /// <summary>
        /// При изменении текст - поиск
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        /// <summary>
        /// При выборе элемента из списка - фильтрация
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        /// <summary>
        /// При показе - обновление списка
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateUsers();
        }
    }
}
