using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ITNotificationPanelOld : MonoBehaviour
{
    private ITReferencePasser objectReference;
    private string objectReferenceName;
    private string objectReferenceNameOld;
    private AudioManager audioManager;

    public GameObject shield;

    public Button playButton;
    public Button exitButton;
    public Image playImage;
    public Image exitImage;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Sprite replaySprite;
    public Sprite exitSprite;
    public Sprite stopSprite;
    public Text plantText;

    private bool updateObj;
    [SerializeField]
    private float forwardDistance;
    [SerializeField]
    private float upDistance;

    private bool showingPlay;
    private bool showingExit;

    private ImageTrackerManager imageTrackerManager;

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        imageTrackerManager = FindObjectOfType<ImageTrackerManager>();
    }
    void Start()
    {
        shield.SetActive(false);
        playButton.onClick.AddListener(PlayButtonPress);
        exitButton.onClick.AddListener(ExitButtonPress);

        imageTrackerManager.ImageTrackedEvent += DisableButton;
        imageTrackerManager.ImageLostEvent += EnableButton;
    }
    void FixedUpdate()
    {
        if (updateObj)
        {   //Since the object always needs to face the camera, its position/rotation properties don't need to be attached to a specific tracking state
            objectReference.transform.position = Camera.main.transform.position + Camera.main.transform.forward * forwardDistance + Camera.main.transform.up * upDistance;
            objectReference.transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w); //rotation doesn't need to be set
        }
    }
    private void PlayButtonPress()
    {
        StopAllCoroutines();
        //the object reference here is the prefab generated from the tracked image
        if (objectReference != null)
        {
            bool animatorState = objectReference.GetComponent<Animator>().GetBool("isSpinning"); //move this check bool to the enable panel event
            //if (!animatorState)
            if (!showingPlay)
            {
                objectReference.GetComponent<Animator>().SetBool("isSpinning", true);
                playImage.sprite = pauseSprite;
                exitImage.sprite = stopSprite;
                audioManager.PlaySound(imageTrackerManager.currentRef);
            }
            else
            {
                objectReference.GetComponent<Animator>().SetBool("isSpinning", false);
                playImage.sprite = playSprite;
                exitImage.sprite = exitSprite;
                audioManager.PauseSound(imageTrackerManager.currentRef);
            }
            showingPlay = !showingPlay;
        }
        else return;
    }

    private void ExitButtonPress()
    {
        //the object reference here is the prefab generated from the tracked image
        if (objectReference != null)
        {
            if (!showingPlay) //if exit button is visible
            {
                StartCoroutine(HideObjectAfterDelay(0.01f));
            }
            else //if stop button is visible
            {
                objectReference.GetComponent<Animator>().SetBool("isSpinning", false);
                showingPlay = false;
                playImage.sprite = replaySprite;
                exitImage.sprite = exitSprite;
                audioManager.StopSound(imageTrackerManager.currentRef);
            }
        }
        else return;
    }

    public void GiveReference(ITReferencePasser _newObject)
    {
        objectReference = _newObject;
        objectReferenceName = objectReference.ReferenceName();

        objectReference.EnabledEvent += EnablePanel;

        objectReferenceNameOld = objectReferenceName;
        //this can store old reference name and check to see if its a new object
    }

    void EnablePanel()
    {
        if (!shield.activeSelf) //no need to set active if it's already active
        {

            //audioManager.PlaySound("Notification");
            playImage.sprite = playSprite;
            exitImage.sprite = exitSprite;
            exitButton.interactable = true;
            if (objectReference != null)
                plantText.text = imageTrackerManager.currentRef;
            gameObject.SetActive(true);
            updateObj = true;
            StartCoroutine(HideObjectAfterDelay(10f)); //Disables button after X amount of seconds
        }
        {
            //if (audioManager.IsSoundPlaying(objectReference.name))
            //    StartCoroutine(LerpUp(2f));
        }
    }

    void DisablePanel()
    {
        //if (audioManager.IsSoundPlaying(objectReference.name))
        //    StartCoroutine(LerpDown(5f));
    }

    private void OnDestroy()
    {
        objectReference.EnabledEvent -= EnablePanel;
        imageTrackerManager.ImageTrackedEvent -= DisableButton;
        imageTrackerManager.ImageLostEvent -= EnableButton;
    }

    void EnableButton()
    {
        if (updateObj)
        {
            exitButton.interactable = true;
            exitImage.color = Color.white;
        }
    }

    void DisableButton()
    {
        if (updateObj && !showingPlay)
        {
            exitButton.interactable = false;
            exitImage.color = Color.grey;
        }
    }

    IEnumerator HideObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shield.SetActive(false);
        updateObj = false;
        objectReference.gameObject.SetActive(false);
        plantText.text = "";
    }
}
