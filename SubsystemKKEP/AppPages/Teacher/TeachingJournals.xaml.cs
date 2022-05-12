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

namespace SubsystemKKEP.AppPages.Teacher
{
    /// <summary>
    /// Логика взаимодействия для TeachingJournals.xaml
    /// </summary>
    public partial class TeachingJournals : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public TeachingJournals()
        {
            InitializeComponent();
            DGridDisciplines.ItemsSource = App.DataBase.TeacherDisciplineGroups.
                Where(p => p.Teacher.IdUser == InterfaceManagement.ManagementUser.Id).ToList();
        }

        /// <summary>
        /// Обновление списка дисциплин
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var disciplines = App.DataBase.TeacherDisciplineGroups.
                Where(p => p.Teacher.IdUser == InterfaceManagement.ManagementUser.Id).ToList();

            disciplines = disciplines.Where(p => p.Group.NameGroup.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            DGridDisciplines.ItemsSource = disciplines;
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            var selectedJournal = (sender as Button).DataContext as TeacherDisciplineGroup;
            InterfaceManagement.ManagementHeaderText.Text = selectedJournal.Group.NameGroup;
            InterfaceManagement.ManagementPage.Navigate(new GroupJournal(selectedJournal));
        }
    }
}
