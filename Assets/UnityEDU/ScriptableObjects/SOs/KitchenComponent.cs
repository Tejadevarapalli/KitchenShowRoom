using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the ScriptableObject for all kitchen components. Notices that the class is
//of ScriptableObject rather than Monobehaviour! The line directly below is what allows
//you to create a component directly from the create menu.

[CreateAssetMenu (menuName = "Kitchen Element/New Component")]
public class KitchenComponent : ScriptableObject {

    public enum ComponentType {Appliance, Countertop, Drawers, Doors, Accessory, Surface};
    public ComponentType componentType;

    public List<MaterialCollection> materialList = new List<MaterialCollection>();
    public enum HasSwappableMeshes {no, yes};
    public HasSwappableMeshes hasSwappableMeshes;
    
    
    public enum VisibilityOption {no, yes};
    public VisibilityOption visibilityOption; 

    public enum AccessoryType {none, pull, knob, baseWood};
    [Tooltip("Identify special item types")]
    public AccessoryType identityType;

  
}
