using UnityEngine;

public class Director : MonoBehaviour
{
    public bool debug;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}