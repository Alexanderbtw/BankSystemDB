using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Homework_18.Model
{
    public class Node : INotifyPropertyChanged
    {
        private string name;
        public int Id { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Node> Nodes { get; set; }

        public Node(int id, string name)
        {
            Nodes = new ObservableCollection<Node>();
            Id = id;
            Name = name;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
