using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BakeLighting : MonoBehaviour
{
    [MenuItem("Intro to 3D/Bake Lighting")]
    static void BakeLightingMenuItem()
    {
        Lightmapping.Bake();
    }
}
