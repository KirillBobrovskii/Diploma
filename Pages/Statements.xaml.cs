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
using System.Collections.ObjectModel;

namespace ScholarshipAppointment.Pages
{
    /// <summary>
    /// Логика взаимодействия для Statements.xaml
    /// </summary>
    public partial class Statements : Page
    {
        private CollectionViewSource StatementViewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу Statements

        private CollectionViewSource StatementStringViewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу StatementStrings

        private DataBase.Statements statement { get; set; } //Хранение данных о выбранной записи из таблицы Statements

        //Конструктор
        public Statements()
        {
            InitializeComponent();
            DataContext = this;
            StatementViewModel = new CollectionViewSource();
            StatementStringViewModel = new CollectionViewSource();
            UpdateGrid(null);
        }

        //Открытие и закрытие диалоговой секции
        public void DlgLoad(bool flag)
        {
            if (flag)
            {
                GridSplitter.Width = new GridLength(3);
                Dialog.Width = new GridLength(600);
                StatementDataGrid.IsHitTestVisible = false;
                StatementStringDataGrid.IsHitTestVisible = false;
                StatementButtonsPanel.IsEnabled = false;
                StatementStringButtonsPanel.IsEnabled = false;
                StatementStringFilterPanel.IsEnabled = false;
            }
            else
            {
                GridSplitter.Width = new GridLength(0);
                Dialog.Width = new GridLength(0);
                StatementDataGrid.IsHitTestVisible = true;
                StatementStringDataGrid.IsHitTestVisible = true;
                StatementButtonsPanel.IsEnabled = true;
                StatementStringButtonsPanel.IsEnabled = true;
                StatementStringFilterPanel.IsEnabled = true;
            }
        }

        //Получене выбранной записи таблицы Statements
        private DataBase.Statements SelectStatement()
        {
            int idStat = (StatementDataGrid.SelectedItem as DataBase.Statements).idStat;
            return SourceCore.getBase().Statements.Where(s => s.idStat == idStat).FirstOrDefault();
        }

        //Событие кнопки добавления записи в базу данных в таблицу Statements
        private void StatementAddButtonClick(object sender, RoutedEventArgs e)
        {
            DlgLoad(true);
            Frame.Navigate(new Pages.StatementDlg(this, null, true));
        }

        //Событие кнопки добавления записи в базу данных в таблицу StatementStrings
        private void StatementStringAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (StatementDataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                Frame.Navigate(new Pages.StatementStringDlg(this, null, SelectStatement()));
            }
            else
            {
                MessageBox.Show("Выберите ведомость!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие кнопки копирования записи в базе данных в таблице Statements
        private void StatementCopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (StatementDataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                int idStat = (StatementDataGrid.SelectedItem as DataBase.Statements).idStat;
                DataBase.Statements statement = SourceCore.getBase().Statements.AsNoTracking().Where(s => s.idStat == idStat).FirstOrDefault();
                Frame.Navigate(new Pages.StatementDlg(this, statement, true));
            }
            else
            {
                MessageBox.Show("Выберите ведомость!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие кнопки изменения записи в базе данных в таблице Statements
        private void StatementEditButtonClick(object sender, RoutedEventArgs e)
        {
            if (StatementDataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                Frame.Navigate(new Pages.StatementDlg(this, SelectStatement(), false));
            }
            else
            {
                MessageBox.Show("Выберите ведомость!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие изменения записи в базе данных в таблице Statements по двойному клику мыши
        private void StatementDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectStatement().signDate == null)
            {
                DlgLoad(true);
                Frame.Navigate(new Pages.StatementDlg(this, SelectStatement(), false));
            }
        }

        //Событие кнопки удаления записи из базы данных из таблицы Statements
        private void StatementDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (StatementDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить ведомость!", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    DataBase.Statements delitingStatement = (DataBase.Statements)StatementDataGrid.SelectedItem;
                    try
                    {
                        if (StatementDataGrid.SelectedIndex < StatementDataGrid.Items.Count - 1)
                        {
                            StatementDataGrid.SelectedIndex++;
                        }
                        else
                        {
                            if (StatementDataGrid.SelectedIndex > 0)
                            {
                                StatementDataGrid.SelectedIndex--;
                            }
                        }
                        DataBase.Statements selectingStatement = (DataBase.Statements)StatementDataGrid.SelectedItem;
                        StatementDataGrid.SelectedItem = delitingStatement;
                        SourceCore.getBase().Statements.Remove(delitingStatement);
                        SourceCore.getBase().SaveChanges();
                        UpdateGrid(selectingStatement);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(delitingStatement).Reload();
                        MessageBox.Show("Невозможно удалить ведомость, так как он используется в других справочниках базы данных!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    StatementDataGrid.Focus();
                }
            }
            else
            {
                MessageBox.Show("Выберите ведомость!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие кнопки удаления записи из базы данных из таблицы StatementStrings
        private void StatementStringDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            statement = (DataBase.Statements)StatementDataGrid.SelectedItem;
            if (StatementStringDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить строку ведомости!", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    DataBase.StatementStrings delitingStatementString = (DataBase.StatementStrings)StatementStringDataGrid.SelectedItem;
                    try
                    {
                        if (StatementStringDataGrid.SelectedIndex < StatementStringDataGrid.Items.Count - 1)
                        {
                            StatementStringDataGrid.SelectedIndex++;
                        }
                        else
                        {
                            if (StatementStringDataGrid.SelectedIndex > 0)
                            {
                                StatementStringDataGrid.SelectedIndex--;
                            }
                        }
                        DataBase.StatementStrings selectingStatementString = (DataBase.StatementStrings)StatementStringDataGrid.SelectedItem;
                        StatementStringDataGrid.SelectedItem = delitingStatementString;
                        SourceCore.getBase().StatementStrings.Remove(delitingStatementString);
                        SourceCore.getBase().SaveChanges();
                        UpdateStatementStringGrid(selectingStatementString);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(statement).Reload();
                        MessageBox.Show("Невозможно удалить строку приказа, так как она используется в других справочниках базы данных!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    StatementStringDataGrid.Focus();
                }
            }
            else
            {
                MessageBox.Show("Выберите строку приказа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Загрузка заголовков таблиц Orders и OrderStrings в combobox-ы для фильтрации
        private void PageLoad(object sender, RoutedEventArgs e)
        {
            List<String> Columns = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                Columns.Add(StatementDataGrid.Columns[i].Header.ToString());
            }
            StatementFilterBox.ItemsSource = Columns;
            StatementFilterBox.SelectedIndex = 0;
            foreach (DataGridColumn Column in StatementDataGrid.Columns)
            {
                Column.CanUserSort = false;
            }
            Columns = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                Columns.Add(StatementStringDataGrid.Columns[i].Header.ToString());
            }
            StatementStringFilterBox.ItemsSource = Columns;
            StatementStringFilterBox.SelectedIndex = 0;
            foreach (DataGridColumn Column in StatementStringDataGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }

        //Фильтрация данных таблицы Statements
        private void StatementFilterTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            List<DataBase.Statements> list = new List<DataBase.Statements>();
            switch (StatementFilterBox.SelectedIndex)
            {
                case 0:
                    StatementDataGrid.ItemsSource = SourceCore.getBase().Statements.Where(s => s.docNum.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    foreach (DataBase.Statements s in SourceCore.getBase().Statements)
                    {
                        if (s.docDate.Value.ToShortDateString().Contains(textbox.Text))
                        {
                            list.Add(s);
                        }
                    }
                    StatementDataGrid.ItemsSource = list;
                    break;
                case 2:
                    StatementDataGrid.ItemsSource = SourceCore.getBase().Statements.Where(s => s.monthNum.ToString().Contains(textbox.Text)).ToList();
                    break;
                case 3:
                    StatementDataGrid.ItemsSource = SourceCore.getBase().Statements.Where(s => s.yearNum.ToString().Contains(textbox.Text)).ToList();
                    break;
                case 4:
                    foreach (DataBase.Statements s in SourceCore.getBase().Statements)
                    {
                        if (s.signDate != null)
                        {
                            if (s.signDate.Value.ToShortDateString().Contains(textbox.Text))
                            {
                                list.Add(s);
                            }
                        }
                    }
                    StatementDataGrid.ItemsSource = list;
                    break;
            }
        }

        //События выбора записи из таблицы Statements при изменении типа фильтрации для того,
        //чтобы осуществить фильтрацию в таблице StatementStrings только по тем записям,
        //которые связаны с выбранной записью из таблицы Statements
        private void StatementStringFilterBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            statement = (DataBase.Statements)StatementDataGrid.SelectedItem;
        }

        //Фильтрация данных таблицы StatementStrings
        private void StatementStringFilterTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            switch (StatementStringFilterBox.SelectedIndex)
            {
                case 0:
                    StatementStringDataGrid.ItemsSource = SourceCore.getBase().StatementStrings.Where(s => s.OrderStrings.idOrder.ToString().Contains(textbox.Text) && s.idStat == statement.idStat).ToList();
                    break;
                case 1:
                    StatementStringDataGrid.ItemsSource = SourceCore.getBase().StatementStrings.Where(s => s.OrderStrings.Students.fio.Contains(textbox.Text) && s.idStat == statement.idStat).ToList();
                    break;
                case 2:
                    StatementStringDataGrid.ItemsSource = SourceCore.getBase().StatementStrings.Where(s => s.cost.ToString().Contains(textbox.Text) && s.idStat == statement.idStat).ToList();
                    break;
                case 3:
                    StatementStringDataGrid.ItemsSource = SourceCore.getBase().StatementStrings.Where(s => s.bill.Contains(textbox.Text) && s.idStat == statement.idStat).ToList();
                    break;
            }
        }

        //Кнопка подписи приказа
        private void SignButtonContent()
        {
            if (statement != null)
            {
                if (statement.signDate != null)
                {
                    StatementEditButton.IsEnabled = false;
                    StatementDeleteButton.IsEnabled = false;
                    StatementStringButtonsPanel.IsEnabled = false;
                }
                else
                {
                    StatementEditButton.IsEnabled = true;
                    StatementDeleteButton.IsEnabled = true;
                    StatementStringButtonsPanel.IsEnabled = true;
                }
            }
        }

        //Событие кнопки подписи приказа
        private void SignButtonClick(object sender, RoutedEventArgs e)
        {
            if (statement.signDate == null)
            {
                if (MessageBox.Show("Вы действительно хотите подписать ведомость. В дальнейшем её невозможно будет изменить или удалить!", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    statement.signDate = DateTime.Now;
                    SourceCore.getBase().SaveChanges();
                    SignButtonContent();
                    UpdateGrid(null);
                }
            }
            else
            {
                MessageBox.Show("Ведомость уже подписана!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие выбора записи из таблицы Statements и последующее обновления данных в таблице StatementStrings (Master-detail)
        private void StatementDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            statement = (DataBase.Statements)StatementDataGrid.SelectedItem;
            SignButtonContent();
            UpdateStatementStringGrid(null);
        }

        //Обновление данных таблицы Statements
        public void UpdateGrid(DataBase.Statements statement)
        {
            if ((statement == null) && (StatementViewModel.Source != null))
            {
                statement = (DataBase.Statements)StatementDataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.Statements> statements = new ObservableCollection<DataBase.Statements>(SourceCore.getBase().Statements.ToList());
            StatementViewModel.Source = statements;
            StatementDataGrid.ItemsSource = StatementViewModel.View;
            if (statements.Count > 0)
            {
                StatementDataGrid.SelectedItem = statement;
                if (StatementDataGrid.SelectedIndex < 0)
                {
                    StatementDataGrid.SelectedIndex = 0;
                }
            }
        }

        //Обновление данных таблицы StatementStrings
        public void UpdateStatementStringGrid(DataBase.StatementStrings statementString)
        {
            if ((statementString == null) && (StatementStringViewModel.Source != null))
            {
                statementString = (DataBase.StatementStrings)StatementStringDataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.StatementStrings> statementStrings = new ObservableCollection<DataBase.StatementStrings>();
            if (statement != null)
            {
                statementStrings = new ObservableCollection<DataBase.StatementStrings>(SourceCore.getBase().StatementStrings.Where(s => s.idStat == statement.idStat).ToList());
            }
            StatementStringViewModel.Source = statementStrings;
            StatementStringDataGrid.ItemsSource = StatementStringViewModel.View;
            if (statementStrings.Count > 0)
            {
                StatementStringDataGrid.SelectedItem = statementString;
                if (StatementStringDataGrid.SelectedIndex < 0)
                {
                    StatementStringDataGrid.SelectedIndex = 0;
                }
            }
        }
    }
}
