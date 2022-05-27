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
    /// Логика взаимодействия для UserAddEdit.xaml
    /// </summary>
    public partial class UserAddEdit : Page
    {
        private User currentUser = new User();
        public UserAddEdit(User user)
        {
            InitializeComponent();

            if (user != null)
            {
                currentUser = user;
                PbPassword.IsEnabled = false;
                TbPassword.IsEnabled = false;
            }
            CmbRole.ItemsSource = App.DataBase.Roles.ToList();
            DataContext = currentUser;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            InterfaceManagement.ManagementPage.GoBack();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbUserName.Text) && CmbRole.SelectedIndex != -1)
            {
                if (currentUser.Id == 0)
                {
                    if (PasswordLoginManagement.SearchLoginDontRepeat(TbLogin.Text) && !string.IsNullOrEmpty(PbPassword.Password))
                    {
                        currentUser.Password = PasswordLoginManagement.CreateSHA512(PbPassword.Password);
                        App.DataBase.Users.Add(currentUser);
                    }
                    else
                    {
                        var err = "";
                        if (true)
                        {
                            err += "Введите пароль\n";
                        }
                        if (true)
                        {
                            err += "Такой логин уже существует. Выберите другой";
                        }
                        MessageBox.Show(err, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                try
                {
                    App.DataBase.SaveChanges();
                    MessageBox.Show($"Информация сохранена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    InterfaceManagement.ManagementPage.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                var errors = "";
                if (string.IsNullOrEmpty(TbUserName.Text))
                {
                    errors += "Введите имя пользователя\n";
                }
                if (CmbRole.SelectedIndex == -1)
                {
                    errors += "Выберите роль пользователя\n";
                }
                
                MessageBox.Show($"{errors}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSeePassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordLoginManagement.SeePasswordPreviewMouseDown(TbPassword, PbPassword);
        }

        private void BtnSeePassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PasswordLoginManagement.SeePasswordPreviewMouseUp(TbPassword, PbPassword);
        }
    }
}
