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
    /// Логика взаимодействия для CurrentJournalPage.xaml
    /// </summary>
    public partial class CurrentJournalPage : Page
    {
        /// <summary>
        /// Текущая группа
        /// </summary>
        private Group currentGroup = new Group();

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="group">конкретная группа</param>
        public CurrentJournalPage(Group group)
        {
            InitializeComponent();
            if (group != null)
            {
                currentGroup = group;
            }
            
        }

        /// <summary>
        /// Обновление списка оценок
        /// </summary>
        private void UpdateMarks()
        {
            if (CmbDiscipline.SelectedIndex < 1)
            {
                return;
            }
            var currentAppointment = CmbDiscipline.SelectedItem as Appointment;
            TextTeacher.Text = $"Преподаватель: {currentAppointment.User.UserName}";
            
            var marks = App.DataBase.Marks.
                Where(p => p.IdDiscipline == currentAppointment.IdDiscipline && p.IdUser == currentAppointment.IdUser 
                && p.Student.IdGroup == currentAppointment.IdGroup).ToList();
            
            if (CmbSortStudent.SelectedIndex > 0)
            {
                marks = marks.Where(p => p.Student.Id == (CmbSortStudent.SelectedItem as Student).Id).ToList();
            }

            if (DpDateMarkSorting.SelectedDate != null)
            {
                marks = marks.Where(p => p.Date == DpDateMarkSorting.SelectedDate).ToList();
            }
            DGridMarks.ItemsSource = marks;
            if (marks.Count == 0)
            {
                MessageBox.Show("Отсутствуют оценки", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// При изменении выбранной дисциплины - отображение оценок по этой дисциплины
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbDiscipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarks();
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
        /// При изменении выбранного студента - сортировка по этому студенту
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarks();
        }

        /// <summary>
        /// При изменении выбранной даты - сортировка по этой дате
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void DpDateMarkSorting_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarks();
        }

        /// <summary>
        /// При изменении видимости страницы - обновление данных
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible && InterfaceManagement.ManagementUser != null)
            {
                var appointments = App.DataBase.Appointments.Where(p => p.IdGroup == currentGroup.Id).ToList();
                var currentDisciplines = new List<Appointment>();
                foreach (var disOfDep in App.DataBase.DisciplineOfDepartments.ToList())
                {
                    foreach (var appointment in appointments)
                    {
                        if (appointment.IdDiscipline == disOfDep.IdDiscipline 
                            && disOfDep.CourseOfStudy == currentGroup.CourseOfStudy 
                            && disOfDep.Department.User.Id == InterfaceManagement.ManagementUser.Id)
                        {
                            currentDisciplines.Add(appointment);
                        }
                    }
                }

                currentDisciplines.Insert(0, new Appointment
                {
                    Discipline = new Discipline
                    {
                        DisciplineName = "Выберите дисциплину"
                    }
                });
                CmbDiscipline.ItemsSource = currentDisciplines;
                CmbDiscipline.SelectedIndex = 0;

                var studentsSort = App.DataBase.Students.Where(p => p.Group.Id == currentGroup.Id).ToList();
                studentsSort = studentsSort.OrderBy(p => p.FullName).ToList();
                studentsSort.Insert(0, new Student
                {
                    LastName = "Все студенты",
                    FirstName = " "
                });
                CmbSortStudent.ItemsSource = studentsSort;
                CmbSortStudent.SelectedIndex = 0;
            }
        }
    }
}
