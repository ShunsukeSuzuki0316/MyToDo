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
        /// TODO�̐ÓI�R���X�g���N�^
        /// </summary>
        static Todo()
        {
            // TODO�e�[�u�����i�[����f�[�^�x�[�X���쐬���܂�
            DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mytodo.db3");

            using (var con = new SQLiteConnection(DbPath))
            {
                con.DropTable<Todo>();
                // TODO�e�[�u�����쐬���܂�(�e�[�u��������ꍇ�͉����������܂���)
                con.CreateTable<Todo>();
            }
        }

        // TODO��ID
        [AutoIncrement]
        [PrimaryKey]
        [Column("_id")]
        public int Id { get; set; }

        // TODO�̖��O
        [Column("_name")]
        public string Name { get; set; }

        // TODO�̏ڍ�
        [Column("_description")]
        public string Description { get; set; }

        // TODO�̊����t���O
        [Column("_completed")]
        public bool Completed { get; set; }
        
        /// <summary>
        /// TODO�e�[�u������TODO��S���擾���܂�
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
        /// TODO��ID����擾���܂�
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
        /// TODO��V�K�o�^���܂�
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
        /// TODO���X�V���܂�
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