using UnityEngine;
public class Director : MonoBehaviour
{
    public bool debug;
    public string playingAudio = string.Empty;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Director");

        if (objs.Length > 1) //make sure only one Director exists in scene
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}