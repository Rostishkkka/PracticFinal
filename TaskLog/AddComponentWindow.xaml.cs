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
    /// Interaction logic for AddComponentWindow.xaml
    /// </summary>
    public partial class AddComponentWindow : Window
    {
        public AddComponentWindow()
        {
            InitializeComponent();
        }

        private bool CheckFillOfTextBoxes()
        {
            if(OemNameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Название не может быть пустым", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool CheckSameComponentInDb()
        {
            if(DbUtils.db.Components.Any(x => x.CompOemId == OemIdTextBox.Text && 
                x.CompOemVer == OemVerTextBox.Text &&
                x.CompOemName == OemNameTextBox.Text &&
                x.SwVer == SwVerTextBox.Text)) 
            {
                MessageBox.Show("Такой компонент уже существует", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFillOfTextBoxes() || !CheckSameComponentInDb()) { return; }
            try
            {
                Components component = new Components();
                component.CompOemId = OemIdTextBox.Text;
                component.CompOemVer = OemVerTextBox.Text;
                component.CompOemName = OemNameTextBox.Text;
                component.SwVer = SwVerTextBox.Text;
                DbUtils.db.Components.Add(component);
                DbUtils.db.SaveChanges();
                MessageBox.Show("Компонент успешно добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            PreviousButton_Click(sender, e);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            ComponentWindow componentWindow = new ComponentWindow();
            componentWindow.Show();
            this.Close();
        }
    }
}
