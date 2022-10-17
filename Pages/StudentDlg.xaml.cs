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

namespace ScholarshipAppointment.Pages
{
    /// <summary>
    /// Логика взаимодействия для StudentDlg.xaml
    /// </summary>
    public partial class StudentDlg : Page
    {
        private Page page; //Хранение контекста страницы Students

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.Students student { get; set; } //Хранение данных о выбранной записи из таблицы Students

        //Конструктор
        public StudentDlg(Page page, DataBase.Students student, bool dlgMode)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.student = student;
            this.dlgMode = dlgMode;
            StudentGroup.ItemsSource = SourceCore.getBase().Groups.ToList();
            if (dlgMode)
            {
                if (student == null)
                {
                    AddStudent();
                }
                else
                {
                    CopyStudent();
                }
            }
            else
            {
                EditStudent();
            }
        }

        //Добавление записи в базу данных
        private void AddStudent()
        {
            Label.Content = "Добавить студента";
            Commit.Content = "Добавить";
        }

        //Установка данных из выбранной записи в поля данных
        private void SelectStudent()
        {
            StudentFIO.Text = student.fio;
            StudentBirthDay.SelectedDate = student.birthDay;
            StudentPassInfo.Text = student.passInfo;
        }

        //Копирование записи в базе данных
        private void CopyStudent()
        {
            Label.Content = "Копировать студента";
            Commit.Content = "Копировать";
            SelectStudent();
            StudentGroup.SelectedIndex = (int)student.idGroup - 1;
        }

        //Изменение записи в базе данных
        private void EditStudent()
        {
            Label.Content = "Изменить студента";
            Commit.Content = "Изменить";
            SelectStudent();
            StudentGroup.SelectedItem = student.Groups;
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (StudentFIO.Text.Length > 100)
            {
                MessageBox.Show("ФИО студента должно быть не более 100 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (StudentFIO.Text == "")
            {
                MessageBox.Show("Введите ФИО студента!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (StudentBirthDay.SelectedDate == null)
            {
                MessageBox.Show("Введите дату рождения студента!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (StudentPassInfo.Text.Length != 10)
            {
                MessageBox.Show("Введите паспортные данные студента (10 цифр)!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (StudentGroup.SelectedItem == null)
            {
                MessageBox.Show("Выберите группу для студента!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        //Отправка данных
        private void CommitButtonClick(object sender, RoutedEventArgs e)
        {
            if (CheckInfo() == true)
            {
                if (dlgMode)
                {
                    DataBase.Students newStudent = new DataBase.Students();
                    newStudent.fio = StudentFIO.Text;
                    newStudent.birthDay = StudentBirthDay.SelectedDate;
                    newStudent.passInfo = StudentPassInfo.Text;
                    newStudent.Groups = (DataBase.Groups)StudentGroup.SelectedItem;
                    SourceCore.getBase().Students.Add(newStudent);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Students).UpdateGrid(newStudent);
                }
                else
                {
                    DataBase.Students updateStudent = SourceCore.getBase().Students.Where(s => s.idStud == student.idStud).FirstOrDefault();
                    updateStudent.fio = StudentFIO.Text;
                    updateStudent.birthDay = StudentBirthDay.SelectedDate;
                    updateStudent.passInfo = StudentPassInfo.Text;
                    updateStudent.Groups = (DataBase.Groups)StudentGroup.SelectedItem;
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Students).UpdateGrid(updateStudent);
                }
                (page as Pages.Students).DataGrid.Focus();
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
            (page as Pages.Students).DlgLoad(false);
        }
    }
}
