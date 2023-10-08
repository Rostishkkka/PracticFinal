﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskLog
{
    public class ConnectionStringProvider
    {
        public static string GetDataSource() // статический метод, который считывает данные из .json файла, помещает их в переменные, формирует и возвращает строку подключения к БД
        {
            string filePath = Directory.GetCurrentDirectory() + @"\dataSource\dataSource.json";
            try
            {
                string json = File.ReadAllText(filePath);
                dynamic data = JsonConvert.DeserializeObject(json);
                string dataSource = data.DataSource;
                string userID = data.UserID;
                string pass = data.Password;
                string connectionString = $"Data Source={dataSource};" +
                               "Initial Catalog=TaskLog;" +
                               $"User ID= {userID};" +
                               $"Password = {pass};" +
                               "Trust Server Certificate=True;" +
                               "Command Timeout=300;" +
                               "MultipleActiveResultSets=True";
                return connectionString;
            }
            catch (IOException)
            {
                MessageBox.Show($"Ошибка чтения файла {filePath}");
                throw;
            }
        }
    }
}
