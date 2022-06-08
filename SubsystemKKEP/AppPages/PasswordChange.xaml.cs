﻿using System;
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

namespace SubsystemKKEP.AppPages
{
    /// <summary>
    /// Логика взаимодействия для PasswordChange.xaml
    /// </summary>
    public partial class PasswordChange : Page
    {
        /// <summary>
        /// Загрузка страницы
        /// </summary>
        public PasswordChange()
        {
            InitializeComponent();
        }

        /// <summary>
        /// При нажатии кнопки мыши - старый пароль скрывается 
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSeeOldPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PasswordLoginManagement.SeePasswordPreviewMouseUp(TbOldPassword, PbOldPassword);
        }

        /// <summary>
        /// При нажатии кнопки мыши - старый пароль показывается 
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnSeeOldPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordLoginManagement.SeePasswordPreviewMouseDown(TbOldPassword, PbOldPassword);
        }

        /// <summary>
        /// При нажатии кнопки мыши - новый пароль показывается 
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnNewSeePassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordLoginManagement.SeePasswordPreviewMouseDown(TbNewPassword, PbNewPassword);
        }

        /// <summary>
        /// При нажатии кнопки мыши - новый пароль скрывается 
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnNewSeePassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PasswordLoginManagement.SeePasswordPreviewMouseUp(TbNewPassword, PbNewPassword);
        }

        /// <summary>
        /// По клику на кнопку - происходит смена пароля
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var oldPassword = PbOldPassword.Password;
            var newPassword = PbNewPassword.Password;
            var passwordCheck = PasswordLoginManagement.PasswordCheck(oldPassword);
            if (!string.IsNullOrWhiteSpace(oldPassword) && !string.IsNullOrWhiteSpace(newPassword) 
                && oldPassword != newPassword && passwordCheck)
            {
                PasswordLoginManagement.ChangePassword(newPassword);
                PbOldPassword.Password = "";
                PbNewPassword.Password = "";
                TbNewPassword.Text = "";
                TbOldPassword.Text = "";
                MessageBox.Show("Пароль успешно изменен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                InterfaceManagement.ManagementPage.GoBack();
            }
            else
            {
                var errors = "";
                if (string.IsNullOrWhiteSpace(oldPassword))
                {
                    errors += "Введите старый пароль\n";
                }
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    errors += "Введите новый пароль\n";
                }
                if (oldPassword == newPassword && passwordCheck 
                    && !string.IsNullOrWhiteSpace(newPassword) 
                    && !string.IsNullOrWhiteSpace(oldPassword))
                {
                    errors += "Новый пароль не может совпадать со старым\n";
                }
                if (passwordCheck == false && !string.IsNullOrWhiteSpace(oldPassword))
                {
                    errors += "Введённый пароль не совпадает со старым";
                }
                MessageBox.Show($"{errors}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// По нажатию на кнопку - возврат на предыдущую страницу
        /// </summary>
        /// <param name="sender">предоставляет ссылку на объект, который вызвал событие</param>
        /// <param name="e">передает объект, относящийся к обрабатываемому событию</param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PbOldPassword.Password) || !string.IsNullOrWhiteSpace(PbNewPassword.Password))
            {
                if (MessageBox.Show("У вас есть несохраненные изменения.\nВы уверены, что хотите вернуться назад?",
                "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    InterfaceManagement.ManagementPage.GoBack();
                }
            }
            else
            {
                InterfaceManagement.ManagementPage.GoBack();
            }
        }
    }
}
