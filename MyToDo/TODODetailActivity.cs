using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Exception = System.Exception;

namespace MyToDo
{
    [Activity(Label = "TODODetailActivity")]
    public class TodoDetailActivity : Activity
    {
        private Button _addButton;
        private Button _updateButton;
        private Button _returnButton;

        private EditText _todoName;
        private EditText _todoDescription;

        private Switch _alert;
        private DatePicker _alertDatePicker;
        private TimePicker _alertTimePicker;

        private Switch _completed;

        private Todo _todo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.todoDetail);

            var id = Intent.GetIntExtra("targetTODO", -1);

            _addButton = FindViewById<Button>(Resource.Id.addButton);
            _updateButton = FindViewById<Button>(Resource.Id.updateButton);
            _returnButton = FindViewById<Button>(Resource.Id.returnButton);
            _todoName = FindViewById<EditText>(Resource.Id.todoName);
            _todoDescription = FindViewById<EditText>(Resource.Id.todoDescription);
            _alert = FindViewById<Switch>(Resource.Id.alert);
            _alertDatePicker = FindViewById<DatePicker>(Resource.Id.alertDatePicker);
            _alertTimePicker = FindViewById<TimePicker>(Resource.Id.alertTimePicker);
            _completed = FindViewById<Switch>(Resource.Id.completed);

            _alertDatePicker.Visibility = ViewStates.Invisible;
            _alertDatePicker.UpdateDate(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day);
            _alertTimePicker.Visibility = ViewStates.Invisible;


            _alert.CheckedChange += (sender, e) =>
            {
                if (_alert.Checked)
                {
                    _alertDatePicker.Visibility = ViewStates.Visible;
                    _alertTimePicker.Visibility = ViewStates.Visible;
                }
                else
                {
                    _alertDatePicker.Visibility = ViewStates.Invisible;
                    _alertTimePicker.Visibility = ViewStates.Invisible;
                }
            };

            if (id != -1)
            {
                _todo = Todo.GetTodoById(id);
                _addButton.Visibility = ViewStates.Invisible;
                _todoName.Text = _todo.Name;
                _todoDescription.Text = _todo.Description;
                _completed.Checked = _todo.Completed;
                _alert.Checked = _todo.Alert;
                if (_todo.Alert)
                {
                    var time = _todo.AlertTime;
                    _alertDatePicker.UpdateDate(time.Year, time.Month - 1, time.Day);
                    _alertTimePicker.CurrentHour = (Integer) time.Hour;
                    _alertTimePicker.CurrentMinute = (Integer) time.Minute;
                }
            }
            else
            {
                _updateButton.Visibility = ViewStates.Invisible;
                _completed.Visibility = ViewStates.Invisible;

                _alert.Checked = false;

                _addButton.Click += delegate
                {
                    var targetTodo = new Todo
                    {
                        Name = _todoName.Text,
                        Description = _todoDescription.Text,
                        Completed = _completed.Checked
                    };

                    if (_alert.Checked)
                    {
                        var newDate = _alertDatePicker.DateTime;
                        var span = new TimeSpan(_alertTimePicker.CurrentHour.IntValue(),
                            _alertTimePicker.CurrentMinute.IntValue(), 0);
                        targetTodo.AlertTime = newDate + span;
                        targetTodo.Alert = true;
                    }
                    else
                    {
                        targetTodo.Alert = false;
                    }

                    Todo.AddTodo(targetTodo);

                    Remind(targetTodo);

                    var next = new Intent(this, typeof(MainActivity));
                    StartActivity(next);
                };
            }

            _returnButton.Click += delegate
            {
                var next = new Intent(this, typeof(MainActivity));
                StartActivity(next);
            };

            _updateButton.Click += delegate
            {
                _todo.Name = _todoName.Text;
                _todo.Description = _todoDescription.Text;
                _todo.Completed = _completed.Checked;

                if (_alert.Checked)
                {
                    var newDate = _alertDatePicker.DateTime;
                    var span = new TimeSpan(_alertTimePicker.CurrentHour.IntValue(),
                        _alertTimePicker.CurrentMinute.IntValue(), 0);

                    _todo.AlertTime = newDate + span;
                    _todo.Alert = true;
                }
                else
                {
                    _todo.Alert = false;
                }

                Todo.UpdateTodo(_todo);

                Remind(_todo);

                var next = new Intent(this, typeof(MainActivity));
                StartActivity(next);
            };
        }

        public void Remind(Todo todo)
        {
            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();

            try
            {
                var deletAlarmIntent = new Intent(this, typeof(AlarmReceiver));
                var deletePending =
                    PendingIntent.GetBroadcast(this, todo.Id, deletAlarmIntent, PendingIntentFlags.UpdateCurrent);

                deletePending.Cancel();
                alarmManager.Cancel(deletePending);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var alarmIntent = new Intent(this, typeof(AlarmReceiver));


            alarmIntent.PutExtra("title", todo.Name);
            alarmIntent.PutExtra("message", todo.Description);

            var pending = PendingIntent.GetBroadcast(this, todo.Id, alarmIntent, PendingIntentFlags.UpdateCurrent);


            var ts = todo.AlertTime - DateTime.Now;

            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + (long) ts.TotalMilliseconds,
                pending);
        }
    }
}