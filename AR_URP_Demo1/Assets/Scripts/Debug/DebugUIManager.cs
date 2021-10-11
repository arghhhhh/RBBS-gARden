using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using Joss.SceneManagement;

public class DebugUIManager : MonoBehaviour
{
    public Button gpsButton;
    public Button inspectButton;
    public Button blurButton;
    public Button cameraButton;
    public GameObject gpsText;
    public GameObject inspector;
    public GameObject blurSphere;
    public ARSession arSession;

    public bool debug;
    public GameObject debugger;
    public Text debug1;
    public Text debug2;
    public Text debug3;
    public Text debug4;

    void Awake()
    {
        gpsText.SetActive(false);
        inspector.SetActive(false);
    }
    void Start()
    {
        gpsButton.onClick.AddListener(GpsButtonPress);
        inspectButton.onClick.AddListener(InspectButtonPress);
        blurButton.onClick.AddListener(BlurButtonPress);
        cameraButton.onClick.AddListener(CameraButtonPress);
        SceneLauncher.StoreSceneHistory();
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
        if (!inspector.activeSelf)
        {
            inspector.SetActive(true);
        }
        else
        {
            inspector.SetActive(false);
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

    //private GameObject GetAncestor(GameObject item)
    //{
    //    if (item.transform.parent == null)
    //        return item;
    //    return(GetAncestor(item.transform.parent.gameObject));
    //}
}
