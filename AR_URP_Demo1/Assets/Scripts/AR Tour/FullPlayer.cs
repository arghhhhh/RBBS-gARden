using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FullPlayer : MonoBehaviour
{
    public Camera cam;
    private GameObject objectAnimation;
    public GameObject shield;
    public GameObject backgroundSphere;
    public Button closeButton;
    public Button backButton;

    private AudioManager audioManager;
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

    private MiniPlayer miniPlayer;

    public TextAsset dataFile;
    private string[][] dataPairs;

    public float forwardDistance, upDistance;

    private DebugUIManager debugger;

    void Awake()
    {
        miniPlayer = FindObjectOfType<MiniPlayer>();
        debugger = FindObjectOfType<DebugUIManager>();
        shield.SetActive(false);
        backgroundSphere.SetActive(false);
        cam.enabled = false;
    }

    void Start()
    {
        SortObjectPairs();
        playButton.onClick.AddListener(PlayButtonPress);
        exitButton.onClick.AddListener(ExitButtonPress);
        closeButton.onClick.AddListener(CloseButtonPress);
        miniPlayer.PlayerExpandEvent += PlayerSetup;
    }

    void OnDestroy()
    {
        miniPlayer.PlayerExpandEvent -= PlayerSetup;
    }

    void PlayerSetup()
    {
        cam.enabled = true; //enable background
        shield.SetActive(true); //enable canvas
        backgroundSphere.SetActive(true);
        backButton.gameObject.SetActive(false);
        if (audioManager.IsSoundPlaying(miniPlayer.CurrRef)) 
        {
            playImage.sprite = pauseSprite;
            exitImage.sprite = stopSprite;
            isPlaying = true;
            canExit = false;
        }
        else
        {
            playImage.sprite = playSprite;
            exitImage.sprite = exitSprite;
            isPlaying = false;
            canExit = true;
        }
        LoadPrefab();
    }

    void PlayButtonPress() //triggered whenever the play button is pressed
    {
        if (isPlaying) //showing the pause button -- will change from playing state to paused state
        {
            playImage.sprite = playSprite;
            exitImage.sprite = exitSprite;
            audioManager.PauseSound(miniPlayer.CurrRef);
        }

        else //showing the default play button -- will change from paused state to playing state
        {
            playImage.sprite = pauseSprite;
            exitImage.sprite = stopSprite;
            audioManager.PlaySound(miniPlayer.CurrRef);
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
            audioManager.StopSound(miniPlayer.CurrRef);
            isPlaying = false;
        }

        canExit = !canExit; //flip state of the exit checker bool
    }

    void CloseButtonPress()
    {
        Destroy(objectAnimation);
        cam.enabled = false;
        shield.SetActive(false);
        backgroundSphere.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    void SortObjectPairs()
    {
        string[] dataLines = dataFile.text.Split('\n');
        dataPairs = new string[dataLines.Length][];
        int lineNum = 0;
        foreach (string line in dataLines)
        {
            dataPairs[lineNum++] = line.Split(',');
        }
    }

    void LoadPrefab()
    {
        for (int i = 0; i < dataPairs.GetLength(0); i++)
        {
            if (dataPairs[i][0] == miniPlayer.CurrRef)
            {
                objectTitle.text = miniPlayer.CurrRef;
                //objectAnimation = Instantiate(Resources.Load("Prefabs/HUBB/" + dataPairs[i][1]) as GameObject);
                objectAnimation = Instantiate(Resources.Load("Prefabs/HUBB/Logo Red") as GameObject);
                objectAnimation.transform.position = cam.transform.position + cam.transform.forward * forwardDistance
                    + cam.transform.up * upDistance;
                objectAnimation.transform.SetParent(backgroundSphere.transform);
            }
            if (dataPairs[i][0] == miniPlayer.CurrRef) break;
        }
    }
}
