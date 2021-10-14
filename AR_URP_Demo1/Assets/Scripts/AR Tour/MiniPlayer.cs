using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using EasyUI.Toast;

public class MiniPlayer : PlayerWindow
{
    

    public Button expandButton;

    void Start()
    {
        player.SetActive(false);
        playButton.onClick.AddListener(PlayButtonPress);
        exitButton.onClick.AddListener(ExitButtonPress);
        expandButton.onClick.AddListener(ExpandButtonPress);

        imageTrackerManager.ImageTrackedEvent += ShieldManager;
        imageTrackerManager.ImageLostEvent += ShieldManager;

        if (director.debug)
            Toast.Show("Image tracker manager is named " + imageTrackerManager.name, 2f);
    }

    void OnDestroy()
    {
        imageTrackerManager.ImageTrackedEvent -= ShieldManager;
        imageTrackerManager.ImageLostEvent -= ShieldManager;
    }

    void ShieldManager()
    {
        
        if (!player.activeSelf) //if panel is not already active
        {
            if (director.debug)
                Toast.Show("Player seems to be active!", 2f);
            currRef = imageTrackerManager.currentRef;

            if (currRef != null) //image is being tracked
            {
                if (director.debug)
                    Toast.Show("Reference is not null, it's " + currRef + "!", 2f);
                ReferenceSetter(); //set panel title and reset button state
                player.SetActive(true); //show panel

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
            player.SetActive(false); //hide panel
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

    void ExpandButtonPress()
    {
        fullPlayer.PlayerSetup();
        player.SetActive(false);
    }
}