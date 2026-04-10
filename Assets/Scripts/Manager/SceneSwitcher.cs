using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float speed = 1f;
    bool isSwitching = false;

    private void Awake()
    {
        canvasGroup.alpha = 1f;
    }
    private void Start()
    {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= speed * Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

    IEnumerator FadeOut(string sceneName)
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += speed * Time.unscaledDeltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }


    //public void SceneSwitch(string sceneName)
    //{
    //    StartCoroutine(FadeOut(sceneName));
    //}

    public void SceneSwitch(string sceneName)
    {
        if (isSwitching) return;
        isSwitching = true;
        StartCoroutine(FadeOut(sceneName));
    }

    public void QuitGame()
    {
        //Debug.Log("Quit");
        Application.Quit();
    }
}
