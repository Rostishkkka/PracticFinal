using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskLog.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskLog
{
    /// <summary>
    /// Interaction logic for UserAddAndChangeWindow.xaml
    /// </summary>
    public partial class UserAddAndChangeWindow : Window
    {

        private bool ChangeMode = true;
        private long id;
        public UserAddAndChangeWindow()
        {
            InitializeComponent();
        }

        public UserAddAndChangeWindow(Users? user)
        {
            InitializeComponent();
            FillWindow(user);
        }

        private void FillWindow(Users? user)
        {
            if (user == null)
            {
                ChangeMode = false;
                EditUserWindow.Title = "Добавление пользователя";
                return;
            }
            else
            {
                UserNameTextBox.Text = user.UserName;
                UserEmailTextBox.Text = user.UserEmail;
                id = user.UserId;
                EditUserWindow.Title = "Изменение существуещего пользователя";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeMode)
            {
                EditCurrentUser();
            }
            else
            {
                AddNewUser();
            }
        }

        private void AddNewUser()
        {
            try
            {
                Users user = new Users();
                if(UserNameTextBox.Text.Length == 0 || UserEmailTextBox.Text.Length == 0)
                {
                    MessageBox.Show("Поля имени и электронной почты не могут быть пустыми", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if(!EmailValidation())
                {
                    MessageBox.Show("Неверный формат email", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if(!CheckSameMailInDB())
                {
                    MessageBox.Show("Данный email уже зарегистрирован", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (UserPassTextBox.Password.Length < 5)
                {
                    MessageBox.Show("Пароль должен содержать не менее 5 символов", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (UserPassTextBox.Password != RepeatPasswordTextBox.Password)
                {
                    MessageBox.Show("Пароли не совпадают", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                user.UserName = UserNameTextBox.Text;
                user.UserEmail = UserEmailTextBox.Text;
                user.HashedPass = HashPassword(UserPassTextBox.Password);
                DbUtils.db.Users.Add(user);
                DbUtils.db.SaveChanges();
                MessageBox.Show("Пользователь успешно создан", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch
            {
                MessageBox.Show("error");
            }
        }

        private void EditCurrentUser()
        {
            try
            {
                Users user = DbUtils.db.Users.FirstOrDefault(x => x.UserId == id);
                if (UserNameTextBox.Text.Length == 0 || UserEmailTextBox.Text.Length == 0)
                {
                    MessageBox.Show("Поля имени и электронной почты не могут быть пустыми", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!EmailValidation())
                {
                    MessageBox.Show("Неверный формат email", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (UserEmailTextBox.Text != user.UserEmail)
                {
                    if (!CheckSameMailInDB())
                    {
                        MessageBox.Show("Данный email уже зарегистрирован", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                if (UserPassTextBox.Password.Length == 0)
                {
                    user.HashedPass = user.HashedPass;
                }
                else if (UserPassTextBox.Password.Length < 5)
                {
                    MessageBox.Show("Пароль должен содержать не менее 5 символов", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (UserPassTextBox.Password != RepeatPasswordTextBox.Password)
                {
                    MessageBox.Show("Пароли не совпадают", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    user.HashedPass = HashPassword(UserPassTextBox.Password);
                }

                user.UserName = UserNameTextBox.Text;
                user.UserEmail = UserEmailTextBox.Text;
                DbUtils.db.SaveChanges();
                MessageBox.Show("Изменения успешно сохранены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка изменения пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private bool EmailValidation()
        {
            bool valid = true;
            Regex regex = new Regex(@"^[-\w.]+@([A-z0-9][A-z0-9]+)+\.[A-z]{2,4}$");
            if (!regex.Match(UserEmailTextBox.Text).Success)
            {
                valid = false;
            }
            return valid;
        }

        private bool CheckSameMailInDB()
        {
            if (DbUtils.db.Users.Any(x => x.UserEmail == UserEmailTextBox.Text))
            { 
                return false;
            }
            else
            {
                return true;
            }
        }
        
        private string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
