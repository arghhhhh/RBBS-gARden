using UnityEngine;
using UnityEngine.UI;
using Joss.Helpers;

public class BackButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(BackButtonPress);
    }

    void BackButtonPress()
    {
        SceneLauncher.GoToScene(SceneLauncher.previousScene());
    }
}
