
using UnityEngine;
using VRC.SDKBase;


namespace SweetForest.LazyToggleObjectMA.Components
{



    [ExecuteInEditMode]
    [AddComponentMenu("Lazy Toggle Object MA")]
    [DisallowMultipleComponent]
    public class LazyToggleObjectMAInstaller : MonoBehaviour, IEditorOnly
    {

        //[Header("Namespace Settings")]
        [Tooltip("Grouping menu using the specified namespace")]
        public string NamespaceGroup;

        [Tooltip("Custom name for the toggle button")]
        public string CustomNameButton;

        //[Space(5)]
        // [Header("Parameter Settings")]
        [Tooltip("Default value of the parameter (true/false)")]
        public bool DefaultValue = false;

        [Tooltip("Should the parameter be saved?")]
        public bool SavedValue = true;

        [Tooltip("Is synchronization needed for this parameter?")]
        public bool Synced = true;

        [Tooltip("Should this parameter be local only (not synced across clients)?")]
        public bool LocalOnly = false;

        //[Space(5)]
        // [Header("Icon Settings")]
        [Tooltip("Icon for the toggle button")]
        public Texture2D IconButton;




        public string getParameter()
        {
            return "lazy_toggle_object_ma:" + NamespaceGroup + ":" + gameObject.GetInstanceID();
        }
        public string getNameButton()
        {
            return CustomNameButton != "" ? CustomNameButton : gameObject.name;
        }
    }

}