using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using nadena.dev.modular_avatar.core;
using nadena.dev.modular_avatar.core.menu;
using nadena.dev.ndmf;
using nadena.dev.ndmf.runtime;
using SweetForest.LazyToggleObjectMA.Editor;
using SweetForest.LazyToggleObjectMA.Utilities.LazyHierachies;
using UnityEditor.Animations;
using UnityEngine;

[assembly: ExportsPlugin(typeof(LazyToggleObjectNDMFPlugin))]


namespace SweetForest.LazyToggleObjectMA.Editor

 {

public class LazyToggleObjectNDMFPlugin : Plugin<LazyToggleObjectNDMFPlugin>
{
    public override string DisplayName => "Lazy Toggle Object MA";
    protected override void Configure()
    {

        LazyHierarchy<Components.LazyToggleObjectMAInstaller> lazyHierarchy = new LazyHierarchy<Components.LazyToggleObjectMAInstaller>();
        
        InPhase(BuildPhase.Generating)
            .BeforePlugin("nadena.dev.modular-avatar")
            .Run("Lazy Toggle NDM Plugin Before Plugin", ctx =>
            {
                Debug.Log("Starting Lazy Toggle Object Plugin....");

                prepaireLazyHierachy(lazyHierarchy, ctx);
                
                try {
                    BuildMenu(lazyHierarchy,ctx);
                } catch(Exception e) {
                        Debug.LogError("An error occurred: " + e.Message);
                }
                try {
                    BuildToggleAnimationAnimator(lazyHierarchy,ctx);
                } catch(Exception e) {
                        Debug.LogError("An error occurred: " + e.Message);
                }
                Debug.Log("Finished LazyToggleObjectNDMFPlugin!");


            })
            ;
        // not sure. lol. but it's still working
        InPhase(BuildPhase.Generating)
        .AfterPlugin("nadena.dev.modular-avatar")
        .Run("Lazy Toggle NDM Plugin After Plugin", ctx =>
        {
           // DoAnimation(lazyData, ctx);
        });
    }

    
    public void prepaireLazyHierachy(LazyHierarchy<Components.LazyToggleObjectMAInstaller> lazyHierarchy,BuildContext ctx) {
        try {
            Debug.Log("LazyToggleObjectMA: preparing Lazy Hierachy");
            var list = ctx.AvatarRootObject.GetComponentsInChildren<Components.LazyToggleObjectMAInstaller>().ToList();
            foreach (var item in list)
            {
                lazyHierarchy.Add(item.NamespaceGroup,item);
            }
             Debug.Log("LazyToggleObjectMA: prepared Lazy Hierachy");
        } catch(Exception e) {
            Debug.LogError("An Error occurred: "+e.Message);
        }
    }

    public void BuildMenu(LazyHierarchy<Components.LazyToggleObjectMAInstaller> lazyHierarchy,BuildContext ctx) {
        Debug.Log("LazyToggleObjectMA: building Menu");
        GameObject installerMenu = new GameObject("Lazy Menu");
        var menuInstaller = installerMenu.AddComponent<ModularAvatarMenuInstaller>();
        var menuGroup = installerMenu.AddComponent<ModularAvatarMenuGroup>();
        installerMenu.transform.parent = ctx.AvatarRootObject.transform;

        var parentNamespace = lazyHierarchy.GetTopNamespaceNodes();
        Debug.Log("LazyToggleObjectMA: Top Namespace = "+parentNamespace.Count);
        var namespaceMapData = ctx.AvatarRootObject.GetComponent<Components.LazyNamespaceIconMappingData>();

        foreach (var item in namespaceMapData.GetCachedMappingViewer())
                {
                    Debug.Log("LazyToggleObjectMA: what in side mapping`"+item.Key +" t: "+(item.Value==null?"null":item.Value.name));
                }

    
       ApplyRecursiveParentMenu(namespaceMapData,installerMenu,lazyHierarchy.Root);
            

              
        
    }
    private void ApplyRecursiveParentMenu(Components.LazyNamespaceIconMappingData namespaceMapData, GameObject parentGameObject,LazyHierarchyNode<Components.LazyToggleObjectMAInstaller> node) {
        
         
            foreach (var item in node.Children)
        {
            Debug.Log("LazyToggleObjectMA: adding sub menu "+item.Key+" in "+node.Namespace);
            GameObject itemMenu = new GameObject("sub menu");
            itemMenu.transform.parent = parentGameObject.transform;
            addSubItemMenu(namespaceMapData,itemMenu,item.Value);
            ApplyRecursiveParentMenu(namespaceMapData,itemMenu,item.Value);

        }
           
        
        // add value
        foreach (var lazyToggleObjectMAInstaller in node.Values)
        {
             Debug.Log("LazyToggleObjectMA: added pure item menu"+lazyToggleObjectMAInstaller.getNameButton()+" in "+node.Namespace);
            GameObject itemMenu = new GameObject("item menu");
            itemMenu.transform.parent = parentGameObject.transform;
            addItemMenu(itemMenu,lazyToggleObjectMAInstaller);
        }
    }
     private void addSubItemMenu(Components.LazyNamespaceIconMappingData namespaceMapData,GameObject gameObject, LazyHierarchyNode<Components.LazyToggleObjectMAInstaller> node) {
            var submenu = gameObject.AddComponent<ModularAvatarMenuItem>();
            submenu.name = node.Namespace;
            submenu.Control.name = node.Namespace;
            if(namespaceMapData != null) {
                var t = namespaceMapData.GetTextureViewer(node.NamespacePath);
                Debug.Log("LazyToggleObjectMA: checking path: "+node.NamespacePath+" Name: "+node.Namespace);
                
                if(t!=null) {
                    submenu.Control.icon = t;
                    Debug.Log("LazyToggleObjectMA: overide icon on `"+node.Namespace+"` sub menu");
                }
                
            }
            submenu.Control.type = VirtualControl.ControlType.SubMenu;
            submenu.MenuSource = SubmenuSource.Children;
            Debug.Log("LazyToggleObjectMA: added sub menu "+node.Namespace);
            // check sub menu icon
    }
    
    private void addItemMenu(GameObject gameObject, Components.LazyToggleObjectMAInstaller lazyToggleObjectMAInstaller) {
            // parameter
            var parameterComponent = gameObject.AddComponent<ModularAvatarParameters>();
            var parameterConfig = new ParameterConfig();
            parameterConfig.defaultValue = lazyToggleObjectMAInstaller.DefaultValue ? 1 : 0;
            parameterConfig.saved = lazyToggleObjectMAInstaller.SavedValue;
            parameterConfig.localOnly = lazyToggleObjectMAInstaller.LocalOnly;
            parameterConfig.syncType = lazyToggleObjectMAInstaller.Synced ? ParameterSyncType.Bool : ParameterSyncType.NotSynced;
            parameterConfig.nameOrPrefix = lazyToggleObjectMAInstaller.getParameter();
            parameterComponent.parameters.Add(parameterConfig);

            var itemComponent = gameObject.AddComponent<ModularAvatarMenuItem>();
                itemComponent.name = lazyToggleObjectMAInstaller.getNameButton();
                itemComponent.Control.name = lazyToggleObjectMAInstaller.getNameButton();
                if (lazyToggleObjectMAInstaller.IconButton != null)
                {
                    itemComponent.Control.icon = lazyToggleObjectMAInstaller.IconButton;
                }
                
                itemComponent.Control.type = VirtualControl.ControlType.Toggle;
                var vControlParameter = new VirtualControl.Parameter();
                vControlParameter.name = lazyToggleObjectMAInstaller.getParameter();

                itemComponent.Control.parameter = vControlParameter;

                Debug.Log("LazyToggleObjectMA: added Item menu "+lazyToggleObjectMAInstaller.getParameter());

    }
   
    // animation
    public void BuildToggleAnimationAnimator(LazyHierarchy<Components.LazyToggleObjectMAInstaller> lazyHierarchy,BuildContext ctx) {
        AnimatorController animatorController = ctx.AvatarDescriptor.baseAnimationLayers[4].animatorController as AnimatorController;

        var list = lazyHierarchy.GetAllValues();


        foreach (var item in list)
        {
             
            AddToggleAnimationAnimator(animatorController,item);
            
        }
    }
    
    private void AddToggleAnimationAnimator(AnimatorController animatorController,Components.LazyToggleObjectMAInstaller lazyToggleObjectMAInstaller) {
       
                animatorController.AddParameter(lazyToggleObjectMAInstaller.getParameter(), AnimatorControllerParameterType.Bool);
                var anim_layer = new AnimatorControllerLayer();
                anim_layer.name = lazyToggleObjectMAInstaller.getParameter();
                anim_layer.defaultWeight = 1;
                anim_layer.stateMachine = new AnimatorStateMachine();
                anim_layer.stateMachine.name = lazyToggleObjectMAInstaller.getParameter();;
                animatorController.AddLayer(anim_layer);
                // create state to
                AnimatorState toggleONState = anim_layer.stateMachine.AddState("ON = Object HIDE"); // why false means visible ?
                                                                                                    // in my-case i used to accidentally naked.
                                                                                                    // this better to avoid default value
                                                                                                    // Create a new AnimationClip
                AnimationClip animationClipON = new AnimationClip();

                // Create an AnimationCurve for the Renderer.enabled property
                AnimationCurve curve = new AnimationCurve();
                // Add keyframes for showing and hiding the object
                curve.AddKey(0f, 0f); // Hide the object at time 0
                                      // make set active false to hide on gameobject
                animationClipON.SetCurve(RuntimeUtil.AvatarRootPath(lazyToggleObjectMAInstaller.gameObject), typeof(GameObject), "m_IsActive", curve);
                toggleONState.motion = animationClipON;

                AnimatorState toggleOFFState = anim_layer.stateMachine.AddState("OFF = Object SHOW");
                // Create a new AnimationClip
                AnimationClip animationClipOFF = new AnimationClip();

                // Create an AnimationCurve for the Renderer.enabled property
                AnimationCurve curve2 = new AnimationCurve();
                // Add keyframes for showing and hiding the object
                curve2.AddKey(0f, 1f); // show the object at time 0
                                       // make set active false to show on gameobject
                animationClipOFF.SetCurve(RuntimeUtil.AvatarRootPath(lazyToggleObjectMAInstaller.gameObject), typeof(GameObject), "m_IsActive", curve2);
                toggleOFFState.motion = animationClipOFF;

                AnimatorStateTransition transitionOff = toggleONState.AddTransition(toggleOFFState);
                transitionOff.AddCondition(AnimatorConditionMode.IfNot, 0, lazyToggleObjectMAInstaller.getParameter());
                transitionOff.hasExitTime = false;
                transitionOff.exitTime = 0;
                transitionOff.duration = 0;
                AnimatorStateTransition transitionOn = toggleOFFState.AddTransition(toggleONState);
                transitionOn.AddCondition(AnimatorConditionMode.If, 0, lazyToggleObjectMAInstaller.getParameter());
                transitionOn.hasExitTime = false;
                transitionOn.exitTime = 0;
                transitionOn.duration = 0;

    }

    }
}