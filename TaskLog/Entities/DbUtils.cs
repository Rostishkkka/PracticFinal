using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskLog.Entities
{
    class DbUtils
    {
        public static Db db;

        static DbUtils() 
        {
            try
            {
                db = new Db();
            }    
            catch { MessageBox.Show("Error of connection to database", MessageBoxButton.OK.ToString()); }
        
        }
    }
}
