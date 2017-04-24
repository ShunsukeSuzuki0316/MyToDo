using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MyToDo
{
    [Activity(Label = "MyToDo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _addTodoButton; // 追加ボタン
        private ListView _todoListView; // TODOリスト

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);


            // axmlファイルから構成要素を探します
            _addTodoButton = FindViewById<Button>(Resource.Id.addTodoButton);
            _todoListView  = FindViewById<ListView>(Resource.Id.todoListView);
            

            // テーブルから完了していないタスクの一覧を取得します
            var incomleteTodOs = Todo.GetTodo().Where(todo => !todo.Completed).ToList();

            // リストを制御するためのアダプターを設定します
            var adapter = new CustomAdapter(this, incomleteTodOs);
            _todoListView.Adapter = adapter;


            // 追加ボタンをタップした時に実行するアクション
            _addTodoButton.Click += delegate
            {
                var next = new Intent(this, typeof(TodoDetailActivity));
                StartActivity(next);
            };

            // リストのアイテムをタップした時のアクション
            _todoListView.ItemClick += (sender, e) =>
            {
                var next = new Intent(this, typeof(TodoDetailActivity));
                next.PutExtra("targetTODO", adapter[e.Position].Id);
                StartActivity(next);
            };
        }
    }

    public class CustomAdapter : BaseAdapter<Todo>
    {
        private readonly Activity _context; // リストを使用するActivity
        private readonly List<Todo> _todoList; // リストで使用するデータ

        public CustomAdapter(Activity context, List<Todo> items)
        {
            _context = context;
            _todoList = items;
        }

        /// <summary>
        /// リストに使用するデータのインデクサー
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Todo this[int position] => _todoList[position];

        /// <summary>
        /// リストの件数 = データの件数
        /// </summary>
        public override int Count => _todoList.Count;

        /// <summary>
        /// タップしたアイテムのインデックスを取得します
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// リストのアイテムに表示するものを設定します　
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this[position];


            // リストのアイテムにaxmlファイルで作成した内容を設定します
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.todoCell, parent, false);

           
            // テキストビューにTODO名を設定します
            view.FindViewById<TextView>(Resource.Id.todoName).Text = item.Name;

            return view;
        }
    }
}