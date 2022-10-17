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
    /// Логика взаимодействия для OrderDlg.xaml
    /// </summary>
    public partial class OrderDlg : Page
    {
        private Page page; //Хранение контекста страницы Orders

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.Orders order { get; set; } //Хранение данных о выбранной записи из таблицы Orders

        //Конструктор
        public OrderDlg(Page page, DataBase.Orders order, bool dlgMode)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.order = order;
            this.dlgMode = dlgMode;
            OrderScholarshipType.ItemsSource = SourceCore.getBase().ScholarshipTypes.ToList();
            OrderType.ItemsSource = SourceCore.getBase().OrderTypes.ToList();
            OrderUser.ItemsSource = SourceCore.getBase().Users.ToList();
            if (dlgMode)
            {
                if (order == null)
                {
                    AddOrder();
                }
                else
                {
                    CopyOrder();
                }
            }
            else
            {
                EditOrder();
            }
        }

        //Добавление записи в базу данных
        private void AddOrder()
        {
            Label.Content = "Добавить приказ";
            Commit.Content = "Добавить";
            OrderDate.SelectedDate = DateTime.Now;
        }

        //Копирование записи в базе данных
        private void CopyOrder()
        {
            Label.Content = "Копировать приказ";
            Commit.Content = "Копировать";
            OrderScholarshipType.SelectedIndex = (int)order.idST - 1;
            OrderType.SelectedIndex = (int)order.idOT - 1;
            OrderDate.SelectedDate = order.docDate;
            OrderUser.SelectedIndex = (int)order.idUser - 1;
            OrderComm.Text = order.comm;
        }

        //Изменение записи в базе данных
        private void EditOrder()
        {
            Label.Content = "Изменить приказ";
            Commit.Content = "Изменить";
            OrderScholarshipType.SelectedItem = order.ScholarshipTypes;
            OrderType.SelectedItem = order.OrderTypes;
            OrderDate.SelectedDate = order.docDate;
            OrderUser.SelectedItem = order.Users;
            OrderComm.Text = order.comm;
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (OrderScholarshipType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип стипендии!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (OrderType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип приказа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (OrderDate.SelectedDate == null)
            {
                MessageBox.Show("Введите дату приказа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (OrderUser.SelectedItem == null)
            {
                MessageBox.Show("Введите сотрудника!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    DataBase.Orders newOrder = new DataBase.Orders();
                    newOrder.ScholarshipTypes = (DataBase.ScholarshipTypes)OrderScholarshipType.SelectedItem;
                    newOrder.OrderTypes = (DataBase.OrderTypes)OrderType.SelectedItem;
                    newOrder.docDate = OrderDate.SelectedDate;
                    newOrder.Users = (DataBase.Users)OrderUser.SelectedItem;
                    newOrder.signDate = null;
                    newOrder.comm = OrderComm.Text;
                    SourceCore.getBase().Orders.Add(newOrder);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Orders).UpdateOrderGrid(newOrder);
                }
                else
                {
                    DataBase.Orders updateOrder = SourceCore.getBase().Orders.Where(o => o.idOrder == order.idOrder).FirstOrDefault();
                    updateOrder.ScholarshipTypes = (DataBase.ScholarshipTypes)OrderScholarshipType.SelectedItem;
                    updateOrder.OrderTypes = (DataBase.OrderTypes)OrderType.SelectedItem;
                    updateOrder.docDate = OrderDate.SelectedDate;
                    updateOrder.Users = (DataBase.Users)OrderUser.SelectedItem;
                    updateOrder.signDate = null;
                    updateOrder.comm = OrderComm.Text;
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Orders).UpdateOrderGrid(updateOrder);
                }
                (page as Pages.Orders).OrderDataGrid.Focus();
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
            (page as Pages.Orders).DlgLoad(false);
            (page as Pages.Orders).Frame.Navigate(null);
        }
    }
}
