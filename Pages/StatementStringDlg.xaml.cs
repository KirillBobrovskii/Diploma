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
    /// Логика взаимодействия для StatementStringDlg.xaml
    /// </summary>
    public partial class StatementStringDlg : Page
    {
        private Page page; //Хранение контекста страницы Statements

        private DataBase.StatementStrings statementString { get; set; } //Хранение данных о выбранной записи из таблицы StatementStrings

        private DataBase.Statements statement { get; set; } //Хранение данных о выбранной записи из таблицы Statements

        //Конструктор
        public StatementStringDlg(Page page, DataBase.StatementStrings statementString, DataBase.Statements statement)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.statementString = statementString;
            this.statement = statement;
            LoadOrderStrings();
            AddStatementString();
            OrderStringDataGrid.SelectionMode = DataGridSelectionMode.Extended;
        }

        //Сортировка таблицы OrderStrings
        private void LoadOrderStrings()
        {
            List<DataBase.OrderStrings> orderStrings1 = SourceCore.getBase().OrderStrings.Where(o => o.startDate <= new DateTime((int)statement.yearNum, (int)statement.monthNum, 1) && o.finishDate > new DateTime((int)statement.yearNum, (int)statement.monthNum, 1) && o.Orders.signDate != null && o.Orders.idOT == 1).ToList();
            List<DataBase.OrderStrings> orderStrings2 = SourceCore.getBase().OrderStrings.Where(o => o.startDate <= new DateTime((int)statement.yearNum, (int)statement.monthNum, 1) && o.finishDate > new DateTime((int)statement.yearNum, (int)statement.monthNum, 1) && o.Orders.signDate != null && o.Orders.idOT == 2).ToList();
            List<DataBase.OrderStrings> orderStrings3 = new List<DataBase.OrderStrings>();
            for (int i = 0; i < orderStrings1.Count; i++)
            {
                if (orderStrings2.FirstOrDefault(o => o.idStud == orderStrings1[i].idStud && o.Orders.idST == orderStrings1[i].Orders.idST) == null)
                {
                    orderStrings3.Add(orderStrings1[i]);
                }
            }
            OrderStringDataGrid.ItemsSource = orderStrings3;
            OrderStringDataGrid.SelectedIndex = 0;
        }

        //Добавление записи в базу данных
        private void AddStatementString()
        {
            Label.Content = "Добавить строку ведомости";
            Commit.Content = "Добавить";
        }

        //Отправка данных
        private void CommitButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (DataBase.OrderStrings orderString in OrderStringDataGrid.SelectedItems)
            {
                if (SourceCore.getBase().StatementStrings.SingleOrDefault(s => s.idOS == orderString.idOS && s.idStat == statement.idStat)  == null)
                {
                    int idStud = orderString.Students.idStud;
                    DataBase.Bills bufBill = (DataBase.Bills)SourceCore.getBase().Bills.Where(b => b.idStud == idStud).FirstOrDefault();
                    if (bufBill != null)
                    {
                        DataBase.StatementStrings newStatementString = new DataBase.StatementStrings();
                        newStatementString.OrderStrings = orderString;
                        newStatementString.idStat = statement.idStat;
                        newStatementString.cost = orderString.cost;
                        newStatementString.bill = bufBill.bill;
                        SourceCore.getBase().StatementStrings.Add(newStatementString);
                        SourceCore.getBase().SaveChanges();
                        (page as Pages.Statements).UpdateStatementStringGrid(newStatementString);
                        (page as Pages.Statements).StatementStringDataGrid.Focus();
                    }
                    else
                    {
                        MessageBox.Show("У студента " + orderString.Students.fio + " отсутствует номер счёта!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Студент " + orderString.Students.fio + " со стипендией в " + orderString.cost + " руб. уже есть в ведомости!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            CloseDlg();
        }

        //Событие кнопки закрытия диалоговой секции
        private void RollBackButtonClick(object sender, RoutedEventArgs e)
        {
            CloseDlg();
        }

        //Закрытие диалоговой секции
        private void CloseDlg()
        {
            (page as Pages.Statements).DlgLoad(false);
            (page as Pages.Statements).Frame.Navigate(null);
        }
    }
}
