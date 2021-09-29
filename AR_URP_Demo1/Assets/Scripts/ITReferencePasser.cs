using UnityEngine;
public class ITReferencePasser : MonoBehaviour
{
    private ITNotificationPanel itNotificationPanel;
    [SerializeField]
    private GameObject refPrefab;

    public delegate void RefPrefabEnabled();
    public event RefPrefabEnabled EnabledEvent;

    public delegate void RefPrefabDisabled();
    public event RefPrefabDisabled DisabledEvent;
    private void Awake()
    {
        //instantiate spin button instance and set its parent to be the Canvas
        GameObject _ref = Instantiate(refPrefab, transform.localPosition-new Vector3(0f,300f,0f), Quaternion.identity);
        Canvas UI = FindObjectOfType<Canvas>(); //only one canvas object per scene
        _ref.transform.SetParent(UI.gameObject.transform, false); //set the Canvas to be the parent of the instantiated button

        //pass this specific prefab reference to its own spin controller
        itNotificationPanel = _ref.GetComponent<ITNotificationPanel>();
        itNotificationPanel.GiveReference(this); //gives the SpinController a reference to this prefab

        GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true; //keeps state of animator between enabling & disabling of prefab object
    }

    private void OnEnable()
    {
        if (EnabledEvent != null)
        {
            EnabledEvent();
        }
    }

    private void OnDisable()
    {
        if (DisabledEvent != null)
        {
            DisabledEvent();
        }
    }
}
