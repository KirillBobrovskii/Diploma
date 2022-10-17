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
    /// Логика взаимодействия для OrderTypeDlg.xaml
    /// </summary>
    public partial class OrderTypeDlg : Page
    {
        private Page page; //Хранение контекста страницы OrderTypes

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.OrderTypes type { get; set; } //Хранение данных о выбранной записи из таблицы OrderTypes

        //Конструктор
        public OrderTypeDlg(Page page, DataBase.OrderTypes type, bool dlgMode)
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
            Label.Content = "Добавить тип приказа";
            Commit.Content = "Добавить";
        }

        //Копирование записи в базе данных
        private void CopyType()
        {
            Label.Content = "Копировать тип приказа";
            Commit.Content = "Копировать";
            TypeName.Text = type.otName;
        }

        //Изменение записи в базе данных
        private void EditType()
        {
            Label.Content = "Изменить тип приказа";
            Commit.Content = "Изменить";
            TypeName.Text = type.otName;
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (TypeName.Text.Length > 100)
            {
                MessageBox.Show("Название типа приказа должно быть не более 100 символов!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (TypeName.Text == "")
            {
                MessageBox.Show("Введите тип приказа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    DataBase.OrderTypes newType = new DataBase.OrderTypes();
                    newType.otName = TypeName.Text;
                    SourceCore.getBase().OrderTypes.Add(newType);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.OrderTypes).UpdateGrid(newType);
                }
                else
                {
                    DataBase.OrderTypes updateType = SourceCore.getBase().OrderTypes.Where(t => t.idOT == type.idOT).FirstOrDefault();
                    updateType.otName = TypeName.Text;
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.OrderTypes).UpdateGrid(updateType);
                }
                (page as Pages.OrderTypes).DataGrid.Focus();
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
            (page as Pages.OrderTypes).DlgLoad(false);
        }
    }
}
