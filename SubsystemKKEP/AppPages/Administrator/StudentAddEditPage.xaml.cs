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
    /// Логика взаимодействия для StudentAddEditPage.xaml
    /// </summary>
    public partial class StudentAddEditPage : Page
    {
        private Student currentStudent = new Student();
        public StudentAddEditPage(Student student)
        {
            InitializeComponent();
            if (student != null)
            {
                currentStudent = student;
            }
            CmbGroup.ItemsSource = App.DataBase.Groups.ToList();
            DataContext = currentStudent;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TbLastName.Text))
            {
                errors.AppendLine("Введите фамилию");
            }
            if (string.IsNullOrWhiteSpace(TbFirstName.Text))
            {
                errors.AppendLine("Введите имя");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (currentStudent.Id == 0)
            {
                App.DataBase.Students.Add(currentStudent);
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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }
    }
}
