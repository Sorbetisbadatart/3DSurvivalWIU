using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

    public void LoadScene(int SceneID)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneID);
    }
}
