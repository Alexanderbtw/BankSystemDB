namespace Homework_18.ViewModel
{
    class NotificationWindowViewModel : BaseViewModel
    {
        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }
    }
}
