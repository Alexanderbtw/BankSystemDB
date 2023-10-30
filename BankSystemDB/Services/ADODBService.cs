using Homework_18.Model;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace Homework_18.Services
{
    class ADODBService : IDBService
    {
        private string connectionString;
        public ADODBService()
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory);
            connectionString = ConfigurationManager.ConnectionStrings["DBConnectionServer"].ConnectionString;
            //Debug.WriteLine($"\tСтрока подключения: {connectionString}");
        }

        public Account SelectAccount(int clientId)
        {
            string sqlExpression = @"SELECT * FROM Accounts
                                   WHERE ClientId = @ClientId";

            Account account = new Account();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add("@ClientId", SqlDbType.Int).Value = clientId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Debug.WriteLine("Accounts");
                            Debug.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}\t{reader.GetName(3)}");

                            while (reader.Read())
                            {
                                Debug.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}\t{reader.GetValue(3)}");
                                account.Id = reader.GetInt32(0);
                                account.ClientId = reader.GetInt32(1);
                                account.Balance = reader.GetDecimal(2);
                                account.CreateDate = reader.GetDateTime(3);
                            }
                        }
                        else
                        {
                            account = null;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            return account;
        }
        public void InsertAccount(Account account)
        {
            string sqlExpression = @"INSERT INTO Accounts (ClientId,  Balance, CreateDate) 
                                   VALUES (@ClientId, @Balance, @CreateDate);
                                   SET @Id = @@IDENTITY;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(idParam);
                    command.Parameters.Add("@ClientId", SqlDbType.Int).Value = account.ClientId;
                    command.Parameters.Add("@Balance", SqlDbType.Decimal).Value = account.Balance;
                    command.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = account.CreateDate;

                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Добавлено счетов: {number}");
                    Debug.WriteLine($"\tId нового счета: {idParam.Value}");

                    account.Id = (int)idParam.Value;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public void UpdateAccount(Account account)
        {
            string sqlExpression = @"UPDATE Accounts SET 
                                   ClientId = @ClientId, 
                                   Balance = @Balance,
                                   CreateDate = @CreateDate
                                   WHERE Id = @Id";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = account.Id;
                    command.Parameters.Add("@ClientId", SqlDbType.Int).Value = account.ClientId;
                    command.Parameters.Add("@Balance", SqlDbType.Decimal).Value = account.Balance;
                    command.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = account.CreateDate;

                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Изменено счетов: {number}");
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public void DeleteAccount(Account account)
        {
            string sqlExpression = @"DELETE FROM Accounts
                                   WHERE Id = @Id";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = account.Id;
                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Удалено счетов: {number}");
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }

        }

        public ObservableCollection<Client> GetAllClients()
        {
            string sqlExpression = @"SELECT * FROM Clients";
            ObservableCollection<Client> clients = new ObservableCollection<Client>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            Debug.WriteLine("Clients");
                            Debug.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

                            while (reader.Read()) // построчно считываем данные
                            {
                                Debug.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");

                                clients.Add(new Client()
                                {
                                    Id = reader.GetInt32(0),
                                    DepartmentId = reader.GetInt32(1),
                                    Name = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return clients;
        }
        public ObservableCollection<Client> GetClientsInDepartment(Department department)
        {
            string sqlExpression = @"SELECT * FROM Clients
                                   WHERE DepartmentId = @DepartmentId";

            ObservableCollection<Client> clients = new ObservableCollection<Client>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = department.Id;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Debug.WriteLine("Clients");
                            Debug.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

                            while (reader.Read())
                            {
                                Debug.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");

                                clients.Add(new Client()
                                {
                                    Id = reader.GetInt32(0),
                                    DepartmentId = reader.GetInt32(1),
                                    Name = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return clients;
        }
        public void InsertClient(Client client)
        {
            string sqlExpression = @"INSERT INTO Clients (DepartmentId,  Name) 
                                   VALUES (@DepartmentId, @Name);
                                   SET @Id = @@IDENTITY;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(idParam);
                    command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = client.DepartmentId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = client.Name;

                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Добавлено клиентов: {number}");
                    Debug.WriteLine($"\tId нового клиента: {idParam.Value}");

                    client.Id = (int)idParam.Value;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public void UpdateClient(Client client)
        {
            string sqlExpression = @"UPDATE Clients SET 
                           DepartmentId = @DepartmentId, 
                           Name = @Name 
                           WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = client.Id;
                    command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = client.DepartmentId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = client.Name;

                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Изменено департаментов: {number}");
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
        public void DeleteClient(Client client)
        {
            string sqlExpression = @"DELETE FROM Clients
                                   WHERE Id = @Id";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = client.Id;
                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Удалено клиентов: {number}");
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<Department> GetAllDepartments()
        {
            string sqlExpression = "SELECT * FROM Departments";
            ObservableCollection<Department> departments = new ObservableCollection<Department>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    Debug.WriteLine("Подключение открыто");

                    Debug.WriteLine("Свойства подключения:");
                    Debug.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
                    Debug.WriteLine($"\tСтрока подключения: {connectionString}");
                    Debug.WriteLine($"\tБаза данных: {connection.Database}");
                    Debug.WriteLine($"\tСервер: {connection.DataSource}");
                    Debug.WriteLine($"\tВерсия сервера: {connection.ServerVersion}");
                    Debug.WriteLine($"\tСостояние: {connection.State}");
                    Debug.WriteLine($"\tWorkstationld: {connection.WorkstationId}");

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            Debug.WriteLine("Departments");
                            Debug.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

                            while (reader.Read()) // построчно считываем данные
                            {
                                Debug.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");

                                departments.Add(new Department()
                                {
                                    Id = reader.GetInt32(0),
                                    ParentId = reader.GetInt32(1),
                                    Name = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return departments;
        }
        public Department SelectDepartment(int id)
        {
            string sqlExpression = @"SELECT * FROM Departments
                                   WHERE Id = @Id";
            Department department = new Department();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Debug.WriteLine("Departments");
                            Debug.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

                            while (reader.Read())
                            {
                                Debug.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}");

                                department.Id = reader.GetInt32(0);
                                department.ParentId = reader.GetInt32(1);
                                department.Name = reader.GetString(2);
                            }
                        }
                        else
                        {
                            department = null;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return department;
        }
        public void InsertDepartment(Department department)
        {
            string sqlExpression = @"INSERT INTO Departments (ParentId,  Name) 
                                 VALUES (@ParentId, @Name);
                                SET @Id = @@IDENTITY;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output // параметр выходной
                    };

                    command.Parameters.Add(idParam);
                    command.Parameters.Add("@ParentId", SqlDbType.Int).Value = department.ParentId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = department.Name;

                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Добавлено департаментов: {number}");
                    Debug.WriteLine($"\tId нового департамента: {idParam.Value}");

                    department.Id = (int)idParam.Value;
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }

        }
        public void UpdateDepartment(Department department)
        {
            string sqlExpression = @"UPDATE Departments SET 
                                   ParentId = @ParentId, 
                                   Name = @Name 
                                   WHERE Id = @Id";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = department.Id;
                    command.Parameters.Add("@ParentId", SqlDbType.Int).Value = department.ParentId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = department.Name;

                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Изменено департаментов: {number}");
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
        public void DeleteDepartment(Department department)
        {
            //Рекурсивное удаление всех вложенных департаментов
            string sqlExpression =
                @"WITH RecursiveQuery (Id, ParentId, Name)
                AS
                (
                SELECT Id, ParentId, Name
                FROM Departments dep
                WHERE dep.Id = @Id
                UNION ALL
                SELECT dep.Id, dep.ParentId, dep.Name
                FROM Departments dep
                JOIN RecursiveQuery rec ON dep.ParentId = rec.Id
                )
                
                DELETE FROM Departments
                WHERE Id in (SELECT Id From RecursiveQuery)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = department.Id;
                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Удалено департаментов: {number}");
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }

        public ObservableCollection<Deposit> SelectClientDeposits(int clientId)
        {
            string sqlExpression = @"SELECT * FROM Deposits
                                   WHERE ClientId = @ClientId";

            ObservableCollection<Deposit> deposits = new ObservableCollection<Deposit>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.Parameters.Add("@ClientId", SqlDbType.Int).Value = clientId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            Debug.WriteLine("Deposits");
                            Debug.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}\t{reader.GetName(3)}\t{reader.GetName(4)}\t{reader.GetName(5)}");

                            while (reader.Read())
                            {
                                Debug.WriteLine($"{reader.GetValue(0)}\t{reader.GetValue(1)}\t{reader.GetValue(2)}\t{reader.GetValue(3)}\t{reader.GetValue(4)}\t{reader.GetValue(5)}");

                                deposits.Add(new Deposit()
                                {
                                    Id = reader.GetInt32(0),
                                    ClientId = reader.GetInt32(1),
                                    Name = reader.GetString(2),
                                    Balance = reader.GetDecimal(3),
                                    CreateDate = reader.GetDateTime(4),
                                    IsWithCapitalization = reader.GetBoolean(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return deposits;
        }
        public void InsertDeposit(Deposit deposit)
        {

            string sqlExpression = @"INSERT INTO Deposits (ClientId,  Name, Balance, CreateDate, IsWithCapitalization) 
                                 VALUES (@ClientId, @Name, @Balance, @CreateDate, @IsWithCapitalization);
                                SET @Id = @@IDENTITY;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(idParam);
                    command.Parameters.Add("@ClientId", SqlDbType.Int).Value = deposit.ClientId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = deposit.Name;
                    command.Parameters.Add("@Balance", SqlDbType.Decimal).Value = deposit.Balance;
                    command.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = deposit.CreateDate;
                    command.Parameters.Add("@IsWithCapitalization", SqlDbType.Bit).Value = deposit.IsWithCapitalization;

                    int number = command.ExecuteNonQuery();

                    Debug.WriteLine($"Добавлено департаментов: {number}");
                    Debug.WriteLine($"\tId нового департамента: {idParam.Value}");

                    deposit.Id = (int)idParam.Value;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
