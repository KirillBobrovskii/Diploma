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
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        private CollectionViewSource viewModel { get; set; } //Коллекция для загрузки обновленных данных из базы в таблицу

        //Конструктор
        public Users()
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
            Frame.Navigate(new Pages.UserDlg(this, null));
        }

        //Изменение записи в базе данных
        private void Edit()
        {
            DlgLoad(true);
            int idUser = (DataGrid.SelectedItem as DataBase.Users).idUser;
            DataBase.Users user = SourceCore.getBase().Users.Where(u => u.idUser == idUser).FirstOrDefault();
            Frame.Navigate(new Pages.UserDlg(this, user));
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
            DataBase.Users user = (DataGrid.SelectedItem as DataBase.Users);
            if ((user.idUser != SourceCore.currentUser.idUser) && (SourceCore.currentUser.uAdmin != false))
            {
                Edit();
            }
        }

        //Событие кнопки удаления записи из базы данных
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись!", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    DataBase.Users delitingUser = (DataBase.Users)DataGrid.SelectedItem;
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
                        DataBase.Users selectingUser = (DataBase.Users)DataGrid.SelectedItem;
                        DataGrid.SelectedItem = delitingUser;
                        SourceCore.getBase().Users.Remove(delitingUser);
                        SourceCore.getBase().SaveChanges();
                        UpdateGrid(selectingUser);
                    }
                    catch
                    {
                        SourceCore.getBase().Entry(delitingUser).Reload();
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
                    DataGrid.ItemsSource = SourceCore.getBase().Users.Where(u => u.fio.Contains(textbox.Text)).ToList();
                    break;
                case 1:
                    DataGrid.ItemsSource = SourceCore.getBase().Users.Where(u => u.uLogin.Contains(textbox.Text)).ToList();
                    break;
                case 2:
                    DataGrid.ItemsSource = SourceCore.getBase().Users.Where(u => u.uPassword.Contains(textbox.Text)).ToList();
                    break;
            }
        }

        private void UserDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataBase.Users user = (DataGrid.SelectedItem as DataBase.Users);
            if (user != null)
            {
                if ((user.idUser == SourceCore.currentUser.idUser) || (SourceCore.currentUser.uAdmin == false))
                {
                    UserAddButton.IsEnabled = false;
                    UserEditButton.IsEnabled = false;
                    UserDeleteButton.IsEnabled = false;
                }
                else
                {
                    UserAddButton.IsEnabled = true;
                    UserEditButton.IsEnabled = true;
                    UserDeleteButton.IsEnabled = true;
                }
            }
        }

        //Обновление данных таблицы
        public void UpdateGrid(DataBase.Users user)
        {
            if ((user == null) && (viewModel.Source != null))
            {
                user = (DataBase.Users)DataGrid.SelectedItem;
            }
            ObservableCollection<DataBase.Users> users = new ObservableCollection<DataBase.Users>(SourceCore.getBase().Users.ToList());
            viewModel.Source = users;
            DataGrid.ItemsSource = viewModel.View;
            if (users.Count > 0)
            {
                DataGrid.SelectedItem = user;
                if (DataGrid.SelectedIndex < 0)
                {
                    DataGrid.SelectedIndex = 0;
                }
            }
        }
    }
}
