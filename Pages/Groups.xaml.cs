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
    /// Логика взаимодействия для Groups.xaml
    /// </summary>
    public partial class Groups : Page
    {
        private CollectionViewSource viewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу

        //Конструктор
        public Groups()
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
            Frame.Navigate(new Pages.GroupDlg(this, null, true));
        }

        //Событие кнопки копирования записи в базе данных
        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                int idGroup = (DataGrid.SelectedItem as DataBase.Groups).idGroup;
                DataBase.Groups group = SourceCore.getBase().Groups.AsNoTracking().Where(g => g.idGroup == idGroup).FirstOrDefault();
                Frame.Navigate(new Pages.GroupDlg(this, group, true));
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
            int idGroup = (DataGrid.SelectedItem as DataBase.Groups).idGroup;
            DataBase.Groups group = SourceCore.getBase().Groups.Where(g => g.idGroup == idGroup).FirstOrDefault();
            Frame.Navigate(new Pages.GroupDlg(this, group, false));
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
                    DataBase.Groups delitingGroup = (DataBase.Groups)DataGrid.SelectedItem;
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
                        DataBase.Groups selectingGroup = (DataBase.Groups)DataGrid.SelectedItem;
                        DataGrid.SelectedItem = delitingGroup;
                        SourceCore.getBase().Groups.Remove(delitingGroup);
                        SourceCore.getBase().SaveChanges();
                        UpdateGrid(selectingGroup);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(delitingGroup).Reload();
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
                    DataGrid.ItemsSource = SourceCore.getBase().Groups.Where(g => g.gName.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    DataGrid.ItemsSource = SourceCore.getBase().Groups.Where(g => g.Specialties.specName.Contains(textbox.Text)).ToList();
                    break;
            }
        }

        //Обновление данных таблицы
        public void UpdateGrid(DataBase.Groups group)
        {
            if ((group == null) && (viewModel.Source != null))
            {
                group = (DataBase.Groups)DataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.Groups> groups = new ObservableCollection<DataBase.Groups>(SourceCore.getBase().Groups.ToList());
            viewModel.Source = groups;
            DataGrid.ItemsSource = viewModel.View;
            if (groups.Count > 0)
            {
                DataGrid.SelectedItem = group;
                if (DataGrid.SelectedIndex < 0)
                {
                    DataGrid.SelectedIndex = 0;
                }
            }
        }
    }
}
