﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private long CurrentCompId, UserId;

        public AddTaskWindow()
        {
            InitializeComponent();
            App app = (App)Application.Current;
            UserId = app.UserId;
        }

        private void SelectComponentButton_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия на кнопку "SelectComponent". Открывает окно выбора компонента и возвращает выбранный компонент
        {
            ComponentWindow componentWindow = new ComponentWindow();
            bool? result = componentWindow.ShowDialog();
            if(result == true) 
            {
                var ReturnedComponent = componentWindow.ReturnedComponent;
                IdComponentTB.Text = ReturnedComponent.CompOemId;
                VerComponentTB.Text = ReturnedComponent.CompOemVer;
                NameComponentTB.Text = ReturnedComponent.CompOemName;
                SwVerComponentTB.Text = ReturnedComponent.SwVer;
                CurrentCompId = ReturnedComponent.CompId;
                CreateButton.IsEnabled = true;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия на кнопку "CreateButton". Создает новую задачу на основе введенных значений и полученного компонента 
        {
            TextRange textRange = new TextRange(TaskDescrTextBox.Document.ContentStart
                        , TaskDescrTextBox.Document.ContentEnd);
            string TaskDescr = textRange.Text.Replace("\r\n", string.Empty);
            if (SerialNumberTextBox.Text.Length > 0 && TaskDescr.Length > 0)
            {
                try
                {
                    Tasks task = new Tasks();
                    task.CompId = CurrentCompId;
                    task.TaskDescr = TaskDescr;
                    task.CompSn = SerialNumberTextBox.Text;
                    task.UserId = UserId;
                    DbUtils.db.Tasks.Add(task);
                    DbUtils.db.SaveChanges();
                    EventLog eventLog = new EventLog();
                    eventLog.EventType = "Создано";
                    eventLog.EventTimestamp = DateTime.Now;
                    eventLog.TaskId = task.TaskId;
                    eventLog.UserId = task.UserId;
                    DbUtils.db.EventLog.Add(eventLog);
                    DbUtils.db.SaveChanges();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия на кнопку "PreviousButton". Закрывает это окно
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
