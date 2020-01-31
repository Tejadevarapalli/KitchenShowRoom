using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This simple script allows you to switch between scenes based on the
//order that they appear in the build settings

public class LoadScene : MonoBehaviour {

    public void SelectScene (int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
} 
