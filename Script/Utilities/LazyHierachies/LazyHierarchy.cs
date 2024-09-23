using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetForest.LazyToggleObjectMA.Utilities.LazyHierachies
{
    public class LazyHierarchy<T>
    {
        public LazyHierarchyNode<T> Root; 
        private Dictionary<string, LazyHierarchyNode<T>> cachedNodes; 

        // Constructor
        // Constructor without a specific root namespace
        public LazyHierarchy() 
        {
            Root = new LazyHierarchyNode<T>("",""); // Set a generic root
            cachedNodes = new Dictionary<string, LazyHierarchyNode<T>>();
            cachedNodes[""] = Root; // Cache the root
        }


        // Method to add a value to the hierarchy
      public void Add(string path, T value)
{

    if (path == "") // add value into root
    {

        Root.AddValue(value);
       // cachedNodes[""] = Root; no root is root is not inside root
        return; 
    }

    string[] namespaces = path.Split('/'); // O(m)
    var currentNode = Root;
    string currentPath = ""; // Track current path level by level

    foreach (var ns in namespaces) // O(m)
    {
        // Build the current path
        if (string.IsNullOrEmpty(currentPath))
            currentPath = ns;
        else
            currentPath += "/" + ns;

        // Check if child node exists
        if (!currentNode.Children.TryGetValue(ns, out var childNode)) // O(1)
        {
            // If not, create a new one
            childNode = new LazyHierarchyNode<T>(ns, currentPath);  // Fix NamespacePath to use currentPath
            currentNode.Children[ns] = childNode; // O(1)
        }

        // Cache the current path
        if (!cachedNodes.ContainsKey(currentPath))
            cachedNodes[currentPath] = childNode;

        currentNode = childNode; // Move to the child node
    }

    currentNode.AddValue(value); // Add the value at the final node
    cachedNodes[path] = currentNode; // Cache the node
}




        // Method to retrieve a node from the cache
        public LazyHierarchyNode<T> GetNode(string path) // O(1)
        {
            cachedNodes.TryGetValue(path, out var node);
            return node; // Returns null if not found
        }
        
     public LazyHierarchyNode<T> GetParent(string path) 
{
    if (!path.Contains("/")) 
    {
        return null; // No parent if there's no "/"
    }

    // O(n) - We will iterate backwards to find the last '/' index
    int lastIndex = path.Length - 1;
    while (lastIndex >= 0 && path[lastIndex] != '/')
    {
        lastIndex--; // Move backwards until we find '/'
    }

    // If lastIndex is -1, it means there's no '/' which shouldn't happen due to the previous check.
    string parentPath = path.Substring(0, lastIndex); // Still using substring, but we may refine this.

    // O(1) - Fetching from the dictionary.
    cachedNodes.TryGetValue(parentPath, out var parentNode);
    return parentNode; // Returns null if not found.
}
public List<LazyHierarchyNode<T>> GetSiblings(string path)
{
    // Get the parent node first
    var parentNode = GetParent(path);
    if (parentNode == null)
    {
        return new List<LazyHierarchyNode<T>>(); // No siblings if there's no parent
    }

    // Find the current node
    if (!cachedNodes.TryGetValue(path, out var currentNode))
    {
        return new List<LazyHierarchyNode<T>>(); // Return empty if the current node doesn't exist
    }

    // O(1) - Fetch siblings by excluding the current node
    var siblings = new List<LazyHierarchyNode<T>>();
    foreach (var child in parentNode.Children.Values)
    {
        if (child != currentNode) // Exclude the current node
        {
            siblings.Add(child);
        }
    }

    return siblings; // Return the list of sibling nodes
}

public List<LazyHierarchyNode<T>> GetTopNamespaceNodes() {
    List<LazyHierarchyNode<T>> list = new List<LazyHierarchyNode<T>>();

    foreach (var item in cachedNodes)
    {
        if(!item.Key.Contains("/")) {
            list.Add(item.Value);
        }
    }
    return list;
}

public List<T> GetAllValues()
{
    List<T> values = new List<T>();
    GetAllValuesRecursively(Root, values);
    return values;
}

private void GetAllValuesRecursively(LazyHierarchyNode<T> node, List<T> values)
{

    foreach (var item in node.Values)
    {
         values.Add(item);
    }

    // Recursively call for each child node
    foreach (var child in node.Children.Values)
    {
        GetAllValuesRecursively(child, values);
    }
}

    public Dictionary<string, LazyHierarchyNode<T>> getNamespaces() {
        return cachedNodes;
    }

    public void PrintLogHierachy() {

        foreach (var kvp in cachedNodes)
        {
            string key = kvp.Key; 
            LazyHierarchyNode<T> value = kvp.Value; 

            Debug.Log($"Key: {key}, {value}");
        }
    }

    
    }




}
