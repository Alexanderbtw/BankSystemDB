using Homework_18.Exception;
using Homework_18.Helper;
using Homework_18.Model;
using Homework_18.Model.Event;
using Homework_18.View;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Homework_18.Extensions;
using Homework_18.Services;
using Homework_18.TemplateMethod;

namespace Homework_18.ViewModel
{
    class MainWindowViewModel : BaseViewModel
    {
        #region fields
        private Department selectedDepartment;
        private ObservableCollection<Department> departments;
        private Client selectedClient;
        private ObservableCollection<Client> clientsInDepartment;
        private Account account;
        private ObservableCollection<Deposit> deposites;
        private Node selectedNode;
        private ObservableCollection<Node> nodes;
        private DBManager dBmanager;
        #endregion

        #region properties
        public Department SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                selectedDepartment = value;
                //ClientsInDepartment = ClientService.GetClientsInDepartment(selectedDepartment);
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Department> Departments
        {
            get { return departments; }
            set
            {
                departments = value;
                OnPropertyChanged();
            }
        }
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                Account = dBmanager.SelectAccount(SelectedClient?.Id ?? 0);
                Deposits = dBmanager.SelectClientDeposits(SelectedClient?.Id ?? 0);
                if (SelectedClient != null && Account == null)
                {
                    throw new СlientHasNoAccountException();
                }

                OnPropertyChanged();
            }
        }
        public ObservableCollection<Client> ClientsInDepartment
        {
            get { return clientsInDepartment; }
            set
            {
                clientsInDepartment = value;
                OnPropertyChanged();
            }
        }
        public Account Account
        {
            get { return account; }
            set
            {
                account = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Deposit> Deposits
        {
            get { return deposites; }
            set
            {
                deposites = value;
                OnPropertyChanged();
            }
        }
        public Node SelectedNode
        {
            get { return selectedNode; }
            set
            {
                selectedNode = value;
                SelectedDepartment = Departments.Where(x => x.Id == SelectedNode.Id).FirstOrDefault();
                ClientsInDepartment = dBmanager.GetClientsInDepartment(selectedDepartment);
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Node> Nodes
        {
            get { return nodes; }
            set
            {
                nodes = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region constructor
        public MainWindowViewModel()
        {
            dBmanager = new DBManager();
            Departments = dBmanager.GetAllDepartments();

            Nodes = GetTreeViewNodes();
            Account.Notify += Account_Notify;
            Account.Notify += Logging;
        }
        #endregion

        #region commands
        private RelayCommand addDepartment;
        private RelayCommand editDepartment;
        private RelayCommand removeDepartment;
        private RelayCommand addClient;
        private RelayCommand editClient;
        private RelayCommand removeClient;
        private RelayCommand addDeposit;
        private RelayCommand addChildDepartment;
        private RelayCommand sendTo;

        public RelayCommand AddDepartment
        {
            get
            {
                return addDepartment ??= new RelayCommand(obj =>
                    {
                        Nodes ??= new ObservableCollection<Node>();

                        Department newDepartment = new Department();
                        DepartmentWindow departmentWindow = new DepartmentWindow()
                        {
                            DataContext = new DepartmentViewModel(newDepartment)
                        };

                        departmentWindow.Owner = obj as Window;
                        departmentWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        departmentWindow.ShowDialog();

                        if (departmentWindow.DialogResult.Value)
                        {
                            dBmanager.InsertDepartment(newDepartment);
                            Departments.Add(newDepartment);
                            Nodes.Add(new Node(newDepartment.Id, newDepartment.Name));
                        }
                    });
            }
        }
        public RelayCommand EditDepartment
        {
            get
            {
                return editDepartment ??= new RelayCommand(obj =>
                    {
                        Department updatedDepartment = SelectedDepartment;
                        DepartmentWindow departmentWindow = new DepartmentWindow()
                        {
                            DataContext = new DepartmentViewModel(updatedDepartment)
                        };

                        departmentWindow.Owner = obj as Window;
                        departmentWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        departmentWindow.ShowDialog();
                        Debug.WriteLine($"Результат закрытия диалогового окна: {departmentWindow.DialogResult}");

                        if (departmentWindow.DialogResult.Value)
                        {
                            dBmanager.UpdateDepartment(updatedDepartment);
                            SelectedNode.Id = updatedDepartment.Id;
                            SelectedNode.Name = updatedDepartment.Name;
                        }
                    },
                    obj => SelectedDepartment != null);
            }
        }
        public RelayCommand AddChildDepartment
        {
            get
            {
                return addChildDepartment ??= new RelayCommand(obj =>
                    {
                        Department newChildDepartment = new Department()
                        {
                            ParentId = SelectedDepartment.Id
                        };

                        DepartmentWindow departmentWindow = new DepartmentWindow()
                        {
                            DataContext = new DepartmentViewModel(newChildDepartment)
                        };

                        departmentWindow.Owner = obj as Window;
                        departmentWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        departmentWindow.ShowDialog();

                        if (departmentWindow.DialogResult.Value)
                        {
                            dBmanager.InsertDepartment(newChildDepartment);
                            Departments.Add(newChildDepartment);
                            SelectedNode.Nodes.Add(new Node(newChildDepartment.Id, newChildDepartment.Name));
                        }
                    },
                    obj => SelectedDepartment != null);
            }
        }
        public RelayCommand RemoveDepartment
        {
            get
            {
                return removeDepartment ??= new RelayCommand(obj =>
                    {
                        dBmanager.DeleteDepartment(SelectedDepartment);
                        Departments.Remove(SelectedDepartment);
                        ObservableCollection<Node> parentNode = Nodes.GetParentNode(SelectedNode);
                        parentNode.Remove(SelectedNode);
                        ClientsInDepartment = null; //Для обновления отображения пустого списка клиентов
                    },
                    obj => SelectedDepartment != null);
            }
        }
        public RelayCommand AddClient
        {
            get
            {
                return addClient ??= new RelayCommand(obj =>
                    {
                        Client newClient = new Client();

                        ClientWindow clientWindow = new ClientWindow()
                        {
                            DataContext = new ClientViewModel(newClient)
                        };

                        clientWindow.Owner = obj as Window;
                        clientWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        clientWindow.ShowDialog();

                        if (clientWindow.DialogResult.Value)
                        {
                            dBmanager.InsertClient(newClient);
                            dBmanager.InsertAccount(new Account()
                            {
                                ClientId = newClient.Id,
                                Balance = 10000, //Открываем счет с балансом 10000р по умолчанию
                                CreateDate = DateTime.Now
                            });
                            if(SelectedDepartment != null)
                            {
                                ClientsInDepartment = dBmanager.GetClientsInDepartment(SelectedDepartment);
                            }

                            MyAbstractClass myAbstractClass = new FirstConcreteClass();
                            myAbstractClass.TemplateMethod();
                        }

                    });
            }
        }
        public RelayCommand EditClient
        {
            get
            {
                return editClient ??= new RelayCommand(obj =>
                    {
                        Client updatedClient = SelectedClient;

                        ClientWindow clientWindow = new ClientWindow()
                        {
                            DataContext = new ClientViewModel(updatedClient)
                        };

                        clientWindow.Owner = obj as Window;
                        clientWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        clientWindow.ShowDialog();

                        if (clientWindow.DialogResult.Value)
                        {
                            dBmanager.UpdateClient(updatedClient);
                            ClientsInDepartment = dBmanager.GetClientsInDepartment(selectedDepartment);
                        }
                    },
                    obj => SelectedClient != null);
            }
        }
        public RelayCommand RemoveClient
        {
            get
            {
                return removeClient ??= new RelayCommand(obj =>
                    {
                        dBmanager.DeleteClient(SelectedClient);
                        ClientsInDepartment = dBmanager.GetClientsInDepartment(SelectedDepartment);
                        //TODO: Добавить удаление счетов и депозитов клиента
                        MyAbstractClass myAbstractClass = new SecondConcreteClass();
                        myAbstractClass.TemplateMethod();
                    },
                    obj => SelectedClient != null);
            }
        }
        public RelayCommand AddDeposit
        {
            get
            {
                return addDeposit ??= new RelayCommand(obj =>
                    {
                        Deposit newDeposit = new Deposit();
                        newDeposit.ClientId = SelectedClient.Id;
                        OpenDepositWindow openDepositWindow = new OpenDepositWindow()
                        {
                            DataContext = new OpenDepositViewModel(newDeposit, account)
                        };

                        openDepositWindow.Owner = obj as Window;
                        openDepositWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        openDepositWindow.ShowDialog();

                        if (openDepositWindow.DialogResult.Value)
                        {
                            dBmanager.InsertDeposit(newDeposit);
                            dBmanager.UpdateAccount(account);
                            Deposits = dBmanager.SelectClientDeposits(SelectedClient?.Id ?? 0);
                            Account = dBmanager.SelectAccount(SelectedClient?.Id ?? 0);
                        }
                    },
                    obj => SelectedClient != null);
            }
        }
        public RelayCommand SendTo
        {
            get
            {
                return sendTo ??= new RelayCommand(obj =>
                    {
                        Account = dBmanager.SelectAccount(SelectedClient?.Id ?? 0);
                        TransferBetweenAccountsWindow transferBetweenAccountsWindow = new TransferBetweenAccountsWindow()
                        {
                            DataContext = new TransferBetweenAccountsViewModel(Account)
                        };

                        transferBetweenAccountsWindow.Owner = obj as Window;
                        transferBetweenAccountsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        transferBetweenAccountsWindow.ShowDialog();

                        if (transferBetweenAccountsWindow.DialogResult.Value)
                        {
                            dBmanager.UpdateAccount(Account);
                        }
                    },
                    obj => SelectedClient != null);
            }
        }
        #endregion

        #region Приватные методы

        private void Logging(object sender, AccountEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("log.txt", true))
            {
                sw.WriteLine(DateTime.Now + " " + e.Message);
            }
        }

        private void Account_Notify(object sender, AccountEventArgs e)
        {
            Debug.Print(e.Message);

            Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                Window mainWindow = Application.Current.MainWindow;
                NotificationWindow alarm = new NotificationWindow()
                {
                    DataContext = new NotificationWindowViewModel()
                    {
                        Message = e.Message
                    }
                };
                alarm.Owner = mainWindow;
                alarm.Left = mainWindow.Left + mainWindow.Width - alarm.Width - 8;
                alarm.Top = mainWindow.Top + mainWindow.Height - alarm.Height - 8;
                alarm.Show();
                await Task.Delay(2000);
                alarm.Close();
            });
        }

        /// <summary>
        /// Формируем узлы для TreeView из департаментов
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        private ObservableCollection<Node> GetTreeViewNodes(Department department = null)
        {
            ObservableCollection<Node> nodes = new ObservableCollection<Node>();

            if (department == null)
            {
                var rootDep = Departments.Where(x => x.ParentId == 0).ToList();
                rootDep.ForEach(e =>
                {
                    var node = new Node(e.Id, e.Name);
                    node.Nodes = GetTreeViewNodes(e);
                    nodes.Add(node);
                });
                return nodes;
            }
            else
            {
                var subDep = Departments.Where(x => x.ParentId == department.Id).ToList();
                subDep.ForEach(e =>
                {
                    var node = new Node(e.Id, e.Name);
                    node.Nodes = GetTreeViewNodes(e);
                    nodes.Add(node);
                });
                return nodes;
            }
        }

        #endregion
    }
}
