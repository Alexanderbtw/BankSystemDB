using Homework_18.Helper;
using Homework_18.Model;
using System;
using System.ComponentModel;
using System.Windows;

namespace Homework_18.ViewModel
{
    class OpenDepositViewModel : BaseViewModel, IDataErrorInfo
    {
        private string amount;
        private bool isWithCapitalization;
        private string errorMessage;

        private Account account;
        public Account Account
        {
            get { return account; }
            set
            {
                this.account = value;
                OnPropertyChanged();
            }
        }

        private Deposit deposit;
        public Deposit Deposit
        {
            get { return deposit; }
            set
            {
                this.deposit = value;
                OnPropertyChanged();
            }
        }

        public string Amount
        {
            get { return amount; }
            set
            {
                this.amount = value;
                OnPropertyChanged();
            }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                this.errorMessage = value;
                OnPropertyChanged();
            }
        }

        public OpenDepositViewModel(Deposit deposit, Account account)
        {
            this.deposit = deposit;
            this.account = account;
        }

        private RelayCommand selectRadioButton;
        public RelayCommand SelectRadioButton
        {
            get
            {
                return selectRadioButton ??
                    (selectRadioButton = new RelayCommand(obj =>
                    {
                        isWithCapitalization = Boolean.Parse((string)obj);
                    }));
            }
        }

        private RelayCommand confirmCommand;
        public RelayCommand СonfirmCommand
        {
            get
            {
                return confirmCommand ??
                    (confirmCommand = new RelayCommand(obj =>
                    {
                        if (decimal.TryParse(Amount, out decimal result))
                        {
                            if (isWithCapitalization)
                            {
                                deposit.Name = "Вклад с капитализацией";   
                            }
                            else
                            {
                                deposit.Name = "Вклад без капитализации";
                            }
                            deposit.IsWithCapitalization = isWithCapitalization;
                            account.Balance -= result;
                            deposit.Balance = result;
                            deposit.CreateDate = DateTime.Now;

                            Window window = obj as Window;
                            window.DialogResult = true;
                            window.Close();
                        }


                    },
                    obj => string.IsNullOrEmpty(ErrorMessage) && !string.IsNullOrEmpty(Amount)));
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Amount":
                        if (!string.IsNullOrEmpty(Amount))
                        {
                            if (decimal.TryParse(amount, out decimal result))
                            {
                                if (result <= 0)
                                {
                                    error = "Сумма должна быть больше 0";
                                }

                                if (result > Account.Balance)
                                {
                                    error = "Сумма превышает сумму на счете списания";
                                }
                            }
                            else
                            {
                                error = "Введены недопустимые символы";
                            }
                        }
                        break;

                }
                ErrorMessage = error;
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

    }
}
