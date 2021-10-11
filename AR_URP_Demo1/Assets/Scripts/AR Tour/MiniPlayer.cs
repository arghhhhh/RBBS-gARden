using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MiniPlayer : MonoBehaviour
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
    public string CurrRef { get; private set; }

    public Button expandButton;
    public GameObject fullPlayer;
    public delegate void PlayerExpanded();
    public event PlayerExpanded PlayerExpandEvent;

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
        expandButton.onClick.AddListener(ExpandButtonPress);

        imageTrackerManager.ImageTrackedEvent += ShieldManager;
        imageTrackerManager.ImageLostEvent += ShieldManager;
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
            CurrRef = imageTrackerManager.currentRef;

            if (CurrRef != null) //image is being tracked
            {
                ReferenceSetter(); //set panel title and reset button state
                shield.SetActive(true); //show panel

                //prevRef = currRef;
            }
        }
    }

    void ReferenceSetter()
    {
        objectTitle.text = CurrRef;
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
            audioManager.PauseSound(CurrRef);
        }

        else //showing the default play button -- will change from paused state to playing state
        {
            playImage.sprite = pauseSprite;
            exitImage.sprite = stopSprite;
            audioManager.PlaySound(CurrRef);
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
            audioManager.StopSound(CurrRef);
            isPlaying = false;
        }

        canExit = !canExit; //flip state of the exit checker bool
    }

    void ExpandButtonPress()
    {
        if (PlayerExpandEvent != null) //Fires only when an image is newly tracked
        {
            PlayerExpandEvent();
        }
        shield.SetActive(false);
    }
}
