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
        private Discipline currentDiscipline = new Discipline();
        private List<DisciplineOfDepartment> currentDisOfDep = new List<DisciplineOfDepartment>();

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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }

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
                IdDepartment = (CmbDepartment.SelectedItem as Department).Id,
                CourseOfStudy = CmbCourse.SelectedItem as string,
                Department = CmbDepartment.SelectedItem as Department,
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var removing = (sender as Button).DataContext as DisciplineOfDepartment;
            currentDisOfDep.Remove(removing);
            DGridDisciplinesOfDepartment.Items.Refresh();
        }
    }
}
