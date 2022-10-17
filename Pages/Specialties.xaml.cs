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
    /// Логика взаимодействия для Specialties.xaml
    /// </summary>
    public partial class Specialties : Page
    {
        private CollectionViewSource viewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу

        //Конструктор
        public Specialties()
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
            Frame.Navigate(new Pages.SpecialtyDlg(this, null, true));
        }

        //Событие кнопки копирования записи в базе данных
        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                int idSpec = (DataGrid.SelectedItem as DataBase.Specialties).idSpec;
                DataBase.Specialties specialty = SourceCore.getBase().Specialties.AsNoTracking().Where(s => s.idSpec == idSpec).FirstOrDefault();
                Frame.Navigate(new Pages.SpecialtyDlg(this, specialty, true));
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
            int idSpec = (DataGrid.SelectedItem as DataBase.Specialties).idSpec;
            DataBase.Specialties speciality = SourceCore.getBase().Specialties.Where(s => s.idSpec == idSpec).FirstOrDefault();
            Frame.Navigate(new Pages.SpecialtyDlg(this, speciality, false));
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
                    DataBase.Specialties delitingSpecialty = (DataBase.Specialties)DataGrid.SelectedItem;
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
                        DataBase.Specialties selectingSpecialty = (DataBase.Specialties)DataGrid.SelectedItem;
                        DataGrid.SelectedItem = delitingSpecialty;
                        SourceCore.getBase().Specialties.Remove(delitingSpecialty);
                        SourceCore.getBase().SaveChanges();
                        UpdateGrid(selectingSpecialty);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(delitingSpecialty).Reload();
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
            for (int i = 0; i < 1; i++)
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
                    DataGrid.ItemsSource = SourceCore.getBase().Specialties.Where(s => s.specName.Contains(textbox.Text)).ToList();
                    break;
            }
        }

        //Обновление данных таблицы
        public void UpdateGrid(DataBase.Specialties specialty)
        {
            if ((specialty == null) && (viewModel.Source != null))
            {
                specialty = (DataBase.Specialties)DataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.Specialties> specialties = new ObservableCollection<DataBase.Specialties>(SourceCore.getBase().Specialties.ToList());
            viewModel.Source = specialties;
            DataGrid.ItemsSource = viewModel.View;
            if (specialties.Count > 0)
            {
                DataGrid.SelectedItem = specialty;
                if (DataGrid.SelectedIndex < 0)
                {
                    DataGrid.SelectedIndex = 0;
                }
            }
        }
    }
}
