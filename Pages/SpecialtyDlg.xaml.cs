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
    /// Логика взаимодействия для SpecialtyDlg.xaml
    /// </summary>
    public partial class SpecialtyDlg : Page
    {
        private Page page; //Хранение контекста страницы Specialties

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.Specialties specialty { get; set; } //Хранение данных о выбранной записи из таблицы Specialties

        //Конструктор
        public SpecialtyDlg(Page page, DataBase.Specialties specialty, bool dlgMode)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.specialty = specialty;
            this.dlgMode = dlgMode;
            if (dlgMode)
            {
                if (specialty == null)
                {
                    AddSpecialty();
                }
                else
                {
                    CopySpecialty();
                }
            }
            else
            {
                EditSpecialty();
            }
        }

        //Добавление записи в базу данных
        private void AddSpecialty()
        {
            Label.Content = "Добавить специальность";
            Commit.Content = "Добавить";
        }

        //Копирование записи в базе данных
        private void CopySpecialty()
        {
            Label.Content = "Копировать специальность";
            Commit.Content = "Копировать";
            SpecialtyName.Text = specialty.specName;
        }

        //Изменение записи в базе данных
        private void EditSpecialty()
        {
            Label.Content = "Изменить специальность";
            Commit.Content = "Изменить";
            SpecialtyName.Text = specialty.specName;
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (SpecialtyName.Text.Length > 100)
            {
                MessageBox.Show("Название специальности должно быть не более 100 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (SpecialtyName.Text == "")
            {
                MessageBox.Show("Введите название специальности!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    DataBase.Specialties newSpecialty = new DataBase.Specialties();
                    newSpecialty.specName = SpecialtyName.Text;
                    SourceCore.getBase().Specialties.Add(newSpecialty);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Specialties).UpdateGrid(newSpecialty);
                }
                else
                {
                    DataBase.Specialties updateSpecialty = SourceCore.getBase().Specialties.Where(s => s.idSpec == specialty.idSpec).FirstOrDefault();
                    updateSpecialty.specName = SpecialtyName.Text;
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Specialties).UpdateGrid(updateSpecialty);
                }
                (page as Pages.Specialties).DataGrid.Focus();
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
            (page as Pages.Specialties).DlgLoad(false);
        }
    }
}
