using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button gpsButton;
    [SerializeField]
    private Button inspectButton;
    [SerializeField]
    private GameObject gpsText;
    [SerializeField]
    private GameObject inspectParent;

    void Start()
    {
        gpsButton.onClick.AddListener(GpsButtonPress);
        inspectButton.onClick.AddListener(InspectButtonPress);
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
        if (!inspectParent.activeSelf)
        {
            inspectParent.SetActive(true);
        }
        else
        {
            inspectParent.SetActive(false);
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
