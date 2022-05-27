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
using SubsystemKKEP.Classes;

namespace SubsystemKKEP.AppPages.Administrator
{
    /// <summary>
    /// Логика взаимодействия для SpecialtiesPage.xaml
    /// </summary>
    public partial class SpecialtiesPage : Page
    {
        public SpecialtiesPage()
        {
            InitializeComponent();
        }

        private void UpdateSpecialization()
        {
            var currentSpecialization = App.DataBase.Specializations.ToList();

            if (CmbSortDepartment.SelectedIndex > 0)
            {
                currentSpecialization = currentSpecialization.Where(p => p.Department == (CmbSortDepartment.SelectedItem as Department)).ToList();
            }

            currentSpecialization = currentSpecialization.Where(p => p.SpecializationOfName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            if (currentSpecialization.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }
            DGridSpecializations.ItemsSource = currentSpecialization.OrderBy(p => p.SpecializationOfName);
        }

        private void BtnAddSpecialization_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new SpecialtieAddEditPage(null));
        }

        private void BtnDeleteSpecialization_Click(object sender, RoutedEventArgs e)
        {
            var specializationsRemoving = DGridSpecializations.SelectedItems.Cast<Specialization>().ToList();
            if (specializationsRemoving.Count == 0)
            {
                MessageBox.Show($"Выберите специальности", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"Вы точно хотите удалить {specializationsRemoving.Count} специальностей?",
                "Подтверждение удаления", MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.DataBase.Specializations.RemoveRange(specializationsRemoving);
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateSpecialization();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSpecialization();
        }

        private void CmbSortDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSpecialization();
        }

        private void BtnEditSpecialization_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new SpecialtieAddEditPage((sender as Button).DataContext as Specialization));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var allDepartments = App.DataBase.Departments.ToList();
            allDepartments.Insert(0, new Department
            {
                DepartmentName = "Все отделения"
            });
            CmbSortDepartment.ItemsSource = allDepartments;
            CmbSortDepartment.SelectedIndex = 0;
            UpdateSpecialization();
        }
    }
}
