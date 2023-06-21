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
using System.Windows.Shapes;
using TaskLog.Entities;

namespace TaskLog
{
    /// <summary>
    /// Логика взаимодействия для TasksWindow.xaml
    /// </summary>
    public partial class TasksWindow : Window
    {
        public TasksWindow()
        {
            InitializeComponent();
            FillDataMainGrid();
        }

        public void FillDataMainGrid()
        {
            MainDataGrid.ItemsSource = DbUtils.db.Tasks.Select(p => new
            {
                p.TaskId,
                p.CompId,
                p.TaskDescr,
                p.CompSn,
                p.UserId,
                p.Comments
            }).ToList();
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
