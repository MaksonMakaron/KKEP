﻿using SubsystemKKEP.Classes;
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
    /// Логика взаимодействия для StudentsPage.xaml
    /// </summary>
    public partial class StudentsPage : Page
    {
        public StudentsPage()
        {
            InitializeComponent();
        }

        private void UpdateStudents()
        {
            var currentStudents = App.DataBase.Students.ToList();

            if (CmbSortGroup.SelectedIndex > 0)
            {
                currentStudents = currentStudents.Where(p => p.IdGroup == (CmbSortGroup.SelectedItem as Group).Id).ToList();
            }
            
            currentStudents = currentStudents.Where(p => p.FullName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            if (currentStudents.Count() > 0)
            {
                PopupSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                PopupSearch.Visibility = Visibility.Visible;
            }
            DGridStudents.ItemsSource = currentStudents.OrderBy(p => p.FullName);
        }


        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var allGrops = App.DataBase.Groups.ToList();
            allGrops.Insert(0, new Group
            {
                GroupName = "Все группы"
            });
            CmbSortGroup.ItemsSource = allGrops;
            CmbSortGroup.SelectedIndex = 0;
            UpdateStudents();
        }

        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new StudentAddEditPage(null));
        }

        private void BtnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            var studentsRemoving = DGridStudents.SelectedItems.Cast<Student>().ToList();
            if (studentsRemoving.Count == 0)
            {
                MessageBox.Show($"Выберите студентов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"Вы точно хотите удалить {studentsRemoving.Count} студентов?",
                "Подтверждение удаления", MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.DataBase.Students.RemoveRange(studentsRemoving);
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateStudents();
        }

        private void CmbSortGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStudents();
        }

        private void BtnEditStudent_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.Navigate(new StudentAddEditPage((sender as Button).DataContext as Student));
        }
    }
}
