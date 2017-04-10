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
        private Button _addMove;
        private ListView _incompleteList;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _addMove = FindViewById<Button>(Resource.Id.moveAdd);
            _addMove.Click += delegate
            {
                var next = new Intent(this, typeof(TodoDetailActivity));
                StartActivity(next);
            };


            _incompleteList = FindViewById<ListView>(Resource.Id.incompleteList);
            var incomleteTodOs = Todo.GetTodo().Where(todo => !todo.Completed).ToList();
            var adapter = new CustomAdapter(this, incomleteTodOs);
            _incompleteList.Adapter = adapter;
            _incompleteList.ItemClick += (sender, e) =>
            {
                var next = new Intent(this, typeof(TodoDetailActivity));
                next.PutExtra("targetTODO", adapter[e.Position].Id);
                StartActivity(next);
            };
        }
    }

    public class CustomAdapter : BaseAdapter<Todo>
    {
        private readonly Activity _context;
        private readonly List<Todo> _todoList;

        public CustomAdapter(Activity context, List<Todo> items)
        {
            _context = context;
            _todoList = items;
        }

        public override Todo this[int position] => _todoList[position];

        public override int Count => _todoList.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this[position];

            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.todoCell, parent, false);

            // BaseAdapter<T>の対応するプロパティを割り当て
            view.FindViewById<TextView>(Resource.Id.todoName).Text = item.Name;

            return view;
        }
    }
}