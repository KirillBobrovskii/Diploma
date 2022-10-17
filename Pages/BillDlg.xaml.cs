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
    /// Логика взаимодействия для BillDlg.xaml
    /// </summary>
    public partial class BillDlg : Page
    {
        private Page page; //Хранение контекста страницы Bills

        private bool dlgMode; //Режим обработки данных (добавление, копирование изменение)

        private DataBase.Bills bill { get; set; } //Хранение данных о выбранной записи из таблицы Bills

        //Конструктор
        public BillDlg(Page page, DataBase.Bills bill, bool dlgMode)
        {
            InitializeComponent();
            DataContext = this;
            this.page = page;
            this.bill = bill;
            this.dlgMode = dlgMode;
            BillStudentFIO.ItemsSource = SourceCore.getBase().Students.ToList();
            if (dlgMode)
            {
                if (bill == null)
                {
                    AddBill();
                }
                else
                {
                    CopyBill();
                }
            }
            else
            {
                EditBill();
            }
        }


        //Добавление записи в базу данных
        private void AddBill()
        {
            Label.Content = "Добавить карту";
            Commit.Content = "Добавить";
        }

        //Установка данных из выбранной записи в поля данных
        private void SelectBill()
        {
            Bill.Text = bill.bill;
            BillFinishDate.SelectedDate = bill.finishDate;
        }

        //Копирование записи в базе данных
        private void CopyBill()
        {
            Label.Content = "Копировать карту";
            Commit.Content = "Копировать";
            SelectBill();
            BillStudentFIO.SelectedIndex = (int)bill.idStud - 1;
        }

        //Изменение записи в базе данных
        private void EditBill()
        {
            Label.Content = "Изменить карту";
            Commit.Content = "Изменить";
            SelectBill();
            BillStudentFIO.SelectedItem = bill.Students;
        }

        //Проверка введенных пользователем данных в поля данных
        private bool CheckInfo()
        {
            if (BillStudentFIO.SelectedItem == null)
            {
                MessageBox.Show("Выберите студента!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (Bill.Text.Length != 16)
            {
                MessageBox.Show("Введите номер карты студента (16 цифр)!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (BillFinishDate.SelectedDate == null)
            {
                MessageBox.Show("Введите год окончания действия карты!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    DataBase.Bills newBill = new DataBase.Bills();
                    newBill.Students = (DataBase.Students)BillStudentFIO.SelectedItem;
                    newBill.bill = Bill.Text;
                    newBill.finishDate = BillFinishDate.SelectedDate;
                    SourceCore.getBase().Bills.Add(newBill);
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Bills).UpdateGrid(newBill);
                }
                else
                {
                    DataBase.Bills updateBill = SourceCore.getBase().Bills.Where(b => b.idBill == bill.idBill).FirstOrDefault();
                    updateBill.Students = (DataBase.Students)BillStudentFIO.SelectedItem;
                    updateBill.bill = Bill.Text;
                    updateBill.finishDate = BillFinishDate.SelectedDate;
                    SourceCore.getBase().SaveChanges();
                    (page as Pages.Bills).UpdateGrid(updateBill);
                }
                (page as Pages.Bills).DataGrid.Focus();
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
            (page as Pages.Bills).DlgLoad(false);
        }
    }
}
