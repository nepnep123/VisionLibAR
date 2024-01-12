using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] GameObject introBackground;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(SceneLoader("Humvee"));
    }


    #region BACKGROUND 

    IEnumerator SceneLoader(string scene)
    {
        yield return new WaitForSeconds(1f);
        LoadingSceneManager.LoadScene(scene);
    }

    #endregion
}
