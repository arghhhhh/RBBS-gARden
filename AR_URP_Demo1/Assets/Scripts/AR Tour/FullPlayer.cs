using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Joss.Helpers;
using EasyUI.Toast;

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

    public MiniPlayer miniPlayer;

    public TextAsset dataFile;
    public string[][] dataPairs;

    public float forwardDistance, upDistance;

    private Director director;

    private string refName;

    void Awake()
    {
        director = FindObjectOfType<Director>();
        audioManager = director.GetComponent<AudioManager>();
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
        if (director.debug)
            Toast.Show("PlayerExpandEvent triggered");
        refName = miniPlayer.CurrRef;
        cam.enabled = true; //enable background
        shield.SetActive(true); //enable canvas
        backgroundSphere.SetActive(true);
        backButton.gameObject.SetActive(false);
        if (refName != null && audioManager != null)
        {
            if (audioManager.IsSoundPlaying(refName))
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
        }
        LoadPrefab();
    }

    void PlayButtonPress() //triggered whenever the play button is pressed
    {
        if (isPlaying) //showing the pause button -- will change from playing state to paused state
        {
            playImage.sprite = playSprite;
            exitImage.sprite = exitSprite;
            audioManager.PauseSound(refName);
        }

        else //showing the default play button -- will change from paused state to playing state
        {
            playImage.sprite = pauseSprite;
            exitImage.sprite = stopSprite;
            audioManager.PlaySound(refName);
        }

        isPlaying = !isPlaying; //flip state of the play checker bool
        canExit = !canExit; //flip state of the exit checker bool -- this is because the exit button state is directly tied to the play button state
    }

    void ExitButtonPress()
    {
        if (canExit) //showing the exit button
        {
            CloseButtonPress();
            //have ImageTrackerManager check for a new reference on exit
        }

        else //showing the stop audio button
        {
            playImage.sprite = replaySprite;
            exitImage.sprite = exitSprite;
            audioManager.StopSound(refName);
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
        string[] dataLines = dataFile.text.Split('\n');//splits text data into separate lines 
        Debug.LogError("DataLines contains " + dataLines.Length + " items");
        if (dataLines.Length != 0)
        {
            dataPairs = new string[dataLines.Length][];
        }
        else
        {
            Toast.Show("Error sorting text data!", 2f);
        }
        if (dataPairs.GetLength(0) != 0)
        {
            int lineNum = 0;
            foreach (string line in dataLines)
            {
                dataPairs[lineNum++] = line.Split(',');
            }
            for (int i = 0; i < dataPairs.GetLength(0); i++)
            {
                Toolbox.RemoveLineEndings(dataPairs[i][1]);
            }
        }
    }

    void LoadPrefab()
    {
        if (refName != null)
        {
            for (int i = 0; i < 4; i++)
            {
                if (dataPairs.Length != 0)
                {
                    if (dataPairs[i][0] == refName)
                    {
                        objectTitle.text = refName;
                        objectAnimation = Instantiate(Resources.Load<GameObject>("Prefabs/HUBB/" + dataPairs[i][1]));
                        if (objectAnimation != null)
                        {
                            if (director.debug)
                                Toast.Show("Instantiated " + "Prefabs/HUBB/" + dataPairs[i][1] + " as GameObject", 3f);
                            objectAnimation.transform.position = cam.transform.position + cam.transform.forward * forwardDistance
                            + cam.transform.up * upDistance;
                            objectAnimation.transform.SetParent(backgroundSphere.transform);
                        }
                        else
                            Toast.Show("Could not instantiate Prefabs/HUBB/" + dataPairs[i][1] + " as GameObject", 3f);
                    }
                    if (dataPairs[i][0] == refName) break;
                }
            }
        }
        else
            if (director.debug)
                Toast.Show("Reference not found!", 2f);
    }
}
