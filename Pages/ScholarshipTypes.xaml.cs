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
    /// Логика взаимодействия для ScholarshipTypes.xaml
    /// </summary>
    public partial class ScholarshipTypes : Page
    {
        private CollectionViewSource viewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу

        //Конструктор
        public ScholarshipTypes()
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
            Frame.Navigate(new Pages.ScholarshipTypeDlg(this, null, true));
        }

        //Событие кнопки копирования записи в базе данных
        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                int idST = (DataGrid.SelectedItem as DataBase.ScholarshipTypes).idST;
                DataBase.ScholarshipTypes type = SourceCore.getBase().ScholarshipTypes.AsNoTracking().Where(t => t.idST == idST).FirstOrDefault();
                Frame.Navigate(new Pages.ScholarshipTypeDlg(this, type, true));
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
            int idST = (DataGrid.SelectedItem as DataBase.ScholarshipTypes).idST;
            DataBase.ScholarshipTypes type = SourceCore.getBase().ScholarshipTypes.Where(t => t.idST == idST).FirstOrDefault();
            Frame.Navigate(new Pages.ScholarshipTypeDlg(this, type, false));
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
                    DataBase.ScholarshipTypes delitingType = (DataBase.ScholarshipTypes)DataGrid.SelectedItem;
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
                        DataBase.ScholarshipTypes selectingType = (DataBase.ScholarshipTypes)DataGrid.SelectedItem;
                        DataGrid.SelectedItem = delitingType;
                        SourceCore.getBase().ScholarshipTypes.Remove(delitingType);
                        SourceCore.getBase().SaveChanges();
                        UpdateGrid(selectingType);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(delitingType).Reload();
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
            for (int i = 0; i < 2; i++)
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
                    DataGrid.ItemsSource = SourceCore.getBase().ScholarshipTypes.Where(t => t.stName.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    DataGrid.ItemsSource = SourceCore.getBase().ScholarshipTypes.Where(t => t.cost.ToString().Contains(textbox.Text)).ToList();
                    break;
            }
        }

        //Обновление данных таблицы
        public void UpdateGrid(DataBase.ScholarshipTypes type)
        {
            if ((type == null) && (viewModel.Source != null))
            {
                type = (DataBase.ScholarshipTypes)DataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.ScholarshipTypes> types = new ObservableCollection<DataBase.ScholarshipTypes>(SourceCore.getBase().ScholarshipTypes.ToList());
            viewModel.Source = types;
            DataGrid.ItemsSource = viewModel.View;
            if (types.Count > 0)
            {
                DataGrid.SelectedItem = type;
                if (DataGrid.SelectedIndex < 0)
                {
                    DataGrid.SelectedIndex = 0;
                }
            }
        }
    }
}
