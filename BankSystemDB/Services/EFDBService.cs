using Homework_18.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Data.Entity.Core;
using System.Diagnostics;

namespace Homework_18.Services
{
    class EFDBService : IDBService
    {
        public void DeleteAccount(Account account)
        {
            using (var ctx = new BankDBContext())
            {
                ctx.Accounts.Attach(account);
                ctx.Accounts.Remove(account);
                ctx.SaveChanges();
            }
        }

        public void DeleteClient(Client client)
        {
            using (var ctx = new BankDBContext())
            {
                ctx.Clients.Attach(client);
                ctx.Clients.Remove(client);
                ctx.SaveChanges();
            }
        }

        public void DeleteDepartment(Department department)
        {
            using (var ctx = new BankDBContext())
            {
                ctx.Departments.Attach(department);
                ctx.Departments.Remove(department);
                ctx.SaveChanges();
            }
        }

        public ObservableCollection<Client> GetAllClients()
        {
            try
            {
                using (var ctx = new BankDBContext())
                {
                    return new ObservableCollection<Client>(ctx.Clients);
                }
            }
            catch (EntityException ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<Department> GetAllDepartments()
        {
            try
            {
                using (var ctx = new BankDBContext())
                {
                    return new ObservableCollection<Department>(ctx.Departments);
                }
            }
            catch (EntityException ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<Client> GetClientsInDepartment(Department department)
        {
            using (var ctx = new BankDBContext())
            {
                var clients = ctx.Clients.Where(x => x.DepartmentId == department.Id);
                return new ObservableCollection<Client>(clients);
            }
        }

        public void InsertAccount(Account account)
        {
            using (var ctx = new BankDBContext())
            {
                ctx.Accounts.Add(account);
                ctx.SaveChanges();
            }
        }

        public void InsertClient(Client client)
        {
            using (var ctx = new BankDBContext())
            {
                ctx.Clients.Add(client);
                ctx.SaveChanges();
            }
        }

        public void InsertDepartment(Department department)
        {
            using (var ctx = new BankDBContext())
            {
                ctx.Departments.Add(department);
                ctx.SaveChanges();
            }
        }

        public void InsertDeposit(Deposit deposit)
        {
            using (var ctx = new BankDBContext())
            {
                ctx.Deposits.Add(deposit);
                ctx.SaveChanges();
            }
        }

        public Account SelectAccount(int clientId)
        {
            using (var ctx = new BankDBContext())
            {
                return ctx.Accounts.Where(x => x.ClientId == clientId).FirstOrDefault();
            }          
        }

        public ObservableCollection<Deposit> SelectClientDeposits(int clientId)
        {
            try
            {
                using (var ctx = new BankDBContext())
                {
                    var deposits = ctx.Deposits.Where(x => x.ClientId == clientId);

                    return new ObservableCollection<Deposit>(deposits);
                }
            }
            catch (EntityException ex)
            {
                throw ex;
            }
                       
        }

        public Department SelectDepartment(int id)
        {
            using (var ctx = new BankDBContext())
            {
                return ctx.Departments.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public void UpdateAccount(Account account)
        {
            using (var ctx = new BankDBContext())
            {
                var acc = ctx.Accounts.Where(x => x.Id == account.Id).FirstOrDefault();
                acc.Balance = account.Balance;
                acc.ClientId = account.ClientId;
                acc.CreateDate = account.CreateDate;
                ctx.SaveChanges();
            }
        }

        public void UpdateClient(Client client)
        {
            using (var ctx = new BankDBContext())
            {
                var cl = ctx.Clients.Where(x => x.Id == client.Id).FirstOrDefault();
                cl.DepartmentId = client.DepartmentId;
                cl.Name = client.Name; 
                ctx.SaveChanges();
            }
        }

        public void UpdateDepartment(Department department)
        {
            using (var ctx = new BankDBContext())
            {
                var dep = ctx.Departments.Where(x => x.Id == department.Id).FirstOrDefault();
                dep.Name = department.Name;
                dep.ParentId = department.ParentId;
                ctx.SaveChanges();
            }
        }
    }
}
