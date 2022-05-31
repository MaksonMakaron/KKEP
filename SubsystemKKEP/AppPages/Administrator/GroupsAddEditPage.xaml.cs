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
    /// Логика взаимодействия для GroupsAddEditPage.xaml
    /// </summary>
    public partial class GroupsAddEditPage : Page
    {
        /// <summary>
        /// Текущая группа
        /// </summary>
        private Group currentGroup = new Group();

        /// <summary>
        /// Загрузка страницы
        /// </summary>
        /// <param name="group">выбранная группа</param>
        public GroupsAddEditPage(Group group)
        {
            InitializeComponent();
            if (group != null)
            {
                currentGroup = group;
            }
            var course = new List<string>
            {
                "1", "2", "3", "4"
            };
            CmbCourse.ItemsSource = course;
            var specialization = App.DataBase.Specializations.ToList();
            CmbSpecialization.ItemsSource = specialization;
            DataContext = currentGroup;
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
        /// При нажатию на кнопку - сохранение информации
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TbGroupName.Text))
            {
                errors.AppendLine("Введите наименование группы");
            }

            if (string.IsNullOrWhiteSpace(TbGroupNumber.Text))
            {
                errors.AppendLine("Введите номер группы");
            }

            if (CmbSpecialization.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите специальность");
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

            foreach (var group in App.DataBase.Groups.ToList())
            {
                if (group.GroupNumber == currentGroup.GroupNumber || group.GroupName == currentGroup.GroupName)
                {
                    MessageBox.Show("Такая группа уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (currentGroup.Id == 0)
            {
                App.DataBase.Groups.Add(currentGroup);
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
        /// При вводе группы - ограничение на символы, отличающиеся от цифр
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void TbGroupNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
