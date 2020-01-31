using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kitchen Element/Material Collection")]
public class MaterialCollection : ScriptableObject {
    public List<Material> materialList = new List<Material>();
    public List<Sprite> swatchList = new List<Sprite>();


}
