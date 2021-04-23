using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string SceneToLoad;

    public RectTransform LoadingOverlay;

    private AsyncOperation _sceneLoadingOperation;

    private void Start()
    {
        LoadingOverlay.gameObject.SetActive(false);

        _sceneLoadingOperation = SceneManager.LoadSceneAsync(SceneToLoad);
        _sceneLoadingOperation.allowSceneActivation = false;
    }

    public void LoadScene()
    {
        LoadingOverlay.gameObject.SetActive(true);

        _sceneLoadingOperation.allowSceneActivation = true;
    }
}
