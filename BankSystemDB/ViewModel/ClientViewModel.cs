using Homework_18.Helper;
using Homework_18.Model;
using System.Collections.Generic;
using System.Windows;
using Homework_18.Services;

namespace Homework_18.ViewModel
{
    class ClientViewModel : BaseViewModel
    {
        private Client client;
        private string name;
        private DBManager dBmanager;

        private Department selectedDepartment;
        private IEnumerable<Department> departments;

        public ClientViewModel(Client client)
        {
            dBmanager = new DBManager();
            this.client = client;
            Name = client.Name;
            departments = dBmanager.GetAllDepartments();
            SelectedDepartment = dBmanager.SelectDepartment(client.DepartmentId);
        }

        public string Name
        {
            get { return name; }
            set
            {
                this.name = value;
                OnPropertyChanged();
            }
        }
        public Department SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                this.selectedDepartment = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<Department> Departments
        {
            get { return departments; }
            set
            {
                this.departments = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                   (saveCommand = new RelayCommand(obj =>
                   {
                       client.Name = Name;
                       client.DepartmentId = SelectedDepartment.Id;

                       Window window = obj as Window;
                       window.DialogResult = true;
                       window.Close();

                   },
                   obj => !string.IsNullOrEmpty(Name) && SelectedDepartment != null));
            }
        }
    }
}
