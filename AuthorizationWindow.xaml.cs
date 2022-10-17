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

namespace ScholarshipAppointment
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private bool passwordFlag; //Флаг скрытия и отображения пароля

        //Экземпляр класса Bobrovski_4IS1_DataBase_ScholarshipAppointmentEntitie для получения базы данных с сервера
        private DataBase.Bobrovski_4IS1_DataBase_ScholarshipAppointment2Entities myDataBase;

        //Конструктор
        public AuthorizationWindow()
        {
            InitializeComponent();
            passwordFlag = false;
            try
            {
                myDataBase = new DataBase.Bobrovski_4IS1_DataBase_ScholarshipAppointment2Entities();
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к базе данных! Проверьте настройки подключения приложения.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
            }
        }

        //Событие кнопки скрытия и отображения пароля
        private void PasswordButtonClick(object sender, RoutedEventArgs e)
        {
            if (passwordFlag == false)
            {
                TextBox.Text = PasswordBox.Password;
                TextBox.Width = PasswordBox.Width;
                PasswordBox.Width = 0;
                PasswordButton.Content = "Скрыть";
            }
            else
            {
                PasswordBox.Password = TextBox.Text;
                PasswordBox.Width = TextBox.Width;
                TextBox.Width = 0;
                PasswordButton.Content = "Показать";
            }
            passwordFlag = !passwordFlag;
        }

        //Событие кнопки авторизации в клиент-серверном приложении
        private void AuthorizationButtonClick(object sender, RoutedEventArgs e)
        {
            if (passwordFlag == true)
            {
                PasswordBox.Password = TextBox.Text;
            }
            DataBase.Users user = SourceCore.getBase().Users.SingleOrDefault(u => u.uLogin == LoginBox.Text && u.uPassword == PasswordBox.Password);
            if ((user != null) && (user.uAdmin != null))
            {
                MainWindow window = new MainWindow();
                window.Show();
                SourceCore.currentUser = user;
                Close();
            }
            else
            {
                MessageBox.Show("Неверно указан логин и/или пароль!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие кнопки регистрации в клиент-серверном приложении
        public void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            RegistrationWindow window = new RegistrationWindow(myDataBase);
            window.ShowDialog();
        }
    }
}
