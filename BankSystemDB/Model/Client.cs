using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Homework_18.Model
{
    public class Client : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private int departmentId;
        public int DepartmentId
        {
            get { return departmentId; }
            set
            {
                departmentId = value;
                OnPropertyChanged();
            }
        }

        public Department Department { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Deposit> Deposits { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
