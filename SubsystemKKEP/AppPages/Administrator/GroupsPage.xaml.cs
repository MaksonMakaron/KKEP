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
    /// Логика взаимодействия для GroupsPage.xaml
    /// </summary>
    public partial class GroupsPage : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public GroupsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обновление списка групп
        /// </summary>
        private void UpdateGroups()
        {
            var currentGroups = App.DataBase.Groups.ToList();

            if (CmbSortCourse.SelectedIndex > 0)
            {
                currentGroups = currentGroups.Where(p => p.CourseOfStudy == CmbSortCourse.SelectedItem.ToString()).ToList();
            }

            if (CmbSortSpecialization.SelectedIndex > 0)
            {
                currentGroups = currentGroups.Where(p => p.Specialization == CmbSortSpecialization.SelectedItem).ToList();
            }

            if (CmbSortDepartment.SelectedIndex > 0)
            {
                currentGroups = currentGroups.Where(p => p.Specialization.Department == CmbSortDepartment.SelectedItem as Classes.Department).ToList();
            }

            if (CbSortIsArchive.IsChecked.Value)
            {
                currentGroups = currentGroups.Where(p => p.IsArchive).ToList();

            }

            currentGroups = currentGroups.Where(p => p.GroupName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            if (currentGroups.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }

            DGridGroup.ItemsSource = currentGroups.OrderBy(p => p.GroupNumber);
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу с добавлением группы
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new GroupsAddEditPage(null));
        }

        /// <summary>
        /// При нажатии на кнопку - удаление групп
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            var groupsRemoving = DGridGroup.SelectedItems.Cast<Group>().ToList();
            if (groupsRemoving.Count == 0)
            {
                MessageBox.Show($"Выберите группы", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"Вы точно хотите удалить {groupsRemoving.Count} групп?",
                "Подтверждение удаления", MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.DataBase.Groups.RemoveRange(groupsRemoving);
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateGroups();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// При изменении содержимого TextBox - обновление списка специальностей
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// При изменении выбранной специальности - обновление списка групп
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// При изменении выбранного курса - обновление списка групп
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// При загрузке страницы - обновление данных
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var course = new List<string>
            {
                "Все курсы", "1", "2", "3", "4"
            };
            CmbSortCourse.ItemsSource = course;
            CmbSortCourse.SelectedIndex = 0;
            var specialization = App.DataBase.Specializations.ToList();
            specialization.Insert(0, new Specialization 
            {
                ShortName = "Все специальности"
            });
            CmbSortSpecialization.ItemsSource = specialization;
            CmbSortSpecialization.SelectedIndex = 0;
            var department = App.DataBase.Departments.ToList();
            department.Insert(0, new Classes.Department
            {
                DepartmentName = "Все отделения"
            });
            CmbSortDepartment.ItemsSource = department;
            CmbSortDepartment.SelectedIndex = 0;
            UpdateGroups();
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу с редактированием группы
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnEditGroup_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new GroupsAddEditPage((sender as Button).DataContext as Group));
        }

        /// <summary>
        /// При изменении выбранного отделения - обновление списка групп
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// При изменении пункта из списка - обновление списка групп
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortIsArchive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// При изменении - обновление списка групп (при checked - только архивные группы)
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CbSortIsArchive_Checked(object sender, RoutedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// При изменении - обновление списка групп (при unchecked - все группы)
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CbSortIsArchive_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateGroups();
        }
    }
}
