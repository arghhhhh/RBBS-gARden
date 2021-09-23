using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class CameraController_Debug : MonoBehaviour
{
    private Text buttonText;
    private bool isCamEnabled;

    [SerializeField]
    private Button blurButton;

    [SerializeField]
    private ARSession arSession;

    private void Awake()
    { 
        buttonText = GetComponentInChildren<Text>();
    }

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(CameraButtonPress);
        //DisableAR();
        isCamEnabled = true;
    }

    void CameraButtonPress()
    {
        if (isCamEnabled)
        {
            buttonText.text = "Enable Camera";
            DisableAR();
            blurButton.interactable = false;
        }
        else
        {
            buttonText.text = "Disable Camera";
            EnableAR();
            blurButton.interactable = true;
        }

        isCamEnabled = !isCamEnabled;
    }

    void DisableAR()
    {
        arSession.enabled = false;
    }

    void EnableAR()
    {
        arSession.enabled = true;
        arSession.Reset();
    }
}
