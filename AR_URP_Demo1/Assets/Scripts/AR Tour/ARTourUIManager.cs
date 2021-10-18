using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Joss.Helpers;
using EasyUI.Toast;

public class ARTourUIManager : MonoBehaviour
{
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private GameObject uiPopup;
    [SerializeField]
    private GameObject blurSphere;
    [SerializeField]
    private ARTrackedImageManager arTrackedImageManager;

    private Director director;

    void Awake()
    {
        director = FindObjectOfType<Director>();
    }

    void Start()
    {
        acceptButton.onClick.AddListener(AcceptButtonPress);
        arTrackedImageManager.enabled = false;
        SceneLauncher.StoreSceneHistory();
    }

    void AcceptButtonPress()
    {
        uiPopup.SetActive(false);
        blurSphere.SetActive(false);
        arTrackedImageManager.enabled = true;
    }
}
