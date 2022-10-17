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
using Word = Microsoft.Office.Interop.Word;
using System.IO;

namespace ScholarshipAppointment.Pages
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        private CollectionViewSource OrderViewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу Orders

        private CollectionViewSource OrderStringViewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу OrderStrings

        private DataBase.Orders order { get; set; } //Хранение данных о выбранной записи из таблицы Orders

        //Конструктор
        public Orders()
        {
            InitializeComponent();
            DataContext = this;
            OrderViewModel = new CollectionViewSource();
            OrderStringViewModel = new CollectionViewSource();
            UpdateOrderGrid(null);
        }

        //Открытие и закрытие диалоговой секции
        public void DlgLoad(bool flag)
        {
            if (flag)
            {
                GridSplitter.Width = new GridLength(3);
                Dialog.Width = new GridLength(500);
                OrderDataGrid.IsHitTestVisible = false;
                OrderStringDataGrid.IsHitTestVisible = false;
                OrderButtonsPanel.IsEnabled = false;
                OrderStringButtonsPanel.IsEnabled = false;
                OrderStringFilterPanel.IsEnabled = false;
            }
            else
            {
                GridSplitter.Width = new GridLength(0);
                Dialog.Width = new GridLength(0);
                OrderDataGrid.IsHitTestVisible = true;
                OrderStringDataGrid.IsHitTestVisible = true;
                OrderButtonsPanel.IsEnabled = true;
                OrderStringButtonsPanel.IsEnabled = true;
                OrderStringFilterPanel.IsEnabled = true;
            }
        }

        //Получене выбранной записи таблицы Orders
        private DataBase.Orders SelectOrder()
        {
            int idOrder = (OrderDataGrid.SelectedItem as DataBase.Orders).idOrder;
            return SourceCore.getBase().Orders.Where(o => o.idOrder == idOrder).FirstOrDefault();
        }

        //Событие кнопки добавления записи в базу данных в таблицу Orders
        private void OrderAddButtonClick(object sender, RoutedEventArgs e)
        {
            DlgLoad(true);
            Frame.Navigate(new Pages.OrderDlg(this, null, true));
        }

        //Событие кнопки добавления записи в базу данных в таблицу OrderStrings
        private void OrderStringAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                Frame.Navigate(new Pages.OrderStringDlg(this, null, SelectOrder(), true));
            }
            else
            {
                MessageBox.Show("Выберите приказ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие кнопки копирования записи в базе данных в таблице Orders
        private void OrderCopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                int idOrder = (OrderDataGrid.SelectedItem as DataBase.Orders).idOrder;
                DataBase.Orders order = SourceCore.getBase().Orders.AsNoTracking().Where(o => o.idOrder == idOrder).FirstOrDefault();
                Frame.Navigate(new Pages.OrderDlg(this, order, true));
            }
            else
            {
                MessageBox.Show("Выберите приказ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие кнопки копирования записи в базе данных в таблице OrderStrings
        private void OrderStringCopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                if (OrderStringDataGrid.SelectedItem != null)
                {
                    DlgLoad(true);
                    int idOS = (OrderStringDataGrid.SelectedItem as DataBase.OrderStrings).idOS;
                    DataBase.OrderStrings orderString = SourceCore.getBase().OrderStrings.AsNoTracking().Where(o => o.idOS == idOS).FirstOrDefault();
                    Frame.Navigate(new Pages.OrderStringDlg(this, orderString, SelectOrder(), true));
                }
                else
                {
                    MessageBox.Show("Выберите строку приказа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите приказ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие кнопки изменения записи в базе данных в таблице Orders
        private void OrderEditButtonClick(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                Frame.Navigate(new Pages.OrderDlg(this, SelectOrder(), false));
            }
            else
            {
                MessageBox.Show("Выберите приказ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие изменения записи в базе данных в таблице Orders по двойному клику мыши
        private void OrderDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectOrder().signDate == null)
            {
                DlgLoad(true);
                Frame.Navigate(new Pages.OrderDlg(this, SelectOrder(), false));
            }
        }

        //Изменение записи в базе данных в таблице OrderStrings
        private void OrderStringEdit()
        {
            DlgLoad(true);
            int idOS = (OrderStringDataGrid.SelectedItem as DataBase.OrderStrings).idOS;
            DataBase.OrderStrings orderString = SourceCore.getBase().OrderStrings.Where(o => o.idOS == idOS).FirstOrDefault();
            Frame.Navigate(new Pages.OrderStringDlg(this, orderString, SelectOrder(), false));
        }

        //Событие изменения записи в базе данных в таблице OrderStrings
        private void OrderStringEditButtonClick(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                if (OrderStringDataGrid.SelectedItem != null)
                {
                    OrderStringEdit();
                }
                else
                {
                    MessageBox.Show("Выберите строку приказа!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите приказ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие изменения записи в базе данных в таблице Orders по двойному клику мыши
        private void OrderStringDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectOrder().signDate == null)
            {
                OrderStringEdit();
            }
        }

        //Событие кнопки удаления записи из базы данных из таблицы Orders
        private void OrderDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить приказ!", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    DataBase.Orders delitingOrder = (DataBase.Orders)OrderDataGrid.SelectedItem;
                    try
                    {
                        if (OrderDataGrid.SelectedIndex < OrderDataGrid.Items.Count - 1)
                        {
                            OrderDataGrid.SelectedIndex++;
                        }
                        else
                        {
                            if (OrderDataGrid.SelectedIndex > 0)
                            {
                                OrderDataGrid.SelectedIndex--;
                            }
                        }
                        DataBase.Orders selectingOrder = (DataBase.Orders)OrderDataGrid.SelectedItem;
                        OrderDataGrid.SelectedItem = delitingOrder;
                        SourceCore.getBase().Orders.Remove(delitingOrder);
                        SourceCore.getBase().SaveChanges();
                        UpdateOrderGrid(selectingOrder);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(delitingOrder).Reload();
                        MessageBox.Show("Невозможно удалить приказ, так как он используется в других справочниках базы данных!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    OrderDataGrid.Focus();
                }
            }
            else
            {
                MessageBox.Show("Выберите приказ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие кнопки удаления записи из базы данных из таблицы OrderStrings
        private void OrderStringDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            order = (DataBase.Orders)OrderDataGrid.SelectedItem;
            if (OrderStringDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить строку приказа!", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    DataBase.OrderStrings delitingOrderString = (DataBase.OrderStrings)OrderStringDataGrid.SelectedItem;
                    try
                    {
                        if (OrderStringDataGrid.SelectedIndex < OrderStringDataGrid.Items.Count - 1)
                        {
                            OrderStringDataGrid.SelectedIndex++;
                        }
                        else
                        {
                            if (OrderStringDataGrid.SelectedIndex > 0)
                            {
                                OrderStringDataGrid.SelectedIndex--;
                            }
                        }
                        DataBase.OrderStrings selectingOrderString = (DataBase.OrderStrings)OrderStringDataGrid.SelectedItem;
                        OrderStringDataGrid.SelectedItem = delitingOrderString;
                        SourceCore.getBase().OrderStrings.Remove(delitingOrderString);
                        SourceCore.getBase().SaveChanges();
                        UpdateOrderStringGrid(selectingOrderString);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(order).Reload();
                        MessageBox.Show("Невозможно удалить строку приказа, так как она используется в других справочниках базы данных!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    OrderStringDataGrid.Focus();
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
            for (int i = 0; i < 7; i++)
            {
                Columns.Add(OrderDataGrid.Columns[i].Header.ToString());
            }
            OrderFilterBox.ItemsSource = Columns;
            OrderFilterBox.SelectedIndex = 0;
            foreach (DataGridColumn Column in OrderDataGrid.Columns)
            {
                Column.CanUserSort = false;
            }
            Columns = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                Columns.Add(OrderStringDataGrid.Columns[i].Header.ToString());
            }
            OrderStringFilterBox.ItemsSource = Columns;
            OrderStringFilterBox.SelectedIndex = 0;
            foreach (DataGridColumn Column in OrderStringDataGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }

        //Фильтрация данных таблицы Orders
        private void OrderFilterTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            List<DataBase.Orders> list = new List<DataBase.Orders>();
            switch (OrderFilterBox.SelectedIndex)
            {
                case 0:
                    OrderDataGrid.ItemsSource = SourceCore.getBase().Orders.Where(o => o.idOrder.ToString().Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    OrderDataGrid.ItemsSource = SourceCore.getBase().Orders.Where(o => o.ScholarshipTypes.stName.Contains(textbox.Text)).ToList();
                    break;
                case 2:
                    OrderDataGrid.ItemsSource = SourceCore.getBase().Orders.Where(o => o.OrderTypes.otName.Contains(textbox.Text)).ToList();
                    break;
                case 3:
                    foreach (DataBase.Orders o in SourceCore.getBase().Orders)
                    {
                        if (o.docDate.Value.ToShortDateString().Contains(textbox.Text))
                        {
                            list.Add(o);
                        }
                    }
                    OrderDataGrid.ItemsSource = list;
                    break;
                case 4:
                    OrderDataGrid.ItemsSource = SourceCore.getBase().Orders.Where(o => o.Users.fio.Contains(textbox.Text)).ToList();
                    break;
                case 5:
                    foreach (DataBase.Orders o in SourceCore.getBase().Orders)
                    {
                        if (o.signDate != null)
                        {
                            if (o.signDate.Value.ToShortDateString().Contains(textbox.Text))
                            {
                                list.Add(o);
                            }
                        }
                    }
                    OrderDataGrid.ItemsSource = list;
                    break;
                case 6:
                    OrderDataGrid.ItemsSource = SourceCore.getBase().Orders.Where(o => o.comm.Contains(textbox.Text)).ToList();
                    break;
            }
        }

        //События выбора записи из таблицы Orders при изменении типа фильтрации для того,
        //чтобы осуществить фильтрацию в таблице OrderStrings только по тем записям,
        //которые связаны с выбранной записью из таблицы Orders
        private void OrderStringFilterBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            order = (DataBase.Orders)OrderDataGrid.SelectedItem;
        }

        //Фильтрация данных таблицы OrderStrings
        private void OrderStringFilterTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            List<DataBase.OrderStrings> list = new List<DataBase.OrderStrings>();
            switch (OrderStringFilterBox.SelectedIndex)
            {
                case 0:
                    OrderStringDataGrid.ItemsSource = SourceCore.getBase().OrderStrings.Where(o => o.Students.fio.Contains(textbox.Text) && o.idOrder == order.idOrder).ToList();
                    break;
                case 1:
                    OrderStringDataGrid.ItemsSource = SourceCore.getBase().OrderStrings.Where(o => o.Students.Groups.gName.Contains(textbox.Text) && o.idOrder == order.idOrder).ToList();
                    break;
                case 2:
                    OrderStringDataGrid.ItemsSource = SourceCore.getBase().OrderStrings.Where(o => o.cost.ToString().Contains(textbox.Text) && o.idOrder == order.idOrder).ToList();
                    break;
                case 3:
                    foreach (DataBase.OrderStrings o in SourceCore.getBase().OrderStrings)
                    {
                        if ((o.startDate.Value.ToShortDateString().Contains(textbox.Text)) && (o.idOrder == order.idOrder))
                        {
                            list.Add(o);
                        }
                    }
                    OrderStringDataGrid.ItemsSource = list;
                    break;
                case 4:
                    foreach (DataBase.OrderStrings o in SourceCore.getBase().OrderStrings)
                    {
                        if ((o.finishDate.Value.ToShortDateString().Contains(textbox.Text)) && (o.idOrder == order.idOrder))
                        {
                            list.Add(o);
                        }
                    }
                    OrderStringDataGrid.ItemsSource = list;
                    break;
            }
        }

        //Кнопка подписи приказа
        private void SignButtonContent()
        {
            if (order != null)
            {
                if (order.signDate != null)
                {
                    OrderEditButton.IsEnabled = false;
                    OrderDeleteButton.IsEnabled = false;
                    OrderStringButtonsPanel.IsEnabled = false;
                    SignButton.Content = "Расподписать";
                }
                else
                {
                    OrderEditButton.IsEnabled = true;
                    OrderDeleteButton.IsEnabled = true;
                    OrderStringButtonsPanel.IsEnabled = true;
                    SignButton.Content = "Подписать";
                }
            }
        }

        //Событие кнопки подписи приказа
        private void SignButtonClick(object sender, RoutedEventArgs e)
        {
            if (order.signDate == null)
            {
                order.signDate = DateTime.Now;
            }
            else
            {
                if (SourceCore.getBase().StatementStrings.Count(s => s.OrderStrings.Orders.idOrder == order.idOrder) < 1)
                {
                    order.signDate = null;
                }
                else
                {
                    MessageBox.Show("Расподпись приказа невозможна т.к. его строки присутствуют в ведомости!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            SourceCore.getBase().SaveChanges();
            SignButtonContent();
            UpdateOrderGrid(null);
        }

        //Событие выбора записи из таблицы Orders и последующее обновления данных в таблице OrderStrings (Master-detail)
        private void OrderDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            order = (DataBase.Orders)OrderDataGrid.SelectedItem;
            SignButtonContent();
            UpdateOrderStringGrid(null);
        }

        //Обновление данных таблицы Orders
        public void UpdateOrderGrid(DataBase.Orders order)
        {
            if ((order == null) && (OrderViewModel.Source != null))
            {
                order = (DataBase.Orders)OrderDataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.Orders> orders = new ObservableCollection<DataBase.Orders>(SourceCore.getBase().Orders.ToList());
            OrderViewModel.Source = orders;
            OrderDataGrid.ItemsSource = OrderViewModel.View;
            if (orders.Count > 0)
            {
                OrderDataGrid.SelectedItem = order;
                if (OrderDataGrid.SelectedIndex < 0)
                {
                    OrderDataGrid.SelectedIndex = 0;
                }
            }
        }

        //Обновление данных таблицы OrderStrings
        public void UpdateOrderStringGrid(DataBase.OrderStrings orderString)
        {
            if ((orderString == null) && (OrderStringViewModel.Source != null))
            {
                orderString = (DataBase.OrderStrings)OrderStringDataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.OrderStrings> orderStrings = new ObservableCollection<DataBase.OrderStrings>();
            if (order != null)
            {
                orderStrings = new ObservableCollection<DataBase.OrderStrings>(SourceCore.getBase().OrderStrings.Where(o => o.idOrder == order.idOrder).ToList());
            }
            OrderStringViewModel.Source = orderStrings;
            OrderStringDataGrid.ItemsSource = OrderStringViewModel.View;
            if (orderStrings.Count > 0)
            {
                OrderStringDataGrid.SelectedItem = orderString;
                if (OrderStringDataGrid.SelectedIndex < 0)
                {
                    OrderStringDataGrid.SelectedIndex = 0;
                }
            }
        }

        //Формирование Word-документа из таблицы OrderStrings
        public void WordExportButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //Создание документа
                Type wordType = Type.GetTypeFromProgID("Word.Application");
                dynamic wordApp = Activator.CreateInstance(wordType);
                wordApp.Visible = true;
                dynamic wordDoc = null;
                Object template = (Environment.CurrentDirectory + "\\Docs\\Temp.docx");
                wordDoc = wordApp.Documents.Add(ref template);
                //Замена меток данными
                var items = new Dictionary<string, string>
                {
                    { "%docNum%", order.idOrder.ToString() },
                    { "%docDate%", order.docDate.Value.ToString("dd.MM.yyyy") },
                    { "%stName%", order.ScholarshipTypes.stName }
                };
                foreach (var item in items)
                {
                    Word.Find find = wordApp.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;
                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;
                    find.Execute(FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: Type.Missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: Type.Missing, Replace: replace);
                }
                //Создание и заполнение таблицы
                wordApp.Selection.Find.Execute("%Table%");
                Word.Range wordRange = wordApp.Selection.Range;
                int row = OrderStringDataGrid.Items.Count + 2;
                int col = OrderStringDataGrid.Columns.Count;
                Word.Table wordTable = wordDoc.Tables.Add(wordRange, row, col);
                wordTable.set_Style("Сетка таблицы");
                wordTable.ApplyStyleHeadingRows = true;
                wordTable.ApplyStyleLastRow = false;
                wordTable.ApplyStyleFirstColumn = true;
                wordTable.ApplyStyleLastColumn = false;
                wordTable.ApplyStyleRowBands = true;
                wordTable.ApplyStyleColumnBands = false;
                wordTable.Cell(1, 1).Range.Text = "Фамилия, Имя, Отчество";
                wordTable.Cell(1, 2).Range.Text = "Группа";
                wordTable.Cell(1, 3).Range.Text = "Сумма (руб.)";
                wordTable.Cell(1, 4).Range.Text = "Дата начала действия";
                wordTable.Cell(1, 5).Range.Text = "Дата конца действия";
                wordTable.Columns[1].Width = 190;
                wordTable.Columns[2].Width = 80;
                wordTable.Columns[3].Width = 70;
                wordTable.Columns[4].Width = 70;
                wordTable.Columns[5].Width = 70;
                List<DataBase.OrderStrings> orderStrings = SourceCore.getBase().OrderStrings.Where(o => o.idOrder == order.idOrder).ToList();
                decimal sum = 0;
                for (var i = 0; i < row - 2; i++)
                {
                    wordTable.Cell(i + 2, 1).Range.Text = orderStrings[i].Students.fio;
                    wordTable.Cell(i + 2, 2).Range.Text = orderStrings[i].Students.Groups.gName;
                    wordTable.Cell(i + 2, 3).Range.Text = orderStrings[i].cost.ToString();
                    wordTable.Cell(i + 2, 4).Range.Text = orderStrings[i].startDate.Value.ToString("dd.MM.yyyy");
                    wordTable.Cell(i + 2, 5).Range.Text = orderStrings[i].finishDate.Value.ToString("dd.MM.yyyy");
                    sum += (decimal)orderStrings[i].cost;
                }
                wordTable.Cell(row, 2).Range.Text = "Общая сумма";
                wordTable.Cell(row, 2).Range.Font.Bold = 3;
                wordTable.Cell(row, 3).Range.Text = sum.ToString();
            }
            catch
            {
                MessageBox.Show("Не удалось создать документ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
