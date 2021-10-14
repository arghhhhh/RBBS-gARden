using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.Toast;
using SimpleJSON;

public class FullPlayer : PlayerWindow
{
    public Camera cam;
    private GameObject objectPrefab;
    public Button minimizeButton;
    public GameObject backButton;

    public float forwardDistance, upDistance;

    public TextAsset dataFile;
    private JSONNode jsonData;

    void Awake()
    {
        player.SetActive(false);
        cam.enabled = false;
    }

    void Start()
    {
        jsonData = JSON.Parse(dataFile.text);
        playButton.onClick.AddListener(PlayButtonPress);
        exitButton.onClick.AddListener(ExitButtonPress);
        minimizeButton.onClick.AddListener(MinimizeButtonPress);
    }

    public void PlayerSetup()
    {
        if (director.debug)
            Toast.Show("PlayerExpandEvent triggered");
        currRef = imageTrackerManager.currentRef;
        cam.enabled = true; //enable background
        player.SetActive(true); //enable player
        backButton.SetActive(false);
        if (currRef != null && audioManager != null)
        {
            if (isPlaying)
            {
                playImage.sprite = pauseSprite;
                exitImage.sprite = stopSprite;
            }
            else
            {
                playImage.sprite = playSprite;
                exitImage.sprite = exitSprite;
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
            MinimizeButtonPress();
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

    void MinimizeButtonPress()
    {
        Destroy(objectPrefab);
        cam.enabled = false;
        player.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    void LoadPrefab()
    {
        if (currRef != null)
        {
            objectTitle.text = currRef;
            objectPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/HUBB/" + jsonData[currRef]["prefab"].Value));
            if (objectPrefab != null)
            {
                if (director.debug)
                    Toast.Show("Instantiated " + "Prefabs/HUBB/" + jsonData[currRef]["prefab"].Value + " as GameObject", 3f);
                objectPrefab.transform.position = cam.transform.position + cam.transform.forward * forwardDistance
                + cam.transform.up * upDistance;
                //objectPrefab.transform.SetParent(backgroundSphere.transform);
            }
            else
                Toast.Show("Could not instantiate Prefabs/HUBB/" + jsonData[currRef]["prefab"].Value + " as GameObject", 3f);
        }
        else
            if (director.debug)
                Toast.Show("Reference not found!", 2f);
    }
}
