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

namespace SubsystemKKEP.AppPages.Administrator
{
    /// <summary>
    /// Логика взаимодействия для SpecialtieAddEditPage.xaml
    /// </summary>
    public partial class SpecialtieAddEditPage : Page
    {
        private Specialization currentSpecialization = new Specialization();
        public SpecialtieAddEditPage(Specialization specialization)
        {
            InitializeComponent();
            if (specialization != null)
            {
                currentSpecialization = specialization;
            }
            CmbDepartment.ItemsSource = App.DataBase.Departments.ToList();
            DataContext = currentSpecialization;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TbSpecializationOfName.Text))
            {
                errors.AppendLine("Введите полное наименование специальности");
            }
            if (string.IsNullOrWhiteSpace(TbShortName.Text))
            {
                errors.AppendLine("Введите краткое наименование специальности");
            }
            if (CmbDepartment.SelectedIndex == -1)
            {
                errors.AppendLine("Выберите отделение");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (currentSpecialization.Id == 0)
            {
                App.DataBase.Specializations.Add(currentSpecialization);
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
    }
}
