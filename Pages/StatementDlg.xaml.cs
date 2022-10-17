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
    /// Логика взаимодействия для StatementDlg.xaml
    /// </summary>
    public partial class StatementDlg : Page
    {
        private Page page; //Хранение контекста страницы Statements

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.Statements statement { get; set; } //Хранение данных о выбранной записи из таблицы Statements

        //Конструктор
        public StatementDlg(Page page, DataBase.Statements statement, bool dlgMode)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.statement = statement;
            this.dlgMode = dlgMode;
            if (dlgMode)
            {
                if (statement == null)
                {
                    AddStatement();
                }
                else
                {
                    CopyStatement();
                }
            }
            else
            {
                EditStatement();
            }
        }

        //Добавление записи в базу данных
        private void AddStatement()
        {
            Label.Content = "Добавить ведомость";
            Commit.Content = "Добавить";
            StatementDate.SelectedDate = DateTime.Now;
        }

        //Установка данных из выбранной записи в поля данных
        private void SelectStatement()
        {
            StatementDocNum.Text = statement.docNum;
            StatementDate.SelectedDate = statement.docDate;
            StatementMonthNum.Text = statement.monthNum.ToString();
            StatementYearNum.Text = statement.yearNum.ToString();
        }

        //Копирование записи в базе данных
        private void CopyStatement()
        {
            Label.Content = "Копировать ведомость";
            Commit.Content = "Копировать";
            SelectStatement();
        }

        //Изменение записи в базе данных
        private void EditStatement()
        {
            Label.Content = "Изменить ведомость";
            Commit.Content = "Изменить";
            SelectStatement();
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (StatementDocNum.Text == "")
            {
                MessageBox.Show("Введите номер документа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (StatementDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату ведомости!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (StatementMonthNum.Text == "")
            {
                MessageBox.Show("Введите номер месяца окончания ведомости!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (StatementYearNum.Text == "")
            {
                MessageBox.Show("Введите год окончания ведомости!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    DataBase.Statements newStatement = new DataBase.Statements();
                    newStatement.docNum = StatementDocNum.Text;
                    newStatement.docDate = StatementDate.SelectedDate;
                    newStatement.monthNum = int.Parse(StatementMonthNum.Text);
                    newStatement.yearNum = int.Parse(StatementYearNum.Text);
                    newStatement.signDate = null;
                    SourceCore.getBase().Statements.Add(newStatement);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Statements).UpdateGrid(newStatement);
                }
                else
                {
                    DataBase.Statements updateStatement = SourceCore.getBase().Statements.Where(s => s.idStat == statement.idStat).FirstOrDefault();
                    updateStatement.docNum = StatementDocNum.Text;
                    updateStatement.docDate = StatementDate.SelectedDate;
                    updateStatement.monthNum = int.Parse(StatementMonthNum.Text);
                    updateStatement.yearNum = int.Parse(StatementYearNum.Text);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Statements).UpdateGrid(updateStatement);
                }
                (page as Pages.Statements).StatementDataGrid.Focus();
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
            (page as Pages.Statements).DlgLoad(false);
            (page as Pages.Statements).Frame.Navigate(null);
        }
    }
}
