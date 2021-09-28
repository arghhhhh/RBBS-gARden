using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Canvas_AR_Tour : MonoBehaviour
{
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private GameObject uiPopup;
    [SerializeField]
    private GameObject blurSphere;
    [SerializeField]
    private ARTrackedImageManager arTrackedImageManager;
    void Start()
    {
        acceptButton.onClick.AddListener(AcceptButtonPress);
        arTrackedImageManager.enabled = false;
    }

    void AcceptButtonPress()
    {
        uiPopup.SetActive(false);
        blurSphere.SetActive(false);
        arTrackedImageManager.enabled = true;
    }
}
