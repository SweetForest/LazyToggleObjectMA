using System.Collections.Generic;

namespace SweetForest.LazyToggleObjectMA.Utilities.LazyHierachies
{
    [System.Serializable]
    public class LazyHierarchyNode<T>
    {
        public string Namespace;
        public string NamespacePath;
        public List<T> Values; // Change from T Value to List<T> Values
        public Dictionary<string, LazyHierarchyNode<T>> Children;

        // Constructor
        public LazyHierarchyNode(string namespaceName, string namespacePath)
        {
            Namespace = namespaceName;
            NamespacePath = namespacePath;
            Values = new List<T>(); // Initialize the list
            Children = new Dictionary<string, LazyHierarchyNode<T>>();
        }

        // Method to add a value to this node
        public void AddValue(T value)
        {
            Values.Add(value); // Add the value to the list
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Values.Count; i++)
            {
                s += Values[i].ToString() + (i + 1 < Values.Count ? ", " : "");
            }
            return "Values {" + s + "}";
        }
    }

}
