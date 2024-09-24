using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using VRC.SDKBase;
using UnityEditor.UIElements;



namespace SweetForest.LazyToggleObjectMA.Editor
{

    [CustomEditor(typeof(Components.LazyToggleObjectMAInstaller))]
    [CanEditMultipleObjects]
    public class LazyToggleObjectMAEditor : UnityEditor.Editor
    {

        public VisualTreeAsset VisualTree;
        private PropertyField _ToggleExistBooleanParameter;
        private Button _groupIconEditorButton;

        private VisualElement _GenerateElement;
        private VisualElement _InputExistParameterElement;
        private SerializedProperty _BoolProperty;

        private void OnEnable() {
            _BoolProperty = serializedObject.FindProperty("UseExistParameter");
        }
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            // add UI builder
            VisualTree.CloneTree(root);

            _GenerateElement = root.Q<VisualElement>("GenerateElement");
            _InputExistParameterElement = root.Q<VisualElement>("InputExistParameterElement");

            _ToggleExistBooleanParameter = root.Q<PropertyField>("ToggleExistBooleanParameter");
            _ToggleExistBooleanParameter.RegisterCallback<ChangeEvent<bool>>(OnBooleanChageEvent);
            _ToggleExistBooleanParameter.RegisterCallback<ClickEvent>(OnClickToggleUseExistParameter);

            _groupIconEditorButton = root.Q<Button>("GroupIconEditor");
            _groupIconEditorButton.RegisterCallback<ClickEvent>(OnClickOpenGroupIconEditorButton);

            UpdateDisplayExistBooleanParameter();
            return root;
        }
        private void OnClickToggleUseExistParameter(ClickEvent e) {
            var t = (Components.LazyToggleObjectMAInstaller)target;
           if(!t.CanToggleToGenerateAnimation()) {
            t.UseExistParameter = true;
            e.PreventDefault();
            e.StopImmediatePropagation();
            e.StopPropagation();
            EditorUtility.DisplayDialog("Lazy Toggle Object MA","Cannot add multiple generating toggle components. Only one generating component is allowed per GameObject.","OK");
            }
        }
        private void OnBooleanChageEvent(ChangeEvent<bool> e)
        {
            
            UpdateDisplayExistBooleanParameter();
        }
        private void UpdateDisplayExistBooleanParameter()
        {
           _InputExistParameterElement.style.display = _BoolProperty.boolValue?DisplayStyle.Flex:DisplayStyle.None;
            _GenerateElement.style.display = _BoolProperty.boolValue?DisplayStyle.None:DisplayStyle.Flex;
                
        }
        private void OnClickOpenGroupIconEditorButton(ClickEvent e)
        {



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
                if (currentParent.GetComponent<VRC_AvatarDescriptor>() != null) break;
            }

            return currentParent.gameObject; // Return the topmost parent GameObject
        }
    }
}