using System.Linq;
using SweetForest.LazyToggleObjectMA.Components;
using SweetForest.LazyToggleObjectMA.Utilities.LazyHierachies;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;

namespace SweetForest.LazyToggleObjectMA.Editor
{
    public class LazyToggleNamespaceWindow : EditorWindow
    {
        public GameObject targetGameObject;

        private GUIStyle alertStyle;
        private GUIStyle successStyle;

        private Vector2 scrollPosition; // Track scroll position

        public static void ShowWindow(GameObject gameObject)
        {
            LazyToggleNamespaceWindow window = GetWindow<LazyToggleNamespaceWindow>("Lazy Namespace Icon Group Editor");
            window.targetGameObject = gameObject;
            window.minSize = new Vector2(300, 100);
            window.Show();
        }

        private Texture2D CreateColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        private void OnEnable()
        {
            alertStyle = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = Color.white },
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            alertStyle.normal.background = CreateColorTexture(Color.red);

            successStyle = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = Color.black },
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            successStyle.normal.background = CreateColorTexture(Color.green);
        }

        private void OnGUI()
        {
            GUILayout.Label("Namespace Icon Group Editor", EditorStyles.boldLabel);

            targetGameObject = (GameObject)EditorGUILayout.ObjectField("Select Root Avatar", targetGameObject, typeof(GameObject), true);

            if (targetGameObject == null)
            {
                GUILayout.Label("You have to select Root Avatar First", alertStyle);
                return;
            }
            if (!targetGameObject.GetComponent<VRC_AvatarDescriptor>())
            {
                GUILayout.Label("No Avatar Descriptor! Please select VRChat Avatar.", alertStyle);
                return;
            }

            GUILayout.Label("Current Avatar: " + targetGameObject.name, successStyle);
            GUILayout.Space(20);

            // Load existing mappings
            var lazyHierarchy = new LazyHierarchy<LazyToggleObjectMAInstaller>();
            var list = targetGameObject.GetComponentsInChildren<Components.LazyToggleObjectMAInstaller>().ToList();
            foreach (var item in list)
            {
                lazyHierarchy.Add(item.NamespaceGroup, item,item.getNameButton());
            }

            var mappingData = targetGameObject.GetComponent<LazyNamespaceIconMappingData>();
            if (mappingData == null)
            {
                mappingData = targetGameObject.AddComponent<LazyNamespaceIconMappingData>();
            }



            var n = lazyHierarchy.getNamespaces();
            mappingData.SyncListToDictionary();
            foreach (var item in n)
            {
                if (!mappingData.GetCachedMappingViewer().ContainsKey(item.Key))
                {
                    mappingData.AddMapping(item.Key, null);
                    Debug.Log("LazyToggleObjectMA: found new icon added " + item.Key);

                }

            }
            // Scroll view for the namespace and texture editor
            GUILayout.Label("Lazy Namespace and Icon Mapping", EditorStyles.boldLabel);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(600));

            foreach (var kvp in mappingData.GetCachedMappingViewer().ToList())
            {
                if (string.IsNullOrEmpty(kvp.Key)) continue;
                if (!n.ContainsKey(kvp.Key)) continue;

                // Begin a group with a background color and outline
                GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f); // Light gray background
                GUILayout.BeginVertical("box"); // Create a box around each entry
                GUI.backgroundColor = Color.white; // Reset background color for inner elements

                GUILayout.BeginHorizontal();

                // Use a label with word wrap to allow for long namespace names
                var labelStyle = new GUIStyle(EditorStyles.label)
                {
                    wordWrap = true,
                    fixedHeight = 40,
                    richText = true
                };

                // Split the namespace into parts
                var parts = kvp.Key.Split('/');

                // Create the formatted label
                var formattedLabel = "<color=#808080>" + string.Join("/", parts.Take(parts.Length - 1)) + "/</color><color=white>" + parts.Last() + "</color>";

                GUILayout.Label(formattedLabel, labelStyle);

                // Check if the texture has changed
                var newTexture = (Texture2D)EditorGUILayout.ObjectField(kvp.Value, typeof(Texture2D), false, GUILayout.Width(100), GUILayout.Height(100));
                if (newTexture != kvp.Value)
                {
                    mappingData.AddMapping(kvp.Key, newTexture); // Update the dictionary with the new texture
                                                                 // EditorUtility.SetDirty(mappingData); // Mark the mapping data as dirty
                    Debug.Log("Update Mapping!");
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical(); // End the vertical group for the entry
            }

            GUILayout.EndScrollView(); // End the scroll view


        }


    }
}
