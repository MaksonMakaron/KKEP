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
        /// <summary>
        /// Текущий журнал группы
        /// </summary>
        private static Appointment journalCurrent = new Appointment();

        /// <summary>
        /// Текущая оценка
        /// </summary>
        private Mark markCurrent = new Mark();

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
        }

        /// <summary>
        /// Обновление оценок и заполнения DGridMarks
        /// </summary>
        private void UpdateMarks()
        {
            var currentMarks = App.DataBase.Marks.
                Where(p => p.IdDiscipline == journalCurrent.IdDiscipline).ToList();

            currentMarks = currentMarks.Where(q => q.Student.Group == journalCurrent.Group).ToList();

            if (DpDateMarkSorting.SelectedDate != null)
            {
                currentMarks = currentMarks.Where(p => p.Date == DpDateMarkSorting.SelectedDate).ToList();
            }

            currentMarks = currentMarks.Where(p => p.Student.FullName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            DGridMarks.ItemsSource = currentMarks;
            if (DGridMarks.Items.Count == 0)
            {
                MessageBox.Show($"Отсутсвуют оценки", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                DGridMarks.IsEnabled = false;
                TbSearch.IsEnabled = false;
            }
            else
            {
                DGridMarks.IsEnabled = true;
                TbSearch.IsEnabled = true;
            }
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
        /// По нажатию на кнопку - переход на страницу с добавлением новой оценки
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnAddMark_Click(object sender, RoutedEventArgs e)
        {
            //InterfaceManagement.ManagementPage.Navigate(new MarkEditing(null, journalCurrent));
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
                    UpdateMarks();
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
            markCurrent = (sender as Button).DataContext as Mark;
            GbMark.DataContext = markCurrent;
            DpDateMark.SelectedDate = DateTime.Now;
            //InterfaceManagement.ManagementPage.Navigate(new MarkEditing((sender as Button).DataContext as Mark, journalCurrent));
        }

        /// <summary>
        /// При загрузки страницы - обновление оценок
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextTeacher.Text = $"Преподаватель: {journalCurrent.User.UserName}";
            CmbMark.ItemsSource = GenerationMarks();
            var students = App.DataBase.Students.Where(p => p.Group.Id == journalCurrent.IdGroup).ToList();
            CmbStudent.ItemsSource = students;
            BtnAddMark.ToolTip = "Внимание!";
            UpdateMarks();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
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
                App.DataBase.Marks.Add(markCurrent);
            }

            App.DataBase.SaveChanges();
            MessageBox.Show($"Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateMarks();
            CmbStudent.SelectedIndex = -1;
            CmbMark.SelectedIndex = -1;
            DpDateMark.SelectedDate = DateTime.Now;
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("","Справка", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
