using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_AR_Tour : MonoBehaviour
{
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private GameObject uiPopup;
    [SerializeField]
    private GameObject blurSphere;
    void Start()
    {
        acceptButton.onClick.AddListener(AcceptButtonPress);
    }

    void AcceptButtonPress()
    {
        uiPopup.SetActive(false);
        blurSphere.SetActive(false);
    }
}
