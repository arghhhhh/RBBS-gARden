using UnityEngine;
using UnityEngine.UI;
using Joss.Helpers;

public class BackButton : MonoBehaviour
{
    private Director director;
    void Start()
    {
        director = FindObjectOfType<Director>();
        GetComponent<Button>().onClick.AddListener(BackButtonPress);
    }

    void BackButtonPress()
    {
        if (director.playingAudio != string.Empty)
        {
            director.GetComponent<AudioManager>().StopSound(director.playingAudio);
            director.playingAudio = string.Empty;
        }
        SceneLauncher.GoToScene(SceneLauncher.previousScene());
    }
}
