using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Linq;
using System.Collections.Generic;

namespace MyToDo
{
    [Activity(Label = "MyToDo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        ListView incompleteList;
        List<TODO> todoList = new List<TODO>();

        Button addMove;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // change

            addMove = FindViewById<Button>(Resource.Id.moveAdd);
            addMove.Click += delegate
            {
                var next = new Intent(this, typeof(TODODetailActivity));
                StartActivity(next);
            };


            incompleteList = FindViewById<ListView>(Resource.Id.incompleteList);
            var incomleteTODOs = TODO.getTODO().Where(todo=>!todo.completed).ToList();
            var adapter = new CustomAdapter(this, incomleteTODOs);
            incompleteList.Adapter = adapter;
            incompleteList.ItemClick += (sender,e) => {

                var next = new Intent(this, typeof(TODODetailActivity));
                var target = (ListView)sender;
                next.PutExtra("targetTODO", adapter[e.Position].id);
                StartActivity(next);

            };
        }
    }

    public class CustomAdapter : BaseAdapter<TODO>
    {
        List<TODO> todoList;
        Activity context;

        public CustomAdapter(Activity context, List<TODO> items) : base()
        {
            this.context = context;
            this.todoList = items;
        }

        public override TODO this[int position]
        {
            get
            {
                return todoList[position];
            }
        }

        public override int Count
        {
            get
            {
                return todoList.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TODO item = this[position];

            View view = convertView;
            if (view == null) // no view to re-use, create new
           
                view = context.LayoutInflater.Inflate(Resource.Layout.todoCell,parent,false);

            // BaseAdapter<T>の対応するプロパティを割り当て
            view.FindViewById<TextView>(Resource.Id.todoName).Text = item.name;

            return view;
        }
    }

}

