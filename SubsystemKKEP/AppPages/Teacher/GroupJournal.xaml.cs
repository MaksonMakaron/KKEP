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
        private static Appointment journalCurrent = new Appointment();

        public GroupJournal(Appointment selectedJournal)
        {
            InitializeComponent();
            TextTeacher.Text = $"Преподаватель: {selectedJournal.User.UserName}";
            journalCurrent = selectedJournal;
            UpdateMarks();
            if (DGridMarks.Items.Count == 0)
            {
                MessageBox.Show($"Отсутсвуют оценки", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                DGridMarks.IsEnabled = false;
                TbSearch.IsEnabled = false;
            }
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

        private void UpdateMarks()
        {
            var currentMarks = App.DataBase.Marks.
                Where(p => p.IdDiscipline == journalCurrent.IdDiscipline).ToList();

            currentMarks = currentMarks.Where(p => p.Student.Groups.Contains(journalCurrent.Group)).ToList();

            currentMarks = currentMarks.Where(p => p.Student.FullName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            DGridMarks.ItemsSource = currentMarks;
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
            InterfaceManagement.ManagementPage.Navigate(new MarkEditing(null, journalCurrent));
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
            InterfaceManagement.ManagementPage.Navigate(new MarkEditing((sender as Button).DataContext as Mark, journalCurrent));
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
                UpdateMarks();
            }
        }
    }
}
