using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Joss.Helpers;

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

    private Director director;

    void Awake()
    {
        director = FindObjectOfType<Director>();

        infoButton.onClick.AddListener(LoadInfo);
        arButton.onClick.AddListener(LoadAR);
        mapButton.onClick.AddListener(LoadMap);
        debugButton.onClick.AddListener(LoadDebug);

        SceneLauncher.StoreSceneHistory();
    }

    void Start()
    {
        director.debug = false;
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
        director.debug = true;
        SceneLauncher.GoToScene(1);
    }
}
