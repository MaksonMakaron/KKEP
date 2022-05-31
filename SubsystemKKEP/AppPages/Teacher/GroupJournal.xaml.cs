using SubsystemKKEP.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SubsystemKKEP.AppPages.Teacher
{
    /// <summary>
    /// Логика взаимодействия для GroupJournal.xaml
    /// </summary>
    public partial class GroupJournal : Page
    {
        /// <summary>
        /// Текущий журнал группы
        /// </summary>
        private static Appointment journalCurrent = new Appointment();

        /// <summary>
        /// Текущая оценка
        /// </summary>
        private Mark markCurrent = new Mark();

        /// <summary>
        /// Загрузка журнала
        /// </summary>
        /// <param name="selectedJournal">выбранный журнал и дисциплина</param>
        public GroupJournal(Appointment selectedJournal)
        {
            InitializeComponent();
            journalCurrent = selectedJournal;
        }

        /// <summary>
        /// По нажатию на кнопку - переход на предыдущую страницу
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
            InterfaceManagement.ManagementHeaderText.Text = "Учебные журналы";
            InterfaceManagement.ManagementImgHeader.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Book.png"));
        }

        /// <summary>
        /// Обновление оценок и заполнения DGridMarks
        /// </summary>
        private void UpdateMarks()
        {
            var currentMarks = App.DataBase.Marks.
                Where(p => p.IdDiscipline == journalCurrent.IdDiscipline).ToList();
            currentMarks = currentMarks.Where(p => p.Student.Group == journalCurrent.Group).ToList();
            if (CmbSortStudent.SelectedIndex > 0)
            {
                currentMarks = currentMarks.Where(p => p.IdStudent == (CmbSortStudent.SelectedItem as Student).Id).ToList();
            }

            if (DpDateMarkSorting.SelectedDate != null)
            {
                currentMarks = currentMarks.Where(p => p.Date == DpDateMarkSorting.SelectedDate).OrderByDescending(p => p.Date).ToList();
            }


            if (currentMarks.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }

            DGridMarks.ItemsSource = currentMarks.OrderByDescending(p => p.Date);
        }

        /// <summary>
        /// При изменении текста происходит поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMarks();
        }
        
        /// <summary>
        /// По нажатию кнопки - удаление оценок
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnDeleteMark_Click(object sender, RoutedEventArgs e)
        {
            var marksRemoving = DGridMarks.SelectedItems.Cast<Mark>().ToList();
            if (marksRemoving.Count == 0)
            {
                MessageBox.Show("Выберите оценки для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (MessageBox.Show($"Вы точно хотите удалить {marksRemoving.Count} оценок?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, 
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.DataBase.Marks.RemoveRange(marksRemoving);
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateMarks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// По нажатию на кнопку - редактирование выбранной оценки
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnEditMark_Click(object sender, RoutedEventArgs e)
        {
            markCurrent = (sender as Button).DataContext as Mark;
            GbMark.DataContext = markCurrent;
            TextAddOrEdit.Text = "Сохранить";
            ImgBtn.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Edit.png"));
            GbMark.Header = "Редактирование оценки";
        }

        /// <summary>
        /// При загрузки страницы - обновление данных
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextTeacher.Text = $"Преподаватель: {journalCurrent.User.UserName}";
            CmbMark.ItemsSource = GenerationMarks();
            var students = App.DataBase.Students.Where(p => p.Group.Id == journalCurrent.IdGroup).ToList();
            CmbStudent.ItemsSource = students;
            var studentsSort = App.DataBase.Students.Where(p => p.Group.Id == journalCurrent.IdGroup).ToList();
            studentsSort.Insert(0, new Student
            {
                LastName = "Все студенты",
                FirstName = " "
            });
            CmbSortStudent.ItemsSource = studentsSort;
            CmbSortStudent.SelectedIndex = 0;
            UpdateMarks();
        }

        /// <summary>
        /// Генерация оценок (возможные оценки 2, 3, 4, 5)
        /// </summary>
        /// <returns>коллекция оценок (2, 3, 4, 5)</returns>
        private static List<string> GenerationMarks()
        {
            var marksValue = new List<string>
            {
                "2",
                "3",
                "4",
                "5"
            };
            return marksValue;
        }

        /// <summary>
        /// При выборе даты - сортировка оценок по этой дате
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void DpDateMarkSorting_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarks();
        }

        /// <summary>
        /// Сброс значений при редактировании/добавлении оценки
        /// </summary>
        public void Reset()
        {
            markCurrent = new Mark();
            GbMark.DataContext = null;
            CmbStudent.SelectedIndex = -1;
            CmbMark.SelectedIndex = -1;
            DpDateMark.SelectedDate = null;
            TextAddOrEdit.Text = "Добавить";
            GbMark.Header = "Добавление оценки";
            ImgBtn.Source = BitmapFrame.Create(new Uri(@"pack://application:,,,/Resources/Add.png"));
        }

        /// <summary>
        /// По нажатию на кнопку - сброс данных при добавлении/редактировании оценки
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// При нажатии на кнопку - сохранение данных (добавление оценки/редкатирование)
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSaveOrAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (CmbStudent.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите студента");
            }
            if (CmbMark.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите оценку");
            }
            if (DpDateMark.SelectedDate == null)
            {
                errors.AppendLine("Выберите дату");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (markCurrent.Id == 0)
            {
                Mark mark = new Mark
                {
                    Discipline = journalCurrent.Discipline,
                    IdDiscipline = journalCurrent.IdDiscipline,
                    IdUser = journalCurrent.IdUser,
                    User = journalCurrent.User,
                    Date = (DateTime)DpDateMark.SelectedDate,
                    IdStudent = (CmbStudent.SelectedItem as Student).Id,
                    MarkValue = CmbMark.SelectedItem.ToString(),
                    Student = CmbStudent.SelectedItem as Student,
                };
                App.DataBase.Marks.Add(mark);
            }

            Reset();
            try
            {
                App.DataBase.SaveChanges();
                MessageBox.Show($"Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateMarks();
            CmbStudent.SelectedIndex = -1;
            CmbMark.SelectedIndex = -1;
            DpDateMark.SelectedDate = null;
        }

        /// <summary>
        /// При выборе определенного студента - оценки отображаются этого студента
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void CmbSortStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMarks();
        }
    }
}
