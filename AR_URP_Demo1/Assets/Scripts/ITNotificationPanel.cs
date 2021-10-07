using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ITNotificationPanel : MonoBehaviour
{
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
    public Text objectTitle;

    private bool isPlaying;
    private bool canExit;

    private ImageTrackerManager imageTrackerManager;
    private string currRef;
    private string prevRef;

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

        imageTrackerManager.ImageTrackedEvent += ShieldManager;
        imageTrackerManager.ImageLostEvent += ShieldManager;
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        imageTrackerManager.ImageTrackedEvent -= ShieldManager;
        imageTrackerManager.ImageLostEvent -= ShieldManager;
    }

    void ShieldManager()
    {
        if (!shield.activeSelf) //if panel is not already active
        {
            currRef = imageTrackerManager.currentRef;

            if (currRef != null) //image is being tracked
            {
                ReferenceSetter(); //set panel title and reset button state
                shield.SetActive(true); //show panel

                //prevRef = currRef;
            }
        }
    }

    void ReferenceSetter()
    {
        objectTitle.text = currRef;
        playImage.sprite = playSprite;
        exitImage.sprite = exitSprite;
        isPlaying = false;
        canExit = true;
    }

    void PlayButtonPress() //triggered whenever the play button is pressed
    {
        if (isPlaying) //showing the pause button -- will change from playing state to paused state
        {
            playImage.sprite = playSprite;
            exitImage.sprite = exitSprite;
            audioManager.PauseSound(currRef);
        }

        else //showing the default play button -- will change from paused state to playing state
        {
            playImage.sprite = pauseSprite;
            exitImage.sprite = stopSprite;
            audioManager.PlaySound(currRef);
        }

        isPlaying = !isPlaying; //flip state of the play checker bool
        canExit = !canExit; //flip state of the exit checker bool -- this is because the exit button state is directly tied to the play button state
    }

    void ExitButtonPress()
    {
        if (canExit) //showing the exit button
        {
            shield.SetActive(false); //hide panel
            //have ImageTrackerManager check for a new reference on exit
        }

        else //showing the stop audio button
        {
            playImage.sprite = replaySprite;
            exitImage.sprite = exitSprite;
            audioManager.StopSound(currRef);
            isPlaying = false;
        }

        canExit = !canExit; //flip state of the exit checker bool
    }

    void TrackingImage()
    {

    }

    void TrackingLost()
    {

    }
}
