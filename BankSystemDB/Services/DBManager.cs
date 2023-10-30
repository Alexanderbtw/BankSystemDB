using Homework_18.Model;
using System.Collections.ObjectModel;

namespace Homework_18.Services
{
    class DBManager
    {
        private IDBService dBService;

        public DBManager()
        {
            //this.dBService = new ADODBService();
            this.dBService = new EFDBService();
        }

        public Account SelectAccount(int clientId)
        {
            return dBService.SelectAccount(clientId);
        }
        public void InsertAccount(Account account)
        {
            dBService.InsertAccount(account);
        }
        public void UpdateAccount(Account account)
        {
            dBService.UpdateAccount(account);
        }
        public void DeleteAccount(Account account)
        {
            dBService.DeleteAccount(account);
        }


        public ObservableCollection<Client> GetAllClients()
        {
            return dBService.GetAllClients();
        }
        public ObservableCollection<Client> GetClientsInDepartment(Department department)
        {
            return dBService.GetClientsInDepartment(department);
        }
        public void InsertClient(Client client)
        {
            dBService.InsertClient(client);
        }
        public void UpdateClient(Client client)
        {
            dBService.UpdateClient(client);
        }
        public void DeleteClient(Client client)
        {
            dBService.DeleteClient(client);
        }


        public ObservableCollection<Department> GetAllDepartments()
        {
            return dBService.GetAllDepartments();
        }
        public Department SelectDepartment(int id)
        {
            return dBService.SelectDepartment(id);
        }
        public void InsertDepartment(Department department)
        {
            dBService.InsertDepartment(department);
        }
        public void UpdateDepartment(Department department)
        {
            dBService.UpdateDepartment(department);
        }
        public void DeleteDepartment(Department department)
        {
            dBService.DeleteDepartment(department);
        }

        public ObservableCollection<Deposit> SelectClientDeposits(int clientId)
        {
            return dBService.SelectClientDeposits(clientId);
        }
        public void InsertDeposit(Deposit deposit)
        {
            dBService.InsertDeposit(deposit);
        }
    }
}
