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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace ScholarshipAppointment
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private bool passwordFlag1; //Флаг скрытия и отображения пароля

        private bool passwordFlag2; //Флаг скрытия и отображения повторно введенного пароля

        //Экземпляр класса Bobrovski_4IS1_DataBase_ScholarshipAppointmentEntities для получения базы данных с сервера
        private DataBase.Bobrovski_4IS1_DataBase_ScholarshipAppointment2Entities myDataBase;

        //Конструктор
        public RegistrationWindow(DataBase.Bobrovski_4IS1_DataBase_ScholarshipAppointment2Entities myDataBase)
        {
            InitializeComponent();
            passwordFlag1 = false;
            passwordFlag2 = false;
            this.myDataBase = myDataBase;
        }

        //Событие кнопки скрытия и отображения пароля
        private void PasswordButtonClick1(object sender, RoutedEventArgs e)
        {
            
            if (passwordFlag1 == false)
            {
                TextBox1.Text = PasswordBox1.Password;
                TextBox1.Width = PasswordBox1.Width;
                PasswordBox1.Width = 0;
                PasswordButton1.Content = "Скрыть";
            }
            else
            {
                PasswordBox1.Password = TextBox1.Text;
                PasswordBox1.Width = TextBox1.Width;
                TextBox1.Width = 0;
                PasswordButton1.Content = "Показать";
            }
            passwordFlag1 = !passwordFlag1;
        }

        //Событие кнопки скрытия и отображения повторно введенного пароля
        private void PasswordButtonClick2(object sender, RoutedEventArgs e)
        {
            if (passwordFlag2 == false)
            {
                TextBox2.Text = PasswordBox2.Password;
                TextBox2.Width = PasswordBox2.Width;
                PasswordBox2.Width = 0;
                PasswordButton2.Content = "Скрыть";
            }
            else
            {
                PasswordBox2.Password = TextBox2.Text;
                PasswordBox2.Width = TextBox2.Width;
                TextBox2.Width = 0;
                PasswordButton2.Content = "Показать";
            }
            passwordFlag2 = !passwordFlag2;
        }

        //Валидация пароля
        private bool PasswordValidation(string password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[\-_?!%&$@#*])[a-zA-Z0-9\-_?!%&$@#*]{8,}$");
            return regex.IsMatch(password);
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (FIOBox.Text.Length > 100)
            {
                MessageBox.Show("ФИО сотрудника должно быть не более 100 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (FIOBox.Text == "")
            {
                MessageBox.Show("Введите ФИО пользователя!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (myDataBase.Users.SingleOrDefault(u => u.uLogin == LoginBox.Text) != null)
            {
                MessageBox.Show("Пользователь с таким логином уже сущетвует!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (LoginBox.Text.Length > 50)
            {
                MessageBox.Show("Логин сотрудника должен быть не более 50 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (LoginBox.Text.Length < 4)
            {
                MessageBox.Show("Логин должен быть не менее 4 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (PasswordValidation(PasswordBox1.Password) != true)
            {
                MessageBox.Show("Пароль должен быть не менее 8 символов, содержать большие и маленькие латинские буквы, цифры и хотя бы один спец. символ из следующего набора -_?!%&$@#*", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (PasswordBox1.Password != PasswordBox2.Password)
            {
                MessageBox.Show("Пароли не равны!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        //Событие кнопки регистрации в клиент-серверном приложении
        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            if (passwordFlag1 == true)
            {
                PasswordBox1.Password = TextBox1.Text;
            }
            if (passwordFlag2 == true)
            {
                PasswordBox2.Password = TextBox2.Text;
            }
            if (CheckInfo())
            {
                DataBase.Users newUser = new DataBase.Users();
                newUser.fio = FIOBox.Text;
                newUser.uLogin = LoginBox.Text;
                newUser.uPassword = PasswordBox1.Password;
                newUser.uAdmin = null;
                myDataBase.Users.Add(newUser);
                myDataBase.SaveChanges();
                Close();
            }
        }

        //Событие кнопки выхода из окна регистрации
        private void RollBackButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
