using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public class Node : INotifyPropertyChanged
    {
        private List<AnalyticOrdered> _analytics = new List<AnalyticOrdered>();
        public List<AnalyticOrdered> Analytics { get => _analytics; set => _analytics = value; }
        public Visibility ContainsAnalytic { get; set; }
        public Visibility invertedContainsAnalytic
        {
            get
            {
                return ContainsAnalytic == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                selectChild(value, this);
                
            }
        }
        public string StructureName { get; set; }
        private ObservableCollection<Node> _childNodes = new ObservableCollection<Node>();
        private int _childCount = 0;
        public int ChildCount { get => _childCount; set => _childCount = value; }
        public ObservableCollection<Node> Nodes
        {
            get
            {
                return _childNodes;
            }
            set
            {
                _childNodes = value;
                OnPropertyChanged(nameof(Nodes));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Node() { }
        public Node(string Name)
        {
            StructureName = Name;

        }
        public override string ToString()
        {
            return $"{StructureName}. Analytics: {Analytics.Count}. ChildNodes: {Nodes.Count}";
        }
        public int CountAnalytics(Node node)
        {
            int i = 0;
            i = i + node.Analytics.Count;

            if (node.Analytics.Count > 0)
            {
                node.ContainsAnalytic = Visibility.Visible;
            }
            else
            {
                node.ContainsAnalytic = Visibility.Collapsed;
            }

            foreach (Node childNode in node.Nodes)
            {
                i = i + CountAnalytics(childNode);
            }
            node.ChildCount = i;
            OnPropertyChanged("ChildCount");
            return i;
        }
        public void AddChild(Node node)
        {
            if (Nodes != null)
                Nodes.Add(node);
            else
            {
                Nodes = new ObservableCollection<Node>();
                Nodes.Add(node);
            }
        }

        public static Node FindNode(string name, IEnumerable<Node> parentCollection)
        {
            Node foundedNode = null;
            foreach (Node parentNode in parentCollection)
            {
                foundedNode = FindNodeByName(name, parentNode);
                if (foundedNode != null) break;
            }
            return foundedNode;
        }
        private static Node FindNodeByName(string name, Node parent)
        {
            if (parent.StructureName.Equals(name)) return parent;

            if (parent.Nodes == null) return null;

            foreach (Node node in parent.Nodes)
            {
                Node node1 = FindNodeByName(name, node);
                if (node1 != null) return node1;
            }
            return null;
        }
        public static void PrintNodes(Node node)
        {
            if (node.Nodes != null)
            {
                foreach (Node childNode in node.Nodes)
                {
                    PrintNodes(childNode);
                }
            }
        }
        private void selectChild(bool val, Node node)
        {
            node.isSelected = val;
            node.OnPropertyChanged("IsSelected");
            foreach (Node childNode in node.Nodes)
            {
                selectChild(val, childNode);

            }
            foreach (AnalyticOrdered analytic in node.Analytics)
            {
                analytic.Selected = val;
            }
        }

    }
}