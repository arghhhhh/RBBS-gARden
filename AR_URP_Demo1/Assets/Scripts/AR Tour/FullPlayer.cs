using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.Toast;
using SimpleJSON;

public class FullPlayer : PlayerWindow
{
    public Camera cam;
    public GameObject objectPrefab { get; private set; }
    public Button minimizeButton;
    public GameObject backButton;

    public float forwardDistance, upDistance;

    public TextAsset dataFile;
    private JSONNode jsonData;

    void Start()
    {
        shield.SetActive(false);
        cam.enabled = false;

        jsonData = JSON.Parse(dataFile.text);
        playButton.onClick.AddListener(PlayButtonPress);
        exitButton.onClick.AddListener(ExitButtonPress);
        minimizeButton.onClick.AddListener(MinimizeButtonPress);
    }

    public void PlayerSetup()
    {
        if (currRef != null)
        {
            if (director.playingAudio == currRef)
            {
                playImage.sprite = pauseSprite;
                exitImage.sprite = stopSprite;
            }
            else
            {
                playImage.sprite = playSprite;
                exitImage.sprite = exitSprite;
            }
            cam.enabled = true; //enable background
            shield.SetActive(true); //enable player
            backButton.SetActive(false);
            LoadPrefab();
        }
        else if (director.debug)
            Toast.Show("PlayerSetup(): Object reference is null!", 2f);
    }

    void PlayButtonPress() //triggered whenever the play button is pressed
    {
        if (director.playingAudio == currRef) //showing the pause button -- will change from playing state to paused state
        {
            playImage.sprite = playSprite;
            exitImage.sprite = exitSprite;
            audioManager.PauseSound(currRef);
            director.playingAudio = string.Empty;
        }

        else //showing the default play button -- will change from paused state to playing state
        {
            playImage.sprite = pauseSprite;
            exitImage.sprite = stopSprite;
            audioManager.PlaySound(currRef);
            director.playingAudio = currRef;
        }

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
            director.playingAudio = string.Empty;
            canExit = true;
        }
    }

    void MinimizeButtonPress()
    {
        if (objectPrefab != null)
            objectPrefab.SetActive(false);
        cam.enabled = false;
        shield.SetActive(false);
        backButton.SetActive(true);
        miniPlayer.LaunchPlayer(currRef);
    }

    void LoadPrefab()
    {
        if (currRef != null)
        {
            objectTitle.text = currRef;
            if (objectPrefab != null) //if object prefab has already been created
                objectPrefab.SetActive(true);
            else //if object prefab has not been instantiated yet
                objectPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/HUBB/" + jsonData[currRef]["prefab"].Value));
            if (objectPrefab != null)
            {
                if (director.debug)
                    Toast.Show("Instantiated " + "Prefabs/HUBB/" + jsonData[currRef]["prefab"].Value + " as GameObject", 3f);
                objectPrefab.transform.position = cam.transform.position + cam.transform.forward * forwardDistance
                + cam.transform.up * upDistance;
                float scale = float.Parse(jsonData[currRef]["scale"].Value);
                if (scale != float.NaN)
                {
                    //if (director.debug)
                    //    Toast.Show("Scale is " + scale, 3f);
                    objectPrefab.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
            else
                Toast.Show("Could not instantiate Prefabs/HUBB/" + jsonData[currRef]["prefab"].Value + " as GameObject", 3f);
        }
        else if (director.debug)
            Toast.Show("Reference not found!", 2f);
    }
}
