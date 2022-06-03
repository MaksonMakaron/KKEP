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

namespace SubsystemKKEP.AppPages.Department
{
    /// <summary>
    /// Логика взаимодействия для GroupsJournalsPage.xaml
    /// </summary>
    public partial class GroupsJournalsPage : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public GroupsJournalsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обновление групп
        /// </summary>
        private void UpdateGroups()
        {
            if (InterfaceManagement.ManagementUser == null)
            {
                return;
            }
            var currentGroups = App.DataBase.Groups.Where(p => p.Specialization.Department.IdUser == InterfaceManagement.ManagementUser.Id).ToList();

            if (CmbSortCourse.SelectedIndex > 0)
            {
                currentGroups = currentGroups.Where(p => p.CourseOfStudy == (CmbSortCourse.SelectedItem as string)).ToList();
            }
            if (CmbSortSpecialization.SelectedIndex > 0)
            {
                currentGroups = currentGroups.Where(p => p.IdSpecialization == (CmbSortSpecialization.SelectedItem as Specialization).Id).ToList();
            }
            currentGroups = currentGroups.Where(p => p.GroupNumber.ToString().Contains(TbSearch.Text)).ToList();
            if (currentGroups.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }
            DGridGroups.ItemsSource = currentGroups.OrderBy(p => p.GroupNumber);
        }

        /// <summary>
        /// При измении текста - поиск по номеру группы
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// При изменении курса - сортировка по курсу
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// При нажатии на кнопку - перерход на страницу просмотра оценок в журнале
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnOpenJournals_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new CurrentJournalPage((sender as Button).DataContext as Group));
        }

        /// <summary>
        /// При нажатии на кнопку - перерход на страницу с анализом успеваемости
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnReportCard_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// При изменении специальности - сортировка по специальности
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            if (InterfaceManagement.ManagementUser != null)
            {
                UpdateGroups();
                var course = new List<string>
                {
                "Все курсы", "1", "2", "3", "4"
                };
                CmbSortCourse.ItemsSource = course;
                CmbSortCourse.SelectedIndex = 0;
                var specialization = App.DataBase.Specializations.Where(p => p.Department.IdUser == InterfaceManagement.ManagementUser.Id).ToList();
                specialization.Insert(0, new Specialization
                {
                    ShortName = "Все специальности"
                });
                CmbSortSpecialization.ItemsSource = specialization;
                CmbSortSpecialization.SelectedIndex = 0;
            }
        }
    }
}
