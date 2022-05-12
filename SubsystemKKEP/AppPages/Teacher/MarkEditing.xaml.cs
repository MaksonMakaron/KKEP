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

namespace SubsystemKKEP.AppPages.Teacher
{
    /// <summary>
    /// Логика взаимодействия для MarkEditing.xaml
    /// </summary>
    public partial class MarkEditing : Page
    {
        private Mark markCurrent = new Mark();

        public MarkEditing(Mark mark)
        {
            InitializeComponent();
            CmbMark.ItemsSource = GenerationMarks();
            if (mark != null)
            {
                markCurrent = mark;
            }
            DataContext = markCurrent;

            //CmbStudent.ItemsSource =
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
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
        /// По нажатию на кнопку - сохранение информации
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbStudent.SelectedIndex != 0 && CmbMark.SelectedIndex != 0
                && DpDateMark.SelectedDate != null)
            {
                var student = CmbStudent.SelectedItem;
                var mark = CmbMark.SelectedItem;
                var date = DpDateMark.SelectedDate;

                if (markCurrent.Id == 0)
                {
                    App.DataBase.Marks.Add(markCurrent);
                    
                }

                try
                {
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBox.Show($"Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                CmbStudent.SelectedIndex = -1;
                CmbMark.SelectedIndex = -1;
                DpDateMark.SelectedDate = null;
            }
            else
            {
                var errors = "";
                if (CmbStudent.SelectedIndex == -1)
                {
                    errors += "Выберите студента\n";
                }
                if (CmbMark.SelectedIndex == -1)
                {
                    errors += "Выберите оценку";
                }
                if (DpDateMark.SelectedDate == null)
                {
                    errors += "Выберите дату";
                }
                MessageBox.Show($"{errors}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
