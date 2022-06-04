using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubsystemKKEP.Classes;
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
    /// Логика взаимодействия для ReportCardPage.xaml
    /// </summary>
    public partial class ReportCardPage : Page
    {
        private Group currentGroup = new Group();

        public ReportCardPage(Group group)
        {
            InitializeComponent();
            currentGroup = group;
            TextGroup.Text = $"Студенты группы: {currentGroup.GroupName}";
            if (currentGroup.Students.Count == 0)
            {
                MessageBox.Show("Отсутствуют студенты. Обратитесь к администратору");
                return;
            }
            DGridGroups.ItemsSource = currentGroup.Students.ToList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }

        private void BtnAnalis_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPDF_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOpenReportCard_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new ReportCardConcreteStudentPage((sender as Button).DataContext as Student));
        }
    }
}
