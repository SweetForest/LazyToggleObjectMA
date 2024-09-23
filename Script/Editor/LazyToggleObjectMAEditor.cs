using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;



namespace SweetForest.LazyToggleObjectMA.Editor
{

    [CustomEditor(typeof(Components.LazyToggleObjectMAInstaller))]
    [CanEditMultipleObjects]
public class LazyToggleObjectMAEditor : UnityEditor.Editor
{

    public VisualTreeAsset VisualTree;
    private Button _groupIconEditorButton;
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        // add UI builder
        VisualTree.CloneTree(root);

        _groupIconEditorButton = root.Q<Button>("GroupIconEditor");
        _groupIconEditorButton.RegisterCallback<ClickEvent>(OnClickOpenGroupIconEditorButton);
        return root;
    }

    private void OnClickOpenGroupIconEditorButton(ClickEvent e) {
       
        
    
        // open window
        Debug.Log("Open Namespace Icon Group Editor");
        // Get the GameObject from the target component
        Components.LazyToggleObjectMAInstaller script = (Components.LazyToggleObjectMAInstaller)target; // Cast target to the correct type
        GameObject gameObjectOfScript = script.gameObject; // Get the GameObject of the script
        // get root

        // Open the window and pass the GameObject
        LazyToggleNamespaceWindow.ShowWindow(GetTopmostParent(gameObjectOfScript));
    }

    // Function to get the topmost parent
    GameObject GetTopmostParent(GameObject obj)
    {
        Transform currentParent = obj.transform;

        // Keep climbing up the parent chain until there's no parent
        while (currentParent.parent != null)
        {
            currentParent = currentParent.parent;
        }

        return currentParent.gameObject; // Return the topmost parent GameObject
    }
}
}