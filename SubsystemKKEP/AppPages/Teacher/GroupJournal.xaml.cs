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
    /// Логика взаимодействия для GroupJournal.xaml
    /// </summary>
    public partial class GroupJournal : Page
    {
        private TeacherDisciplineGroup journalCurrent = new TeacherDisciplineGroup();

        public GroupJournal(TeacherDisciplineGroup selectedJournal)
        {
            InitializeComponent();
            TextTeacher.Text = $"Преподаватель: {selectedJournal.Teacher.User.UserName}";
            // DGridMarks.ItemsSource = ;
            journalCurrent = selectedJournal;

            var students = App.DataBase.Students.Where(p => journalCurrent.IdGroup == p.Groups.FirstOrDefault().Id).ToList();

            List<Student> student1 = new List<Student>();
            
            //var studentsMarks = App.DataBase.Marks.Where(p => p.IdDiscipline == journalCurrent.IdDiscipline && journalCurrent.IdGroup == p.TeachingJournal.IdGroup).ToList();
            

            foreach (var student in students)
            {
                foreach (var mark in student.Marks.Where(p => p.IdDiscipline == journalCurrent.IdDiscipline))
                {
                    var currentStudent = new Student()
                    {
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Marks = new List<Mark>()

                    };
                    currentStudent.Marks.Add(mark);
                    student1.Add(currentStudent);
                }
            }
            
            DGridMarks.ItemsSource = student1;
            //DGridMarks.ItemsSource = App.DataBase.Marks.Where(p => p.IdDiscipline == journalCurrent.IdDiscipline && journalCurrent.IdGroup == p.TeachingJournal.IdGroup).ToList();



            //DGridMarks.ItemsSource = App.DataBase.Mark.
            //   Where(p => p.IdTeacher == selectedJournal.IdTeacher &&
            //   p.IdDiscipline == selectedJournal.IdDiscipline).ToList();

        }

        /// <summary>
        /// По нажатию на кнопку - переход на предыдущую страницу
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }

        /// <summary>
        /// При изменении текста происходит поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// По нажатию на кнопку - переход на страницу с добавлением новой оценки
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAddMark_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new MarkEditing(null));
        }
        
        /// <summary>
        /// По нажатию кнопки - удаление оценок
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnDeleteMark_Click(object sender, RoutedEventArgs e)
        {
            var marksRemoving = DGridMarks.SelectedItems.Cast<Mark>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить {marksRemoving.Count} оценок?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, 
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.DataBase.Marks.RemoveRange(marksRemoving);
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    DGridMarks.ItemsSource = App.DataBase.Marks.
                           Where(p => p.IdDiscipline == journalCurrent.IdDiscipline && journalCurrent.IdGroup == p.TeachingJournal.IdGroup).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// По нажатию на кнопку - переход на страницу с редактированием выбранной оценки
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnEditMark_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new MarkEditing((sender as Button).DataContext as Mark));
        }

        /// <summary>
        /// По нажатию на кнопку - переход на страницу с редактированием выбранной оценки
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                //DGridMarks.ItemsSource = App.DataBase.Marks.Where(p => p.IdDiscipline == journalCurrent.IdDiscipline && journalCurrent.IdGroup == p.TeachingJournal.IdGroup).ToList();
            }
        }
    }
}
