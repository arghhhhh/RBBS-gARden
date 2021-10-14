using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWindow : MonoBehaviour
{
    protected AudioManager audioManager;
    protected Director director;
    protected ImageTrackerManager imageTrackerManager;

    public GameObject player;
    public Button playButton;
    public Button exitButton;

    public Sprite playSprite;
    public Sprite pauseSprite;
    public Sprite replaySprite;
    public Sprite exitSprite;
    public Sprite stopSprite;

    protected bool isPlaying;
    protected bool canExit;

    protected string currRef;

    public Image playImage;
    public Image exitImage;

    public Text objectTitle;

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
