using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackerManager : MonoBehaviour
{
    [Header("The length of this list must match the number of images in Reference Image Library")]
    [SerializeField]
    private List<GameObject> ObjectsToPlace;

    private int refImageCount;
    public Dictionary<string, GameObject> allObjects;

    //create the “trackable” manager to detect 2D images
    private ARTrackedImageManager arTrackedImageManager;
    private IReferenceImageLibrary refLibrary;

    //text used for debugging
    public Text debugText;
    public Text debugTextValue;

    public delegate void ImageTracked();
    public event ImageTracked ImageTrackedEvent;
    public delegate void ImageLost();
    public event ImageTracked ImageLostEvent;
    public static bool tracked;

    public string currentRef { get; private set; }

    private FullPlayer fullPlayer;

    void Awake()
    {
        //initialized tracked image manager  
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }


    //when the tracked image manager is enabled add binding to the tracked 
    //image changed event handler by calling a method to iterate through 
    //image reference’s changes 
    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    //when the tracked image manager is disabled remove binding to the 
    //tracked image changed event handler by calling a method to iterate 
    //through image reference’s changes
    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void Start()
    {
        fullPlayer = FindObjectOfType<FullPlayer>();
        refLibrary = arTrackedImageManager.referenceLibrary;
        refImageCount = refLibrary.count;
        LoadObjectDictionary();
    }

    void LoadObjectDictionary()
    {
        allObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < refImageCount; i++)
        {
            GameObject newOverlay = ObjectsToPlace[i];
            //check if the object is prefab and need to be instantiated
            if (ObjectsToPlace[i].gameObject.scene.rootCount == 0)
            {
                newOverlay = Instantiate(ObjectsToPlace[i], transform.localPosition, Quaternion.identity);
            }
            allObjects.Add(refLibrary[i].name, newOverlay);
            newOverlay.SetActive(false);
        }
    }


    void ActivateTrackedObject(string imageName)
    {
        Debug.Log("Tracked the target: " + imageName);
        allObjects[imageName].SetActive(true); //need to disable this as well to prevent prefabs from loading smh

        //DEBUG
        {
            //if (debugText || debugTextValue) //check if debug text has been assigned in inspector
            //{
            //debugText.text = imageName;
            //}
        }
    }

    public void UpdateTrackedObject(ARTrackedImage trackedImage)
    {
        //if tracked image tracking state is comparable to tracking
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if (!tracked)
            {
                currentRef = trackedImage.referenceImage.name; //this reference will tell the IT Panel which object has most recently been tracked

                if (ImageTrackedEvent != null) //Fires only when an image is newly tracked
                {
                    ImageTrackedEvent();
                }
                tracked = true;
            }

            //AR
            if (!fullPlayer.shield.activeSelf) //hide tracked image prefab when fullscreen player is open
            {
                allObjects[trackedImage.referenceImage.name].SetActive(true); //set ar prefab object to active
                allObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                allObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
                allObjects[trackedImage.referenceImage.name].transform.Rotate(90, 0, 0); //rotate image prefab to correct orientation
            }
        }

        else //tracking state is limited or none
        {
            if (tracked)
            {
                currentRef = null;

                if (ImageLostEvent != null) //Fires only when tracking is newly lost
                {
                    ImageLostEvent();
                }
                tracked = false;
            }

            //AR
            allObjects[trackedImage.referenceImage.name].SetActive(false);
        }

        //DEBUG
        {
        //if (debugText || debugTextValue) //check if debug text has been assigned in inspector
        //{
        //    debugText.text = trackedImage.referenceImage.name;
        //    debugTextValue.text = "Tracking state: " + trackedImage.trackingState.ToString();
        //}
        }
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    //create an event system that notifies listener scripts on other objects when something is changed here
    {
        // for each tracked image that has been added
        foreach (var addedImage in args.added)
        {
            ActivateTrackedObject(addedImage.referenceImage.name);
        }

        // for each tracked image that has been updated
        foreach (var updated in args.updated)
        {
            //throw tracked image to check tracking state
            UpdateTrackedObject(updated);
        }

        // for each tracked image that has been removed  
        foreach (var trackedImage in args.removed)
        {
            // destroy the AR object associated with the tracked image
            Destroy(trackedImage.gameObject);
        }
    }

}
