using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;

namespace MyToDo
{
    [Table("todo")]
    public class Todo
    {
        private static readonly string DbPath;

        static Todo()
        {
            DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mytodo.db3");

            using (var con = new SQLiteConnection(DbPath))
            {
                con.CreateTable<Todo>();
            }
        }

        [AutoIncrement]
        [PrimaryKey]
        [Column("_id")]
        public int Id { get; set; }

        [Column("_name")]
        public string Name { get; set; }

        [Column("_description")]
        public string Description { get; set; }

        [Column("_completed")]
        public bool Completed { get; set; }

        [Column("_alert")]
        public bool Alert { get; set; }

        [Column("_alertTime")]
        public DateTime AlertTime { get; set; }

        [Column("_completedTime")]
        public DateTime CompletedDate { get; set; }


        public static List<Todo> GetTodo()
        {
            using (var con = new SQLiteConnection(DbPath))
            {
                var result = from record in con.Table<Todo>() select record;
                var resultList = result.Count() != 0 ? result.ToList() : new List<Todo>();
                return resultList;
            }
        }

        public static Todo GetTodoById(int id)
        {
            using (var con = new SQLiteConnection(DbPath))
            {
                var result = from record in con.Table<Todo>() where record.Id == id select record;

                return result.First();
            }
        }

        public static int AddTodo(Todo todo)
        {
            using (var con = new SQLiteConnection(DbPath))
            {
                var res = con.Insert(todo);
                return res;
            }
        }

        public static int UpdateTodo(Todo todo)
        {
            using (var con = new SQLiteConnection(DbPath))
            {
                return con.Update(todo);
            }
        }
    }
}