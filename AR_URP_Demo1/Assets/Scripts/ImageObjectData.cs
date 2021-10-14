using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class ImageObjectData : MonoBehaviour
{
    private ImageTrackerManager imageTrackerManager;
    [SerializeField]
    private TextAsset jsonData;
    [SerializeField]
    private GameObject objectPrefab;

    private string prefabName;
    private string animationName;
    private string description;
    private string description2;

    public GameObject miniPlayer;
    public GameObject fullPlayer;

    //{
    ////[SerializeField]
    ////private Animation objectAnimation;
    ////[SerializeField]
    ////private string[][] descriptions;
    ////[SerializeField]
    ////private List<Image> objectPhotos;
    //}

    private void Awake()
    {
        imageTrackerManager = FindObjectOfType<ImageTrackerManager>();
    }
    private void Start()
    {
        imageTrackerManager.ImageTrackedEvent += CheckPlayerState;
    }

    private void OnDestroy()
    {
        imageTrackerManager.ImageTrackedEvent -= CheckPlayerState;
    }

    void CheckPlayerState() 
    {
        if (!miniPlayer.activeSelf || !fullPlayer.activeSelf) //only need to parse data when player isn't showing
        {
            ParseData();
        }
    }

    void ParseData()
    {
        string objectName = imageTrackerManager.currentRef;
        JSONNode n = JSON.Parse(jsonData.text);

        if (objectName != null)
        {
            prefabName = n[objectName]["prefab"].Value;
            animationName = n[objectName]["animation"].Value;
            description = n[objectName]["description"].Value;
            description2 = n[objectName]["description2"].Value;
        }
        else
            Debug.LogError("Object reference not found. Unable to parse JSON data!");
    }

    void SetObjects()
    {
        if (prefabName != string.Empty)
        {
            objectPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/HUBB/" + prefabName));
        }
        if (animationName != string.Empty)
        {

        }
        if (description != string.Empty)
        {

        }
        if (description2 != string.Empty)
        {

        }
    }
}
