using System.Collections.Generic;
using UnityEngine;

namespace SweetForest.LazyToggleObjectMA.Utilities.LazyHierachies
{
    [System.Serializable]
    public class LazyHierarchyNode<T>
    {
        public string Namespace;
        public string NamespacePath;
        public List<T> Values; // Change from T Value to List<T> Values
        private Dictionary<string,int> DuplicatedObjectName;
        private Dictionary<T,int> ValueIndexs;
        public Dictionary<string, LazyHierarchyNode<T>> Children;

        // Constructor
        public LazyHierarchyNode(string namespaceName, string namespacePath)
        {
            Namespace = namespaceName;
            NamespacePath = namespacePath;
            Values = new List<T>(); // Initialize the list
            Children = new Dictionary<string, LazyHierarchyNode<T>>();
            DuplicatedObjectName = new Dictionary<string, int>();
            ValueIndexs = new Dictionary<T, int>();
        }

        // Method to add a value to this node
        // T must be gameobject
        public void AddValue(T value,string name)
        {
       
            if(DuplicatedObjectName.ContainsKey(name)) {
                DuplicatedObjectName[name]++;
            } else {
                DuplicatedObjectName[name] = 0;
            }
            Values.Add(value); // Add the value to the list
            ValueIndexs.Add(value,DuplicatedObjectName[name]);
            
        }
        
         public int GetIndexOfValue(T t)
        {
            
            return ValueIndexs.ContainsKey(t) ? ValueIndexs[t] : -1;
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
