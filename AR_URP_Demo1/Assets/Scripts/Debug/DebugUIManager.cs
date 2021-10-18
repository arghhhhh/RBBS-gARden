using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class DebugUIManager : MonoBehaviour
{
    public Button gpsButton;
    public Button inspectButton;
    public Button simulateTrackButton;
    public Button cameraButton;
    public GameObject gpsText;
    public GameObject runtimeInspector;
    public GameObject runtimeHeirarchy;
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
        simulateTrackButton.onClick.AddListener(SimulateTrackButtonPress);
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

    void SimulateTrackButtonPress()
    {
        MiniPlayer player = FindObjectOfType<MiniPlayer>();
        if (player != null)
            player.SimulateTracking();
    }

    void CameraButtonPress()
    {
        if (arSession.enabled)
        {
            arSession.enabled = false;
        }
        else
        {
            arSession.enabled = true;
            EventSystem.current.SetSelectedGameObject(null); //disables button selected color
        }
    }
}
