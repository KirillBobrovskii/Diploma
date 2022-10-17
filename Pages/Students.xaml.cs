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
    /// Логика взаимодействия для Students.xaml
    /// </summary>
    public partial class Students : Page
    {
        private CollectionViewSource viewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу
        
        //Конструктор
        public Students()
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
            Frame.Navigate(new Pages.StudentDlg(this, null, true));
        }

        //Событие кнопки копирования записи в базе данных
        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                DlgLoad(true);
                int idStud = (DataGrid.SelectedItem as DataBase.Students).idStud;
                DataBase.Students student = SourceCore.getBase().Students.AsNoTracking().Where(s => s.idStud == idStud).FirstOrDefault();
                Frame.Navigate(new Pages.StudentDlg(this, student, true));
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
            int idStud = (DataGrid.SelectedItem as DataBase.Students).idStud;
            DataBase.Students student = SourceCore.getBase().Students.Where(s => s.idStud == idStud).FirstOrDefault();
            Frame.Navigate(new Pages.StudentDlg(this, student, false));
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
                    DataBase.Students delitingStudent = (DataBase.Students)DataGrid.SelectedItem;
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
                        DataBase.Students selectingStudent = (DataBase.Students)DataGrid.SelectedItem;
                        DataGrid.SelectedItem = delitingStudent;
                        SourceCore.getBase().Students.Remove(delitingStudent);
                        SourceCore.getBase().SaveChanges();
                        UpdateGrid(selectingStudent);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(delitingStudent).Reload();
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
            for (int i = 0; i < 4; i++)
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
                    DataGrid.ItemsSource = SourceCore.getBase().Students.Where(s => s.fio.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    {
                        List<DataBase.Students> list = new List<DataBase.Students>();
                        foreach (DataBase.Students s in SourceCore.getBase().Students)
                        {
                            if (s.birthDay.Value.ToShortDateString().Contains(textbox.Text))
                            {
                                list.Add(s);
                            }
                        }
                        DataGrid.ItemsSource = list;
                    }
                    break;
                case 2:
                    DataGrid.ItemsSource = SourceCore.getBase().Students.Where(s => s.passInfo.Contains(textbox.Text)).ToList();
                    break;
                case 3:
                    DataGrid.ItemsSource = SourceCore.getBase().Students.Where(s => s.Groups.gName.Contains(textbox.Text)).ToList();
                    break;
            }
        }

        //Обновление данных таблицы
        public void UpdateGrid(DataBase.Students student)
        {
            if ((student == null) && (viewModel.Source != null))
            {
                student = (DataBase.Students)DataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.Students> students = new ObservableCollection<DataBase.Students>(SourceCore.getBase().Students.ToList());
            viewModel.Source = students;
            DataGrid.ItemsSource = viewModel.View;
            if (students.Count > 0)
            {
                DataGrid.SelectedItem = student;
                if (DataGrid.SelectedIndex < 0)
                {
                    DataGrid.SelectedIndex = 0;
                }
            }
        }
    }
}
