using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SweetForest.LazyToggleObjectMA.Editor
{
    public class LazyToggleNamespaceIconEditor
    {
        public static GameObject rootGameObject;

        [MenuItem("GameObject/Lazy Toggle Namespace Icon Editor", false, 10)]
        private static void OpenEditor()
        {
            // Check if a GameObject is selected
            if (Selection.activeGameObject != null)
            {
                rootGameObject = Selection.activeGameObject;
                // Log the name of the selected GameObject
                Debug.Log("Selected GameObject: " + rootGameObject.name);
                // Open the custom editor window and pass the selected GameObject
                LazyToggleNamespaceWindow.ShowWindow(rootGameObject);
            }
            else
            {
                Debug.LogWarning("No GameObject selected!");
            }
        }

        // New menu item in the top menu bar
        [MenuItem("Tools/Lazy Toggle Object Menu/Namespace Icon Group Editor")]
        private static void OpenNamespaceIconGroupEditor()
        {
            // Logic for opening your editor window can go here
           // Debug.Log("Opening Namespace Icon Group Editor");
            // You can create a new window here if needed
            LazyToggleNamespaceWindow.ShowWindow(null); // Example if you want to open with no GameObject
        }

    }
}