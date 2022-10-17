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
    /// Логика взаимодействия для ScholarshipTypeDlg.xaml
    /// </summary>
    public partial class ScholarshipTypeDlg : Page
    {
        private Page page; //Хранение контекста страницы Bills

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.ScholarshipTypes type { get; set; } //Хранение данных о выбранной записи из таблицы Bills

        //Конструктор
        public ScholarshipTypeDlg(Page page, DataBase.ScholarshipTypes type, bool dlgMode)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.type = type;
            this.dlgMode = dlgMode;
            if (dlgMode)
            {
                if (type == null)
                {
                    AddType();
                }
                else
                {
                    CopyType();
                }
            }
            else
            {
                EditType();
            }
        }

        //Добавление записи в базу данных
        private void AddType()
        {
            Label.Content = "Добавить тип стипендии";
            Commit.Content = "Добавить";
        }

        //Установка данных из выбранной записи в поля данных
        private void SelectType()
        {
            TypeName.Text = type.stName;
            TypeCost.Text = type.cost.ToString();
        }

        //Копирование записи в базе данных
        private void CopyType()
        {
            Label.Content = "Копировать тип стипендии";
            Commit.Content = "Копировать";
            SelectType();
        }

        //Изменение записи в базе данных
        private void EditType()
        {
            Label.Content = "Изменить тип стипендии";
            Commit.Content = "Изменить";
            SelectType();
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (TypeName.Text.Length > 100)
            {
                MessageBox.Show("Тип степендии должен быть не более 100 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (TypeName.Text == "")
            {
                MessageBox.Show("Введите тип степендии!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (TypeCost.Text.Length < 14)
            {
                try
                {
                    Cost();
                }
                catch
                {
                    MessageBox.Show("Введена неверная сумма стипендии!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Введена неверная сумма стипендии!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (TypeCost.Text == "")
            {
                MessageBox.Show("Введите стоимость степендии!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        //Форматирование введённой пользователем суммы стипендии
        private string Cost()
        {
            decimal cost = decimal.Parse(TypeCost.Text.Replace(".", ","));
            return cost.ToString("0.00");
        }

        //Отправка данных
        private void CommitButtonClick(object sender, RoutedEventArgs e)
        {
            if (CheckInfo() == true)
            {
                if (dlgMode)
                {
                    DataBase.ScholarshipTypes newType = new DataBase.ScholarshipTypes();
                    newType.stName = TypeName.Text;
                    newType.cost = decimal.Parse(Cost());
                    SourceCore.getBase().ScholarshipTypes.Add(newType);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.ScholarshipTypes).UpdateGrid(newType);
                }
                else
                {
                    DataBase.ScholarshipTypes updateType = SourceCore.getBase().ScholarshipTypes.Where(t => t.idST == type.idST).FirstOrDefault();
                    updateType.stName = TypeName.Text;
                    updateType.cost = decimal.Parse(Cost());
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.ScholarshipTypes).UpdateGrid(updateType);
                }
                (page as Pages.ScholarshipTypes).DataGrid.Focus();
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
            (page as Pages.ScholarshipTypes).DlgLoad(false);
        }
    }
}
