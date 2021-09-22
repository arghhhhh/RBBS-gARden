using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using ARLocation;
using System.Collections.Generic;

public class GPSLocation : MonoBehaviour
{
    public Text GPSStatus;
    public Text longitudeValue;
    public Text latitudeValue;
    public Text altitudeValue;
    public Text horizontalAccuracyValue;
    public Text timestampValue;
    public Text gameObjDistance;
    public Text gameObjDistance2;
    public GameObject[] sceneObjects;

    private float outOfRangeDistance = 8.0f;

    private void Awake()
    {
        //get location permission on android
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }

    void Start()
    {
        StartCoroutine(GPSLoc());
        Invoke("GetTaggedPrefabs", 0.1f);
    }

    IEnumerator GPSLoc()
    {
        //check if user has gps enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        //start gps service
        Input.location.Start();

        //wait until service is initialized
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        //service didn't initialize within 20 secs
        if (maxWait < 1)
        {
            GPSStatus.text = "GPS Timed out";
            yield break;
        }

        //connection failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Unable to determine GPS location";
            yield break;
        }
        else //access granted
        {
            GPSStatus.text = "Running";
            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
        }
    }

    private void UpdateGPSData()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            //GPS has been initialized
            GPSStatus.text = "Running";
            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            timestampValue.text = Input.location.lastData.timestamp.ToString();
            gameObjDistance.text = FindClosestObject()[0];
            gameObjDistance2.text = FindClosestObject()[1].ToString();
        }
        else
        {
            //service is stopped
            GPSStatus.text = "Stopped";
        }
    }

    private void GetTaggedPrefabs()
    {
        sceneObjects = GameObject.FindGameObjectsWithTag("Test");
    }

    private string[] FindClosestObject()
    {
        string[] closestObjData = new string[2];
        float closestObjDist = 10000f;
        foreach (GameObject obj in sceneObjects)
        {
            var dist = obj.GetComponent<PlaceAtLocation>().SceneDistance;

            HideFarawayObjects(obj, dist);

            //get distance of each game object. cycle thru, find closest object
            if (dist < closestObjDist)
            {
                closestObjData[0] = obj.name; //set name of closest object
                closestObjData[1] = dist.ToString(); //set distance to closest object distance
            }
        }
        return closestObjData;
    }
    void HideFarawayObjects(GameObject obj, float dist)
    {
        if (dist > outOfRangeDistance)
        {
            obj.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            obj.GetComponent<Renderer>().enabled = true;
        }
    }
}
