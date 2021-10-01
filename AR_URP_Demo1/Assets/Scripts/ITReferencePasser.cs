using UnityEngine;
public class ITReferencePasser : MonoBehaviour
{
    public delegate void RefPrefabEnabled();
    public event RefPrefabEnabled EnabledEvent;

    public delegate void RefPrefabDisabled();
    public event RefPrefabDisabled DisabledEvent;

    [SerializeField]
    private string refName;

    private ITNotificationPanel itNotifPanel; 

    private void Awake()
    {
        itNotifPanel = FindObjectOfType<ITNotificationPanel>();

        GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true; //keeps state of animator between enabling & disabling of prefab object
    }

    private void OnEnable()
    {
        itNotifPanel.GiveReference(this);
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

    public string ReferenceName() => refName;
}
