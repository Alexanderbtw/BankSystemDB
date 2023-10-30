using Homework_18.Model.Event;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Homework_18.Model
{
    public class Account : INotifyPropertyChanged
    {
        public delegate void AccountHandler(object sender, AccountEventArgs e);
        public static event AccountHandler Notify;

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        private decimal balance;
        public decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        public bool SendTo(Account recipient, decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            if (amount > Balance)
            {
                return false;
            }

            Notify?.Invoke(this, new AccountEventArgs($"Со счета {this.Id} на счет {recipient.Id} переведена суммма {amount}", amount));
            recipient.Balance += amount;
            this.Balance -= amount;

            return true;
        }
    }
}
