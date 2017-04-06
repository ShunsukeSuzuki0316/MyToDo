using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;

namespace MyToDo
{
    [Table("todo")]
    public class TODO
    {
        [AutoIncrement,PrimaryKey,Column("_id")]
        public int id { get; set; }
        [Column("_name")]
        public string name { get; set; }
        [Column("_description")]
        public string description { get; set; }
        [Column("_completed")]
        public bool completed { get; set; }
        [Column("_alert")]
        public bool alert { get; set; }
        [Column("_alertTime")]
        public DateTime alertTime { get; set; }
        [Column("_completedTime")]
        public DateTime completedDate { get; set; }


        private static string dbPath;

        static TODO()
        {
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mytodo.db3");

            using (SQLiteConnection con = new SQLiteConnection(dbPath))
            {
                con.CreateTable<TODO>();
            }
        }


        public static List<TODO> getTODO()
        {

            using (SQLiteConnection con = new SQLiteConnection(dbPath))
            {
                var result = from record in con.Table<TODO>() select record;
                var resultList = result.Count() != 0 ? result.ToList() : new List<TODO>();
                return resultList;
            }
        }

        public static TODO getTODOById(int id)
        {
            using (SQLiteConnection con = new SQLiteConnection(dbPath))
            {
                var result = from record in con.Table<TODO>() where record.id == id select record;

                return result.First();   
            }
        }

        public static int addTODO(TODO todo)
        {

            using (SQLiteConnection con = new SQLiteConnection(dbPath))
            {
                var res = con.Insert(todo);
                return res;
            }
        }

        public static int updateTODO(TODO todo)
        {
            using (SQLiteConnection con = new SQLiteConnection(dbPath))
            {
                return con.Update(todo);
            }
        }

    }
}