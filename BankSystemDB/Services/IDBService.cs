using Homework_18.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Homework_18.Services
{
    public interface IDBService
    {
        public Account SelectAccount(int clientId);
        public void InsertAccount(Account account);
        public void UpdateAccount(Account account);
        public void DeleteAccount(Account account);


        public ObservableCollection<Client> GetAllClients();
        public ObservableCollection<Client> GetClientsInDepartment(Department department);
        public void InsertClient(Client client);
        public void UpdateClient(Client client);
        public void DeleteClient(Client client);


        public ObservableCollection<Department> GetAllDepartments();
        public Department SelectDepartment(int id);
        public void InsertDepartment(Department department);
        public void UpdateDepartment(Department department);
        public void DeleteDepartment(Department department);

        public ObservableCollection<Deposit> SelectClientDeposits(int clientId);
        public void InsertDeposit(Deposit deposit);
    }
}
