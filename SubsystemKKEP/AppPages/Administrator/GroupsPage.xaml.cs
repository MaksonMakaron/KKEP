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
        public GroupsPage()
        {
            InitializeComponent();
        }

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
                currentGroups = currentGroups.Where(p => p.Specialization.Department == CmbSortDepartment.SelectedItem as Department).ToList();
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

            DGridGroup.ItemsSource = currentGroups.OrderBy(p => p.GroupName);
        }


        private void BtnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new GroupsAddEditPage(null));
        }

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

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGroups();
        }

        private void CmbSortSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        private void CmbSortCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

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
            department.Insert(0, new Department
            {
                DepartmentName = "Все отделения"
            });
            CmbSortDepartment.ItemsSource = department;
            CmbSortDepartment.SelectedIndex = 0;
            UpdateGroups();
        }

        private void BtnEditGroup_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new GroupsAddEditPage((sender as Button).DataContext as Group));
        }

        private void CmbSortDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }
    }
}
