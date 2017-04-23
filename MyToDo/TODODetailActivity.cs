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
        private Button _saveButton; // �ۑ��{�^��
        private Button _returnButton; // �߂�{�^��

        private EditText _todoNameEditText; // TODO���̃e�L�X�g�{�b�N�X
        private EditText _todoDescriptionEditText; // TODO�̏ڍׂ̃e�L�X�g�{�b�N�X

        private TextView _completedTextView; // �����e�L�X�g
        private Switch _completedSwitch; // �����̃X�C�b�`

        private Todo _todo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.todoDetail);

            // axml�t�@�C������\���v�f��T���܂�
            _saveButton = FindViewById<Button>(Resource.Id.saveButton);
            _returnButton = FindViewById<Button>(Resource.Id.returnButton);
            _todoNameEditText = FindViewById<EditText>(Resource.Id.todoNameEditText);
            _todoDescriptionEditText = FindViewById<EditText>(Resource.Id.todoDescriptionEditText);
            _completedTextView = FindViewById<TextView>(Resource.Id.completedTextView);�@
            _completedSwitch = FindViewById<Switch>(Resource.Id.completedSwitch);

            // ��ʑJ�ڎ��ɓn���ꂽ�l���擾���܂�
            var id = Intent.GetIntExtra("targetTODO", -1);



            if (id != -1)
            {
                // ���X�g�̃A�C�e�����^�b�v���ꂽ�ꍇ�́A�^�b�v����TODO�̏����e�v�f�ɐݒ肵�܂�
                _todo = Todo.GetTodoById(id);

                // TODO���̃e�L�X�g�{�b�N�X��TODO����ݒ肵�܂�
                _todoNameEditText.Text = _todo.Name;
                // TODO�̏ڍׂ̃e�L�X�g�{�b�N�X��TODO�̏ڍׂ�ݒ肵�܂�
                _todoDescriptionEditText.Text = _todo.Description;
                
                // �ۑ��{�^���̃A�N�V�����ɂ�TODO�̍X�V������ݒ肵�܂�
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
                // TODO�V�K�ǉ��̏ꍇ�͊����̃X�C�b�`�͔�\���ɂ��܂�
                _completedSwitch.Visibility = ViewStates.Invisible;
                _completedTextView.Visibility = ViewStates.Invisible;
                
                // �ۑ��{�^���̃A�N�V�����ɂ�TODO�̒ǉ�������ݒ肵�܂�
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

            // �߂�{�^���̃A�N�V�����ɂ͈ꗗ��ʂւ̑J�ڏ�����ݒ肵�܂�
            _returnButton.Click += delegate
            {
                var next = new Intent(this, typeof(MainActivity));
                StartActivity(next);
            };

            
        }
    }
}