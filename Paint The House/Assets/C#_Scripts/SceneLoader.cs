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

    public void reloadScene()
    {
        Scene currScene = SceneManager.GetActiveScene();

        StartCoroutine(loadSceneAsync(currScene.name));
    }

    public void loadScene(string sceneName)
    {
        StartCoroutine(loadSceneAsync(sceneName));
    }

    IEnumerator loadSceneAsync(string sceneName)
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
