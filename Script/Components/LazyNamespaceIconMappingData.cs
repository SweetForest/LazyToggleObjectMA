using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace SweetForest.LazyToggleObjectMA.Components
{
    [ExecuteInEditMode]
    [AddComponentMenu("Lazy Namespace Icon Mapping Data")]
    [DisallowMultipleComponent]
    public class LazyNamespaceIconMappingData : MonoBehaviour, IEditorOnly
    {
        [System.Serializable]
        public class NamespaceTextureMapping
        {
            public string namespacePath;
            public Texture2D texture;
        }

        // Use Dictionary instead of List for better performance
        private Dictionary<string, Texture2D> namespaceTextureDictionary = new Dictionary<string, Texture2D>();

        // If you still want to expose the list in the inspector for manual editing
        public List<NamespaceTextureMapping> mappings;

        // Converts the list to a dictionary for faster access
        private void Awake()
        {
            // Convert list to dictionary for optimized access
            if (mappings != null)
            {
                foreach (var mapping in mappings)
                {
                    if (!namespaceTextureDictionary.ContainsKey(mapping.namespacePath))
                    {
                        namespaceTextureDictionary[mapping.namespacePath] = mapping.texture;
                    }
                }
            }
        }

        // Retrieves the texture associated with the given namespace path using the dictionary
        public Texture2D GetTextureViewer(string namespacePath)
        {
            return namespaceTextureDictionary.ContainsKey(namespacePath) ? namespaceTextureDictionary[namespacePath] : null;

        }
        public Texture2D GetTexture(string namespacePath)
        {
            // Loop through the mappings list to find the corresponding texture
            foreach (var mapping in mappings)
            {
                if (mapping.namespacePath == namespacePath)
                {
                    return mapping.texture; // Return the texture if found
                }
            }

            // If not found, return null
            Debug.LogWarning("Texture not found for: " + namespacePath);
            return null;
        }

        // Removes the mapping associated with the given namespace path using the dictionary
        public void RemoveMapping(string namespacePath)
        {
            if (namespaceTextureDictionary.ContainsKey(namespacePath))
            {
                namespaceTextureDictionary.Remove(namespacePath);
            }
        }

        // Adds a new mapping or updates an existing one, allowing null textures
        public void AddMapping(string namespacePath, Texture2D texture = null)
        {
            // Update dictionary entry or add new mapping, allowing texture to be null
            namespaceTextureDictionary[namespacePath] = texture;

            // Sync back to the list for inspector viewing
            SyncDictToMapper();

            // Mark the object as dirty to save changes in the Editor
            //EditorUtility.SetDirty(this);

        }


        // Optionally, if you want to keep the list and sync changes back to it
        public void SyncListToDictionary() // laod?
        {
            foreach (var item in mappings)
            {
                namespaceTextureDictionary[item.namespacePath] = item.texture;
            }

        }
        public Dictionary<string, Texture2D> GetCachedMappingViewer()
        {
            return namespaceTextureDictionary;
        }
        public void SyncDictToMapper()
        {
            mappings.Clear();
            foreach (var kvp in namespaceTextureDictionary)
            {
                mappings.Add(new NamespaceTextureMapping { namespacePath = kvp.Key, texture = kvp.Value });
            }
        }
    }

}
