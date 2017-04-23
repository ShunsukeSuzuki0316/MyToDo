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

        /// <summary>
        /// TODOの静的コンストラクタ
        /// </summary>
        static Todo()
        {
            // TODOテーブルを格納するデータベースを作成します
            DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mytodo.db3");

            using (var con = new SQLiteConnection(DbPath))
            {
                con.DropTable<Todo>();
                // TODOテーブルを作成します(テーブルがある場合は何も処理しません)
                con.CreateTable<Todo>();
            }
        }

        // TODOのID
        [AutoIncrement]
        [PrimaryKey]
        [Column("_id")]
        public int Id { get; set; }

        // TODOの名前
        [Column("_name")]
        public string Name { get; set; }

        // TODOの詳細
        [Column("_description")]
        public string Description { get; set; }

        // TODOの完了フラグ
        [Column("_completed")]
        public bool Completed { get; set; }
        
        /// <summary>
        /// TODOテーブルからTODOを全件取得します
        /// </summary>
        /// <returns></returns>
        public static List<Todo> GetTodo()
        {
            using (var con = new SQLiteConnection(DbPath))
            {
                var result = from record in con.Table<Todo>() select record;
                var resultList = result.Count() != 0 ? result.ToList() : new List<Todo>();
                return resultList;
            }
        }

        /// <summary>
        /// TODOをIDから取得します
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Todo GetTodoById(int id)
        {
            using (var con = new SQLiteConnection(DbPath))
            {
                var result = from record in con.Table<Todo>() where record.Id == id select record;

                return result.First();
            }
        }

        /// <summary>
        /// TODOを新規登録します
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        public static int AddTodo(Todo todo)
        {
            using (var con = new SQLiteConnection(DbPath))
            {
                var res = con.Insert(todo);
                return res;
            }
        }

        /// <summary>
        /// TODOを更新します
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        public static int UpdateTodo(Todo todo)
        {
            using (var con = new SQLiteConnection(DbPath))
            {
                return con.Update(todo);
            }
        }
    }
}