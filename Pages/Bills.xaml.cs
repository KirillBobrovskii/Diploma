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
    /// Логика взаимодействия для Bills.xaml
    /// </summary>
    public partial class Bills : Page
    {
        private CollectionViewSource viewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу

        //Конструктор
        public Bills()
        {
            InitializeComponent();
            DataContext = this;
            viewModel = new CollectionViewSource();
            UpdateGrid(null);
        }

        //Открытие и закрытие диалоговой секции
        public void DlgLoad(bool flag)
        {
            if (flag)
            {
                GridSplitter.Width = new GridLength(3);
                Dialog.Width = new GridLength(500);
                DataGrid.IsHitTestVisible = false;
                ButtonsPanel.IsEnabled = false;
            }
            else
            {
                GridSplitter.Width = new GridLength(0);
                Dialog.Width = new GridLength(0);
                DataGrid.IsHitTestVisible = true;
                ButtonsPanel.IsEnabled = true;
            }
        }

        //Событие кнопки добавления записи в базу данных
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            DlgLoad(true);
            Frame.Navigate(new Pages.BillDlg(this, null, true));
        }

        //Событие кнопки копирования записи в базе данных
        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                int idBill = (DataGrid.SelectedItem as DataBase.Bills).idBill;
                DataBase.Bills bill = SourceCore.getBase().Bills.AsNoTracking().Where(b => b.idBill == idBill).FirstOrDefault();
                Frame.Navigate(new Pages.BillDlg(this, bill, true));
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Изменение записи в базе данных
        private void Edit()
        {
            DlgLoad(true);
            int idBill = (DataGrid.SelectedItem as DataBase.Bills).idBill;
            DataBase.Bills bill = SourceCore.getBase().Bills.Where(b => b.idBill == idBill).FirstOrDefault();
            Frame.Navigate(new Pages.BillDlg(this, bill, false));
        }

        //Событие кнопки изменения записи в базе данных
        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                Edit();
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Событие изменения записи в базе данных по двойному клику мыши
        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edit();
        }

        //Событие кнопки удаления записи из базы данных
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись!", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    DataBase.Bills delitingBill = (DataBase.Bills)DataGrid.SelectedItem;
                    try
                    {
                        if (DataGrid.SelectedIndex < DataGrid.Items.Count - 1)
                        {
                            DataGrid.SelectedIndex++;
                        }
                        else
                        {
                            if (DataGrid.SelectedIndex > 0)
                            {
                                DataGrid.SelectedIndex--;
                            }
                        }
                        DataBase.Bills selectingBill = (DataBase.Bills)DataGrid.SelectedItem;
                        DataGrid.SelectedItem = delitingBill;
                        SourceCore.getBase().Bills.Remove(delitingBill);
                        SourceCore.getBase().SaveChanges();
                        UpdateGrid(selectingBill);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(delitingBill).Reload();
                        MessageBox.Show("Невозможно удалить запись, так как она используется в других справочниках базы данных!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    DataGrid.Focus();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Загрузка заголовков таблицы в combobox для фильтрации
        private void PageLoad(object sender, RoutedEventArgs e)
        {
            List<String> Columns = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                Columns.Add(DataGrid.Columns[i].Header.ToString());
            }
            FilterBox.ItemsSource = Columns;
            FilterBox.SelectedIndex = 0;
            foreach (DataGridColumn Column in DataGrid.Columns)
            {
                Column.CanUserSort = false;
            }
        }

        //Фильтрация данных таблицы
        private void FilterTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            switch (FilterBox.SelectedIndex)
            {
                case 0:
                    DataGrid.ItemsSource = SourceCore.getBase().Bills.Where(b => b.Students.fio.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    DataGrid.ItemsSource = SourceCore.getBase().Bills.Where(b => b.bill.Contains(textbox.Text)).ToList();
                    break;
                case 2:
                    {
                        List<DataBase.Bills> list = new List<DataBase.Bills>();
                        foreach (DataBase.Bills b in SourceCore.getBase().Bills)
                        {
                            if (b.finishDate.Value.ToShortDateString().Contains(textbox.Text))
                            {
                                list.Add(b);
                            }
                        }
                        DataGrid.ItemsSource = list;
                    }
                    break;
            }
        }

        //Обновление данных таблицы
        public void UpdateGrid(DataBase.Bills bill)
        {
            if ((bill == null) && (viewModel.Source != null))
            {
                bill = (DataBase.Bills)DataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.Bills> bills = new ObservableCollection<DataBase.Bills>(SourceCore.getBase().Bills.ToList());
            viewModel.Source = bills;
            DataGrid.ItemsSource = viewModel.View;
            if (bills.Count > 0)
            {
                DataGrid.SelectedItem = bill;
                if (DataGrid.SelectedIndex < 0)
                {
                    DataGrid.SelectedIndex = 0;
                }
            }
        }
    }
}
