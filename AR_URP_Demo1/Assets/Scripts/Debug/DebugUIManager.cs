using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class DebugUIManager : MonoBehaviour
{
    public Button gpsButton;
    public Button inspectButton;
    public Button blurButton;
    public Button cameraButton;
    public GameObject gpsText;
    public GameObject runtimeInspector;
    public GameObject runtimeHeirarchy;
    public GameObject blurSphere;
    public ARSession arSession;

    public Text debug1;
    public Text debug2;
    public Text debug3;
    public Text debug4;

    void Start()
    {
        if (!FindObjectOfType<Director>().debug) //destroy all debuggging stuff if debugging not selected
        {
            Destroy(gameObject);
        }

        gpsText.SetActive(false);
        runtimeInspector.SetActive(false);
        runtimeHeirarchy.SetActive(false);

        gpsButton.onClick.AddListener(GpsButtonPress);
        inspectButton.onClick.AddListener(InspectButtonPress);
        blurButton.onClick.AddListener(BlurButtonPress);
        cameraButton.onClick.AddListener(CameraButtonPress);
    }

    void GpsButtonPress()
    {
        if (!gpsText.activeSelf)
        {
            gpsText.SetActive(true);
        }
        else
        {
            gpsText.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null); //disables button selected color
        }
    }

    void InspectButtonPress()
    {
        if (!runtimeHeirarchy.activeSelf && !runtimeInspector.activeSelf)
        {
            runtimeHeirarchy.SetActive(true);
        }
        else if (runtimeHeirarchy.activeSelf && !runtimeInspector.activeSelf)
        {
            runtimeInspector.SetActive(true);
        }
        else
        {
            runtimeHeirarchy.SetActive(false);
            runtimeInspector.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null); //disables button selected color
        }
    }

    void BlurButtonPress()
    {
        if (!blurSphere.activeSelf)
        {
            blurSphere.SetActive(true);
        }
        else
        {
            blurSphere.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null); //disables button selected color
        }
    }

    void CameraButtonPress()
    {
        if (arSession.enabled)
        {
            arSession.enabled = false;
            blurButton.interactable = false;
        }
        else
        {
            arSession.enabled = true;
            EventSystem.current.SetSelectedGameObject(null); //disables button selected color
        }
    }
}
