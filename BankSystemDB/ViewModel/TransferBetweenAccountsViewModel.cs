using Homework_18.Helper;
using Homework_18.Model;
using Homework_18.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Homework_18.ViewModel
{
    class TransferBetweenAccountsViewModel : BaseViewModel, IDataErrorInfo
    {

        #region fields
        private string amount;
        private Client selectedRecipient;
        private Account senderAccount;
        private IEnumerable<Client> recipients;
        private string errorMessage;
        private RelayCommand confirmCommand;
        private DBManager dBmanager;
        #endregion

        #region constructors
        public TransferBetweenAccountsViewModel(Account senderAccount)
        {
            dBmanager = new DBManager();
            SenderAccount = senderAccount;
            Recipients = dBmanager.GetAllClients().Where(x => x.Id != SenderAccount.ClientId);
        }
        #endregion

        #region properties
        public string Amount
        {
            get { return amount; }
            set
            {
                this.amount = value;
                OnPropertyChanged();
            }
        }
        public Client SelectedRecipient
        {
            get { return selectedRecipient; }
            set
            {
                this.selectedRecipient = value;
                OnPropertyChanged();
            }
        }
        public Account SenderAccount
        {
            get { return senderAccount; }
            set
            {
                this.senderAccount = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<Client> Recipients
        {
            get { return recipients; }
            set
            {
                this.recipients = value;
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
        #endregion

        #region commands

        public RelayCommand ConfirmCommand
        {
            get
            {
                return confirmCommand ??
                    (confirmCommand = new RelayCommand(obj =>
                    {
                        Account recipientAccount = dBmanager.SelectAccount(SelectedRecipient.Id);
                        decimal amountDecimal = decimal.Parse(Amount);
                        senderAccount.Balance -= amountDecimal;
                        recipientAccount.Balance += amountDecimal;
                        dBmanager.UpdateAccount(recipientAccount);

                        Window window = obj as Window;
                        window.DialogResult = true;
                        window.Close();
                    },
                    obj => string.IsNullOrEmpty(ErrorMessage) && !string.IsNullOrEmpty(Amount) && SelectedRecipient != null));
            }
        }

        #endregion

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

                                if (result > SenderAccount.Balance)
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
