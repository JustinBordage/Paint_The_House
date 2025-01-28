using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static bool SceneChanging = false;

    void Awake()
    {
        SceneChanging = false;
    }

    public void ReloadScene()
    {
        Scene currScene = SceneManager.GetActiveScene();

        StartCoroutine(LoadSceneAsync(currScene.name));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        SceneChanging = true;

        AsyncOperation sceneLoader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        sceneLoader.allowSceneActivation = false;

        while (!sceneLoader.isDone)
        {
            if (sceneLoader.progress >= 0.9f)
            {
                sceneLoader.allowSceneActivation = true;
            }

            yield return null;
        }

        yield break;
    }
}
