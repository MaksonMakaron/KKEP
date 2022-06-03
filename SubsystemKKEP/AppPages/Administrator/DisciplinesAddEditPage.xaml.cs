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
    /// Логика взаимодействия для DisciplinesAddEditPage.xaml
    /// </summary>
    public partial class DisciplinesAddEditPage : Page
    {
        /// <summary>
        /// Текущая дисциплина
        /// </summary>
        private Discipline currentDiscipline = new Discipline();

        /// <summary>
        /// Текущая дисциплина на каких отделениях
        /// </summary>
        private List<DisciplineOfDepartment> currentDisOfDep = new List<DisciplineOfDepartment>();

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="discipline">выбранная дисциплина</param>
        public DisciplinesAddEditPage(Discipline discipline)
        {
            InitializeComponent();
            if (discipline != null)
            {
                currentDiscipline = discipline;
                currentDisOfDep = discipline.DisciplineOfDepartments.ToList();
            }
            CmbDepartment.ItemsSource = App.DataBase.Departments.ToList();
            CmbCourse.ItemsSource = new List<string> 
            {
                "1", "2", "3", "4" 
            };
            DataContext = currentDiscipline;
            DGridDisciplinesOfDepartment.ItemsSource = currentDisOfDep;
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

        /// <summary>
        /// При нажатии на кнопку - сохранение информации
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TbDisciplineName.Text))
            {
                errors.AppendLine("Введите название дисциплины");
            }
            if (currentDisOfDep.Count == 0)
            {
                errors.AppendLine("Заполните информацию о дисциплине");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            currentDiscipline.DisciplineOfDepartments = currentDisOfDep;

            foreach (var discipline in App.DataBase.Disciplines.ToList())
            {
                if (discipline.DisciplineName == currentDiscipline.DisciplineName)
                {
                    MessageBox.Show("Такая дисциплина уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (currentDiscipline.Id == 0)
            {
                App.DataBase.Disciplines.Add(currentDiscipline);
            }

            try
            {
                App.DataBase.SaveChanges();
                MessageBox.Show("Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                InterfaceManagement.ManagementPage.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// При нажатии на кнопку - добавление информации о дисциплине
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (CmbDepartment.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите отделение");
            }
            if (CmbCourse.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите курс");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var currentDepOfCourse = new DisciplineOfDepartment
            {
                IdDepartment = (CmbDepartment.SelectedItem as Classes.Department).Id,
                CourseOfStudy = CmbCourse.SelectedItem as string,
                Department = CmbDepartment.SelectedItem as Classes.Department,
                Discipline = currentDiscipline,
                IdDiscipline = currentDiscipline.Id
            };
            foreach (var item in currentDisOfDep)
            {
                if (item.IdDepartment == currentDepOfCourse.IdDepartment && item.CourseOfStudy == currentDepOfCourse.CourseOfStudy)
                {
                    MessageBox.Show("Информация уже содержится", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            currentDisOfDep.Add(currentDepOfCourse);

            DGridDisciplinesOfDepartment.Items.Refresh();
        }

        /// <summary>
        /// При нажатии на кнопку - удаление информации о дисциплине
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var removing = (sender as Button).DataContext as DisciplineOfDepartment;
            currentDisOfDep.Remove(removing);
            DGridDisciplinesOfDepartment.Items.Refresh();
        }
    }
}
