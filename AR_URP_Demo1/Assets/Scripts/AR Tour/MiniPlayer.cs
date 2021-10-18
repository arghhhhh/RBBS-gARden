using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using EasyUI.Toast;

public class MiniPlayer : PlayerWindow
{
    public Button expandButton;
    public string cardRef { get; private set; }

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
        if (!shield.activeSelf && !fullPlayer.shield.activeSelf) //if panel is not already active
        {
            currRef = imageTrackerManager.currentRef;
            cardRef = currRef;

            if (currRef != null) //image is being tracked
            {
                if (director.debug)
                    Toast.Show("Reference is " + currRef + "!", 2f);
                ReferenceSetter(currRef); //set panel title and reset button state
            }
        }
    }

    public void ReferenceSetter(string reference)
    {
        if (reference != null)
        {
            objectTitle.text = reference;
            if (director.playingAudio == currRef)
            {
                playImage.sprite = pauseSprite;
                exitImage.sprite = stopSprite;
                canExit = false;
            }
            else
            {
                playImage.sprite = playSprite;
                exitImage.sprite = exitSprite;
                canExit = true;
            }
            shield.SetActive(true); //show panel
        }
        else if (director.debug)
            Toast.Show("ReferenceSetter(): Object reference is null!", 2f);
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
            if (fullPlayer.objectPrefab != null)
                Destroy(fullPlayer.objectPrefab);
            shield.SetActive(false); //hide panel
            //have ImageTrackerManager check for a new reference on exit
        }

        else //showing the stop audio button
        {
            playImage.sprite = replaySprite;
            exitImage.sprite = exitSprite;
            audioManager.StopSound(currRef);
            director.playingAudio = string.Empty;
        }

        canExit = !canExit; //flip state of the exit checker bool
    }

    void ExpandButtonPress()
    {
        if (director.debug)
            Toast.Show("Expand button pressed!", 2f);
        shield.SetActive(false);
        fullPlayer.PlayerSetup();
    }
}
