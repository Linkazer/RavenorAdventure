using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVNUI_LoadScene : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void LoadScene()
    {
        RVN_SceneManager.LoadScene(sceneIndex);
    }
}
