using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Homework_18.Model
{
    public class Deposit : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsWithCapitalization { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }


        private decimal balance;
        public decimal Balance
        {
            get
            {
                if(IsWithCapitalization)
                {
                    double percentRate = 0.12;
                    int part = 12;
                    int monthsPassed = MonthDifference(DateTime.Now, CreateDate);
                    return balance * (decimal)Math.Pow(1 + percentRate / part, part * monthsPassed / part);
                }
                else
                {
                    if (MonthDifference(DateTime.Now, CreateDate) < 12)
                    {
                        return balance;
                    }
                    return balance + balance * 0.12m;
                }
            }
            set
            {
                this.balance = value;
                OnPropertyChanged();
            }
        }

        private int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
