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

namespace ScholarshipAppointment
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Page> activePages;

        private int currentPageIndex;

        public MainWindow()
        {
            InitializeComponent();
            activePages = new List<Page>();
            currentPageIndex = -1;
        }

        private int GetActivePageIndexByType(Type pageType)
        {
            int index = activePages.Count - 1;
            while ((index >= 0) && (activePages[index].GetType() != pageType))
            {
                index--;
            }
            return index;
        }

        private void ShowPage(Type pageType)
        {
            Page page;
            if (pageType != null)
            {
                int index = GetActivePageIndexByType(pageType);
                if (index < 0)
                {
                    page = (Page)Activator.CreateInstance(pageType);
                    activePages.Add(page);
                    currentPageIndex = activePages.Count - 1;
                }
                else
                {
                    page = activePages[index];
                    currentPageIndex = index;
                }
            }
            else
            {
                page = null;
            }
            Frame.Navigate(page);
        }

        private void FrameLoadCompleted(object sender, NavigationEventArgs e)
        {
            while (Frame.CanGoBack)
            {
                Frame.RemoveBackEntry();
            }
        }

        private void ClosePageButtonClick(object sender, RoutedEventArgs e)
        {
            Page page;
            if (activePages.Count > 0)
            {
                activePages.RemoveAt(currentPageIndex);
                if (currentPageIndex > 0)
                {
                    currentPageIndex--;
                    page = activePages[currentPageIndex];
                }
                else
                {
                    if (currentPageIndex < activePages.Count)
                    {
                        page = activePages[currentPageIndex];
                    }
                    else
                    {
                        page = null;
                    }
                }
                Frame.Navigate(page);
            }
        }

        private void StudentsButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.Students));
        }

        private void GroupsButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.Groups));
        }

        private void SpecialtyButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.Specialties));
        }

        private void ScholarshipTypesButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.ScholarshipTypes));
        }

        private void OrderTypesButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.OrderTypes));
        }

        private void BillsButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.Bills));
        }

        private void UsersButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.Users));
        }

        private void OrdersButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.Orders));
        }

        private void StatementsButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.Statements));
        }

        private void PreviosPageButtonClick(object sender, RoutedEventArgs e)
        {
            currentPageIndex--;
            if (currentPageIndex < 0)
            {
                currentPageIndex = activePages.Count - 1;
            }
            if (activePages.Count > 0)
            {
                Frame.Navigate(activePages[currentPageIndex]);
            }
        }

        private void NextPageButtonClick(object sender, RoutedEventArgs e)
        {
            currentPageIndex++;
            if (currentPageIndex > activePages.Count - 1)
            {
                currentPageIndex = 0;
            }
            if (activePages.Count > 0)
            {
                Frame.Navigate(activePages[currentPageIndex]);
            }
        }
    }
}
