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
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public SpecialtiesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обновление списка специальностей
        /// </summary>
        private void UpdateSpecialization()
        {
            var currentSpecialization = App.DataBase.Specializations.ToList();

            if (CmbSortDepartment.SelectedIndex > 0)
            {
                currentSpecialization = currentSpecialization.Where(p => p.Department == (CmbSortDepartment.SelectedItem as Classes.Department)).ToList();
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

        /// <summary>
        /// По нажатию на кнопку - переход на страницу с добавлением специальности
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAddSpecialization_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new SpecialtieAddEditPage(null));
        }

        /// <summary>
        /// По нажатию на кнопку - удаление специальностей
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
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

        /// <summary>
        /// При изменении содержимого TextBox - обновление списка специальностей
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSpecialization();
        }

        /// <summary>
        /// При изменении выбранного пункта - обновление списка специальностей
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSpecialization();
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу с редактированием специальности
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnEditSpecialization_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new SpecialtieAddEditPage((sender as Button).DataContext as Specialization));
        }

        /// <summary>
        /// При загрузке страницы - обновление данных
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var allDepartments = App.DataBase.Departments.ToList();
            allDepartments.Insert(0, new Classes.Department
            {
                DepartmentName = "Все отделения"
            });
            CmbSortDepartment.ItemsSource = allDepartments;
            CmbSortDepartment.SelectedIndex = 0;
            UpdateSpecialization();
        }
    }
}
