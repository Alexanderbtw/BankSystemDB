using Homework_18.Model;
using System.Collections.ObjectModel;

namespace Homework_18.Extensions
{
    public static class ObservableCollectionNodeExt
    {
        /// <summary>
        /// Получает коллекцию содержащую узел который необходимо удалить
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ObservableCollection<Node> GetParentNode(this ObservableCollection<Node> nodes, Node node)
        {
            ObservableCollection<Node> resultCollection = new ObservableCollection<Node>();

            if (nodes.Contains(node))
            {
                resultCollection = nodes;
            }
            else
            {
                foreach (var item in nodes)
                {
                    resultCollection = item.Nodes.GetParentNode(node);
                    if (resultCollection.Count != 0)
                    {
                        break;
                    }
                }
            }
            return resultCollection;
        }
    }
}
