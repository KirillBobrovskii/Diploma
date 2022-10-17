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
    /// Логика взаимодействия для OrderStringDlg.xaml
    /// </summary>
    public partial class OrderStringDlg : Page
    {
        private Page page; //Хранение контекста страницы Orders

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.OrderStrings orderString { get; set; } //Хранение данных о выбранной записи из таблицы OrderStrings

        private DataBase.Orders order { get; set; } //Хранение данных о выбранной записи из таблицы Orders

        //Конструктор
        public OrderStringDlg(Page page, DataBase.OrderStrings orderString, DataBase.Orders order, bool dlgMode)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.orderString = orderString;
            this.order = order;
            this.dlgMode = dlgMode;
            OrderStringGroups.ItemsSource = SourceCore.getBase().Groups.ToList();
            OrderStringStudent.ItemsSource = SourceCore.getBase().Students.ToList();
            if (dlgMode)
            {
                if (orderString == null)
                {
                    AddOrderString();
                    OrderStringStudent.SelectionMode = SelectionMode.Multiple;
                    DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    OrderStringStartDate.SelectedDate = startDate;
                    OrderStringFinishDate.SelectedDate = startDate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    CopyOrderString();
                    OrderStringStudent.SelectionMode = SelectionMode.Single;
                }
            }
            else
            {
                EditOrderString();
                OrderStringStudent.SelectionMode = SelectionMode.Single;
            }
        }

        //Фильтация таблицы со студентами
        private void FilterComboboxBoxTextChanged(object sender, SelectionChangedEventArgs e)
        {
            DataBase.Groups Group = (DataBase.Groups)OrderStringGroups.SelectedItem;
            OrderStringStudent.ItemsSource = SourceCore.getBase().Students.Where(s => s.idGroup == Group.idGroup).ToList();
        }

        //Добавление записи в базу данных
        private void AddOrderString()
        {
            Label.Content = "Добавить строку приказа";
            Commit.Content = "Добавить";
        }

        //Установка данных из выбранной записи в поля данных
        private void SelectOrderString()
        {
            OrderStringCost.Text = orderString.cost.ToString();
            OrderStringStartDate.SelectedDate = orderString.startDate;
            OrderStringFinishDate.SelectedDate = orderString.finishDate;
        }

        //Копирование записи в базе данных
        private void CopyOrderString()
        {
            Label.Content = "Копировать строку приказа";
            Commit.Content = "Копировать";
            OrderStringStudent.SelectedIndex = (int)orderString.idStud - 1;
            SelectOrderString();
        }

        //Изменение записи в базе данных
        private void EditOrderString()
        {
            Label.Content = "Изменить строку приказа";
            Commit.Content = "Изменить";
            OrderStringStudent.SelectedItem = orderString.Students;
            SelectOrderString();
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (OrderStringStudent.SelectedItem == null)
            {
                MessageBox.Show("Выберите студента!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (OrderStringCost.Text.Length < 14)
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
            if (OrderStringCost.Text == "")
            {
                MessageBox.Show("Введите сумму стипендии!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (OrderStringStartDate.SelectedDate == null)
            {
                MessageBox.Show("Введите дату начала выплаты стипендии!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (OrderStringFinishDate.SelectedDate == null)
            {
                MessageBox.Show("Введите дату конца выплаты стипендии!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        //Форматирование введённой пользоваиелем суммы стипендии
        private string Cost()
        {
            decimal cost = decimal.Parse(OrderStringCost.Text.Replace(".", ","));
            return cost.ToString("0.00");
        }

        //Отправка данных
        private void CommitButtonClick(object sender, RoutedEventArgs e)
        {
            if (CheckInfo() == true)
            {
                if (dlgMode)
                {
                    foreach (DataBase.Students student in OrderStringStudent.SelectedItems)
                    {
                        if (SourceCore.getBase().OrderStrings.SingleOrDefault(o => o.idStud == student.idStud && o.idOrder == order.idOrder) == null)
                        {
                            DataBase.OrderStrings newOrderString = new DataBase.OrderStrings();
                            newOrderString.idOrder = order.idOrder;
                            newOrderString.Students = student;
                            newOrderString.cost = decimal.Parse(Cost());
                            newOrderString.startDate = OrderStringStartDate.SelectedDate;
                            newOrderString.finishDate = OrderStringFinishDate.SelectedDate;
                            SourceCore.getBase().OrderStrings.Add(newOrderString);
                            SourceCore.getBase().SaveChanges();
                            (page as Pages.Orders).UpdateOrderStringGrid(newOrderString);
                        }
                        else
                        {
                            MessageBox.Show("Студент " + student.fio + " уже есть в приказе!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                else
                {
                    DataBase.OrderStrings updateOrderString = SourceCore.getBase().OrderStrings.Where(o => o.idOS == orderString.idOS).FirstOrDefault();
                    updateOrderString.Students = (DataBase.Students)OrderStringStudent.SelectedItem;
                    updateOrderString.cost = decimal.Parse(Cost());
                    updateOrderString.startDate = OrderStringStartDate.SelectedDate;
                    updateOrderString.finishDate = OrderStringFinishDate.SelectedDate;
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Orders).UpdateOrderStringGrid(updateOrderString);
                }
                (page as Pages.Orders).OrderStringDataGrid.Focus();
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
