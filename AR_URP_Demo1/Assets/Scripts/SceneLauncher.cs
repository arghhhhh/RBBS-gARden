using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Joss.Helpers
{
    public class SceneLauncher
    {
        static List<int> sceneHistory = new List<int>();

        public static void GoToScene(int scene)
        {
            SceneManager.LoadScene(scene);
        }

        //add second function called StoreSceneHistory that adds the index of the current scene to a list each time a new scene is loaded
        //will eventually use this for the back button
        public static void StoreSceneHistory()
        {
            sceneHistory.Add(SceneManager.GetActiveScene().buildIndex);
        }

        public static int previousScene()
        {
            int prev = sceneHistory[sceneHistory.Count-2]; //count - 1 would be the most recent scene
            return prev;
        }
    }
}
