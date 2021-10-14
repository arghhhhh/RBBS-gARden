using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWindow : MonoBehaviour
{
    protected AudioManager audioManager;
    protected Director director;
    protected ImageTrackerManager imageTrackerManager;

    protected string currRef;

    public GameObject player;
    public Button playButton;
    public Button exitButton;

    protected Sprite playSprite;
    protected Sprite pauseSprite;
    protected Sprite replaySprite;
    protected Sprite exitSprite;
    protected Sprite stopSprite;

    protected bool isPlaying;
    protected bool canExit;

    protected Image playImage;
    protected Image exitImage;

    protected Text objectTitle;

    protected MiniPlayer miniPlayer;
    protected FullPlayer fullPlayer;

    void Awake()
    {
        director = FindObjectOfType<Director>();
        audioManager = director.GetComponent<AudioManager>();
        imageTrackerManager = FindObjectOfType<ImageTrackerManager>();
        miniPlayer = GetComponent<MiniPlayer>();
        fullPlayer = GetComponent<FullPlayer>();
    }
}
