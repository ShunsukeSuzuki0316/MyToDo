using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyToDo
{
    [Activity(Label = "TODODetailActivity")]
    public class TODODetailActivity : Activity
    {
        Button addButton;
        Button updateButton;
        Button returnButton;

        EditText todoName;
        EditText todoDescription;

        Switch alert;
        DatePicker alertDatePicker;
        TimePicker alertTimePicker;

        Switch completed;

        TODO todo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.todoDetail);

            // Create your application here
            int id = Intent.GetIntExtra("targetTODO",-1);

            addButton = FindViewById<Button>(Resource.Id.addButton);
            updateButton = FindViewById<Button>(Resource.Id.updateButton);
            returnButton = FindViewById<Button>(Resource.Id.returnButton);
            todoName = FindViewById<EditText>(Resource.Id.todoName);
            todoDescription = FindViewById<EditText>(Resource.Id.todoDescription);
            alert = FindViewById<Switch>(Resource.Id.alert);
            alertDatePicker = FindViewById<DatePicker>(Resource.Id.alertDatePicker);
            alertTimePicker = FindViewById<TimePicker>(Resource.Id.alertTimePicker);
            completed = FindViewById<Switch>(Resource.Id.completed);

            alertDatePicker.Visibility = ViewStates.Invisible;
            alertDatePicker.UpdateDate(DateTime.Today.Year,DateTime.Today.Month - 1,DateTime.Today.Day);
            alertTimePicker.Visibility = ViewStates.Invisible;


            alert.CheckedChange += (sender, e) => {
                if (alert.Checked)
                {
                    alertDatePicker.Visibility = ViewStates.Visible;
                    alertTimePicker.Visibility = ViewStates.Visible;
                }else
                {
                    alertDatePicker.Visibility = ViewStates.Invisible;
                    alertTimePicker.Visibility = ViewStates.Invisible;
                }

            };

            if(id != -1)
            {
                todo = TODO.getTODOById(id);
                addButton.Visibility = ViewStates.Invisible;
                todoName.Text = todo.name;
                todoDescription.Text = todo.description;
                completed.Checked = todo.completed;
                alert.Checked = todo.alert;
                if(todo.alertTime != null)
                {
                    DateTime time = todo.alertTime;
                    alertDatePicker.UpdateDate(time.Year, (time.Month - 1), time.Day);
                    alertTimePicker.CurrentHour = (Java.Lang.Integer)time.Hour;
                    alertTimePicker.CurrentMinute = (Java.Lang.Integer)time.Minute;
                }
             
                
            }else
            {
                updateButton.Visibility = ViewStates.Invisible;
                completed.Visibility = ViewStates.Invisible;

                alert.Checked = false;

                addButton.Click += delegate {
                    var targetTODO = new TODO { name = todoName.Text, description = todoDescription.Text, completed = completed.Checked };

                    if (alert.Checked)
                    {
                        DateTime newDate = new DateTime();
                        newDate = alertDatePicker.DateTime;
                        TimeSpan span = new TimeSpan(alertTimePicker.CurrentHour.IntValue(), alertTimePicker.CurrentMinute.IntValue(), 0);
                        targetTODO.alertTime = newDate + span;
                        targetTODO.alert = true;
                    }else
                    {
                        targetTODO.alert = false;
                    }

                    TODO.addTODO(targetTODO);

                    Remind(targetTODO);

                    var next = new Intent(this, typeof(MainActivity));
                    StartActivity(next);
                };
            }

            returnButton.Click += delegate {

                var next = new Intent(this, typeof(MainActivity));
                StartActivity(next);

            };

            updateButton.Click += delegate
            {

                todo.name = todoName.Text;
                todo.description = todoDescription.Text;
                todo.completed = completed.Checked;

                if (alert.Checked)
                {
                    DateTime newDate = new DateTime();
                    newDate = alertDatePicker.DateTime;
                    TimeSpan span = new TimeSpan(alertTimePicker.CurrentHour.IntValue(), alertTimePicker.CurrentMinute.IntValue(),0);
                    
                    todo.alertTime = newDate+span;
                    todo.alert = true;
                }
                else
                {
                    todo.alert = false;
                }

                TODO.updateTODO(todo);

                Remind(todo);

                var next = new Intent(this, typeof(MainActivity));
                StartActivity(next);
            };

        }
        public void Remind(TODO todo)
        {

            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();

            try
            {
                var deletAlarmIntent = new Intent(this, typeof(AlarmReceiver));
                var deletePending = PendingIntent.GetBroadcast(this, todo.id, deletAlarmIntent, PendingIntentFlags.UpdateCurrent);

                deletePending.Cancel();
                alarmManager.Cancel(deletePending);

            }
            catch(Exception e)
            {
                Console.WriteLine(e);

            }

            var alarmIntent = new Intent(this, typeof(AlarmReceiver));
            

            alarmIntent.PutExtra("title", todo.name);
            alarmIntent.PutExtra("message", todo.description);

            var pending = PendingIntent.GetBroadcast(this, todo.id, alarmIntent, PendingIntentFlags.UpdateCurrent);


            TimeSpan ts = todo.alertTime - DateTime.Now;

            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + ((long)ts.TotalMilliseconds ), pending);

        }
    }
}