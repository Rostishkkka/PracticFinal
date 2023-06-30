﻿using System;
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
    /// Interaction logic for UsersViewWindow.xaml
    /// </summary>
    public partial class UsersViewWindow : Window
    {
        public UsersViewWindow()
        {
            InitializeComponent();
            FillDataMainGrid();
        }
        public void FillDataMainGrid() // Функция заполняющая MainDataGrid данными из БД
        {
            MainDataGrid.ItemsSource = DbUtils.db.Users.Select(p => new
            {
               p.UserId,
               p.UserName,
               p.UserEmail
            }).ToList();
        }

        private void FilteringButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearFiltersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            UserAddAndChangeWindow userAddAndChangeWindow = new UserAddAndChangeWindow(null);
            bool? result = userAddAndChangeWindow.ShowDialog();
            if (result == true) 
            {
                FillDataMainGrid();
            }
        }

        private void MainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainDataGrid.SelectedCells.Count() == 0) return;
            var cellInfo = MainDataGrid.SelectedCells[0];
            int content = Convert.ToInt16((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
            var table = DbUtils.db.Users.Where(x => x.UserId == content).FirstOrDefault();
            if (table != null) 
            {
                UserAddAndChangeWindow userAddAndChangeWindow = new UserAddAndChangeWindow(table);
                userAddAndChangeWindow.ShowDialog();
                FillDataMainGrid();
            }
            else
            {
                MessageBox.Show("error");
                FillDataMainGrid();
                return;
            }
        }

        private void MainDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {

        }
    }
}
