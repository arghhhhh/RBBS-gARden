using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button infoButton;
    [SerializeField]
    private Button arButton;
    [SerializeField]
    private Button mapButton;
    [SerializeField]
    private Button debugButton;

    private void Awake()
    {
        infoButton.onClick.AddListener(LoadInfo);
        arButton.onClick.AddListener(LoadAR);
        mapButton.onClick.AddListener(LoadMap);
        debugButton.onClick.AddListener(LoadDebug);
    }

    void SceneLauncher(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    void LoadInfo() 
    {
        SceneLauncher(1);
    }

    void LoadAR()
    {
        SceneLauncher(1);
    }

    void LoadMap()
    {
        SceneLauncher(1);
    }

    void LoadDebug()
    {
        SceneLauncher(2);
    }
}
