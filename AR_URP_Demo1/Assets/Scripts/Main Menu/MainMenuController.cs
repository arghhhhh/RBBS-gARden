using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Joss.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button infoButton;
    [SerializeField]
    private Button arButton;
    [SerializeField]
    private Button mapButton;
    [SerializeField]
    private Button debugButton;

    public static DebugUIManager debugger;

    void Awake()
    {
        infoButton.onClick.AddListener(LoadInfo);
        arButton.onClick.AddListener(LoadAR);
        mapButton.onClick.AddListener(LoadMap);
        debugButton.onClick.AddListener(LoadDebug);

        SceneLauncher.StoreSceneHistory();
    }

    void LoadInfo() 
    {
        SceneLauncher.GoToScene(1);
    }

    void LoadAR()
    {
        SceneLauncher.GoToScene(1);
    }

    void LoadMap()
    {
        SceneLauncher.GoToScene(1);
    }

    void LoadDebug()
    {
        SceneLauncher.GoToScene(2);
    }
}
