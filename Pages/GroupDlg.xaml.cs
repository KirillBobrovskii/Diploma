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
    /// Логика взаимодействия для GroupDlg.xaml
    /// </summary>
    public partial class GroupDlg : Page
    {
        private Page page; //Хранение контекста страницы Groups

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.Groups group { get; set; } //Хранение данных о выбранной записи из таблицы Groups

        //Конструктор
        public GroupDlg(Page page, DataBase.Groups group, bool dlgMode)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.group = group;
            this.dlgMode = dlgMode;
            GroupSpecialty.ItemsSource = SourceCore.getBase().Specialties.ToList();
            if (dlgMode)
            {
                if (group == null)
                {
                    AddGroup();
                }
                else
                {
                    CopyGroup();
                }
            }
            else
            {
                EditGroup();
            }
        }

        //Добавление записи в базу данных
        private void AddGroup()
        {
            Label.Content = "Добавить группу";
            Commit.Content = "Добавить";
        }

        //Копирование записи в базе данных
        private void CopyGroup()
        {
            Label.Content = "Копировать группу";
            Commit.Content = "Копировать";
            GroupName.Text = group.gName;
            GroupSpecialty.SelectedIndex = (int)group.idSpec - 1;
        }

        //Изменение записи в базе данных
        private void EditGroup()
        {
            Label.Content = "Изменить группу";
            Commit.Content = "Изменить";
            GroupName.Text = group.gName;
            GroupSpecialty.SelectedItem = group.Specialties;
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (GroupName.Text.Length > 50)
            {
                MessageBox.Show("Название группы должно быть не болле 50 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (GroupName.Text == "")
            {
                MessageBox.Show("Введите название группы!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (GroupSpecialty.SelectedItem == null)
            {
                MessageBox.Show("Выберите специальность!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    DataBase.Groups newGroup = new DataBase.Groups();
                    newGroup.gName = GroupName.Text;
                    newGroup.Specialties = (DataBase.Specialties)GroupSpecialty.SelectedItem;
                    SourceCore.getBase().Groups.Add(newGroup);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Groups).UpdateGrid(newGroup);
                }
                else
                {
                    DataBase.Groups updateGroup = SourceCore.getBase().Groups.Where(g => g.idGroup == group.idGroup).FirstOrDefault();
                    updateGroup.gName = GroupName.Text;
                    updateGroup.Specialties = (DataBase.Specialties)GroupSpecialty.SelectedItem;
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Groups).UpdateGrid(updateGroup);
                }
                (page as Pages.Groups).DataGrid.Focus();
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
            (page as Pages.Groups).DlgLoad(false);
        }
    }
}
