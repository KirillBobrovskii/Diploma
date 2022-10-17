using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScholarshipAppointment.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserDlg.xaml
    /// </summary>
    public partial class UserDlg : Page
    {
        private Page page; //Хранение контекста страницы Users

        private DataBase.Users user { get; set; } //Хранение данных о выбранной записи из таблицы Users

        //Конструктор
        public UserDlg(Page page, DataBase.Users user)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.user = user;
            UserAdmin.Items.Add("Админ");
            UserAdmin.Items.Add("Пользователь");
            if (user == null)
            {
                AddUser();
            }
            else
            {
                EditUser();
            }
        }

        //Добавление записи в базу данных
        private void AddUser()
        {
            Label.Content = "Добавить сотрудника";
            Commit.Content = "Добавить";
        }

        //Изменение записи в базе данных
        private void EditUser()
        {
            Label.Content = "Изменить студента";
            Commit.Content = "Изменить";
            UserFIO.Text = user.fio;
            UserLogin.Text = user.uLogin;
            UserPassword.Text = user.uPassword;
            if (user.uAdmin == true)
            {
                UserAdmin.SelectedIndex = 0;
            }
            else
            {
                UserAdmin.SelectedIndex = 1;
            }
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
            if (UserFIO.Text.Length > 100)
            {
                MessageBox.Show("ФИО сотрудника должно быть не более 100 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (UserFIO.Text == "")
            {
                MessageBox.Show("Введите ФИО сотрудника!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if ((user != null) && (SourceCore.getBase().Users.SingleOrDefault(u => u.uLogin == UserLogin.Text && u.idUser != user.idUser) != null))
            {
                MessageBox.Show("Пользователь с таким логином уже сущетвует!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (UserLogin.Text.Length > 50)
            {
                MessageBox.Show("Логин сотрудника должен быть не более 50 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (UserLogin.Text.Length < 4)
            {
                MessageBox.Show("Логин должен быть не менее 4 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (PasswordValidation(UserPassword.Text) != true)
            {
                MessageBox.Show("Пароль должен быть не менее 8 символов, содержать большие и маленькие латинские буквы, цифры и хотя бы один спец. символ из следующего набора -_?!%&$@#*", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (UserAdmin.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип пользователя", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        //Отправка данных
        private void CommitButtonClick(object sender, RoutedEventArgs e)
        {
            if (CheckInfo() == true)
            {
                if (user == null)
                {
                    DataBase.Users newUser = new DataBase.Users();
                    newUser.fio = UserFIO.Text;
                    newUser.uLogin = UserLogin.Text;
                    newUser.uPassword = UserPassword.Text;
                    if (UserAdmin.SelectedIndex == 0)
                    {
                        newUser.uAdmin = true;
                    }
                    else
                    {
                        newUser.uAdmin = false;
                    }
                    SourceCore.getBase().Users.Add(newUser);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Users).UpdateGrid(newUser);
                }
                else
                {
                    DataBase.Users updateUser = SourceCore.getBase().Users.Where(u => u.idUser == user.idUser).FirstOrDefault();
                    updateUser.fio = UserFIO.Text;
                    updateUser.uLogin = UserLogin.Text;
                    updateUser.uPassword = UserPassword.Text;
                    if (UserAdmin.SelectedIndex == 0)
                    {
                        updateUser.uAdmin = true;
                    }
                    else
                    {
                        updateUser.uAdmin = false;
                    }
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Users).UpdateGrid(updateUser);
                }
                (page as Pages.Users).DataGrid.Focus();
                CloseDlg();
            }
        }

        //Событие кнопки закрытия диалоговой секции
        private void RollBackButtonClick(object sender, RoutedEventArgs e)
        {
            CloseDlg();
        }

        //Закрытие диалоговой секции
        private void CloseDlg()
        {
            (page as Pages.Users).DlgLoad(false);
        }
    }
}
