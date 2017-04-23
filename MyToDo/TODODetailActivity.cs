using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MyToDo
{
    [Activity(Label = "TODODetailActivity")]
    public class TodoDetailActivity : Activity
    {
        private Button _saveButton; // 保存ボタン
        private Button _returnButton; // 戻るボタン

        private EditText _todoNameEditText; // TODO名のテキストボックス
        private EditText _todoDescriptionEditText; // TODOの詳細のテキストボックス

        private TextView _completedTextView; // 完了テキスト
        private Switch _completedSwitch; // 完了のスイッチ

        private Todo _todo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.todoDetail);

            // axmlファイルから構成要素を探します
            _saveButton = FindViewById<Button>(Resource.Id.saveButton);
            _returnButton = FindViewById<Button>(Resource.Id.returnButton);
            _todoNameEditText = FindViewById<EditText>(Resource.Id.todoNameEditText);
            _todoDescriptionEditText = FindViewById<EditText>(Resource.Id.todoDescriptionEditText);
            _completedTextView = FindViewById<TextView>(Resource.Id.completedTextView);　
            _completedSwitch = FindViewById<Switch>(Resource.Id.completedSwitch);

            // 画面遷移時に渡された値を取得します
            var id = Intent.GetIntExtra("targetTODO", -1);



            if (id != -1)
            {
                // リストのアイテムをタップされた場合は、タップしたTODOの情報を各要素に設定します
                _todo = Todo.GetTodoById(id);

                // TODO名のテキストボックスにTODO名を設定します
                _todoNameEditText.Text = _todo.Name;
                // TODOの詳細のテキストボックスにTODOの詳細を設定します
                _todoDescriptionEditText.Text = _todo.Description;
                
                // 保存ボタンのアクションにはTODOの更新処理を設定します
                _saveButton.Click += delegate
                {
                    _todo.Name = _todoNameEditText.Text;
                    _todo.Description = _todoDescriptionEditText.Text;
                    _todo.Completed = _completedSwitch.Checked;


                    Todo.UpdateTodo(_todo);

                    var next = new Intent(this, typeof(MainActivity));
                    StartActivity(next);
                };
            }
            else
            {
                // TODO新規追加の場合は完了のスイッチは非表示にします
                _completedSwitch.Visibility = ViewStates.Invisible;
                _completedTextView.Visibility = ViewStates.Invisible;
                
                // 保存ボタンのアクションにはTODOの追加処理を設定します
                _saveButton.Click += delegate
                {
                    var targetTodo = new Todo
                    {
                        Name = _todoNameEditText.Text,
                        Description = _todoDescriptionEditText.Text,
                        Completed = _completedSwitch.Checked
                    };


                    Todo.AddTodo(targetTodo);

                    var next = new Intent(this, typeof(MainActivity));
                    StartActivity(next);
                };
            }

            // 戻るボタンのアクションには一覧画面への遷移処理を設定します
            _returnButton.Click += delegate
            {
                var next = new Intent(this, typeof(MainActivity));
                StartActivity(next);
            };

            
        }
    }
}