using Homework_18.Helper;
using Homework_18.Model;
using System.Windows;

namespace Homework_18.ViewModel
{
    class DepartmentViewModel : BaseViewModel
    {
        private Department department;
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

        public DepartmentViewModel(Department department)
        {
            this.department = department;
            Name = department.Name;
        }

        private RelayCommand confirmCommand;
        public RelayCommand ConfirmCommand
        {
            get
            {
                return confirmCommand ??= new RelayCommand(obj =>
                    {
                        department.Name = Name;
                        Window window = obj as Window;
                        window.DialogResult = true;
                        window.Close();
                    },
                    obj => !string.IsNullOrEmpty(Name));
            }
        }
    }
}
