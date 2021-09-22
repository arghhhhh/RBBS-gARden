using UnityEngine;

public class SpinReferencePasser : MonoBehaviour
{
    private SpinController spinController;
    [SerializeField]
    private GameObject spinButtonPrefab;

    public delegate void RefPrefabEnabled();
    public event RefPrefabEnabled EnabledEvent;

    public delegate void RefPrefabDisabled();
    public event RefPrefabDisabled DisabledEvent;
    private void Awake()
    {
        //instantiate spin button instance and set its parent to be the Canvas
        GameObject spinButton = Instantiate(spinButtonPrefab, transform.localPosition-new Vector3(0f,300f,0f), Quaternion.identity);
        UIManager UI = FindObjectOfType<UIManager>(); //only UIManager in the scene is attached to canvas
        spinButton.transform.SetParent(UI.gameObject.transform, false);

        //pass this specific prefab reference to its own spin controller
        spinController = spinButton.GetComponent<SpinController>();
        spinController.GiveReference(this); //gives the SpinController a reference to this prefab

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
