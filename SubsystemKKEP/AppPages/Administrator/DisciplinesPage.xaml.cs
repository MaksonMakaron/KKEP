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
    /// Логика взаимодействия для DisciplinesPage.xaml
    /// </summary>
    public partial class DisciplinesPage : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public DisciplinesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обновление списка дисциплин
        /// </summary>
        private void UpdateDiscipline()
        {
            var currentDiscipline = App.DataBase.Disciplines.ToList();
            currentDiscipline = currentDiscipline.Where(p => p.DisciplineName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();
            if (currentDiscipline.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }
            DGridDisciplines.ItemsSource = currentDiscipline.OrderBy(p => p.DisciplineName);
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу с добавлением дисциплины
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAddDiscipline_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new DisciplinesAddEditPage(null));
        }

        /// <summary>
        /// При нажатии на кнопку - удаление дисциплин
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnDeleteDiscipline_Click(object sender, RoutedEventArgs e)
        {
            var disciplinesRemoving = DGridDisciplines.SelectedItems.Cast<Discipline>().ToList();
            if (disciplinesRemoving.Count == 0)
            {
                MessageBox.Show($"Выберите дисциплины", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"Вы точно хотите удалить {disciplinesRemoving.Count} дисциплин?",
                "Подтверждение удаления", MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.DataBase.Disciplines.RemoveRange(disciplinesRemoving);
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateDiscipline();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// При изменении содержимого TextBox - обновление списка дисциплин
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDiscipline();
        }

        /// <summary>
        /// При изменении выбранного отделения - обновление списка дисциплин
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDiscipline();
        }

        /// <summary>
        /// При изменении выбранного курса - обновление списка дисциплин
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDiscipline();
        }

        /// <summary>
        /// При нажатии на кнопку - переход на страницу с редактированием дисциплины
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnEditDiscipline_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new DisciplinesAddEditPage((sender as Button).DataContext as Discipline));
        }

        /// <summary>
        /// При загрузке страницы - обновление данных
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateDiscipline();
        }
    }
}
