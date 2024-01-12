using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;
    public static string nextScene;

    [SerializeField] Image progressBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        int i = Random.Range(0, 4);

        switch (i)
        {
            case 0:
                txt.text = "TIP.  AR은 항상 밝은 곳에서 실행하세요. 어두울수록 인식률이 낮습니다.";
                break;
            case 1:
                txt.text = "TIP.  타겟을 아웃라인에 맞게 정확하게 비춰주세요.";
                break;
            case 2:
                txt.text = "TIP.  타겟이 잘 잡히지 않으면 타겟의 주변환경을 정리해주세요.";
                break;
            case 3:
                txt.text = "로딩중입니다. 잠시만 기다려주세요.";
                break;
        }

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f) // op.progress < 0.9f
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                {
                    yield return new WaitForSeconds(2f);

                    progressBar.GetComponent<Animator>().SetTrigger("End");

                    yield return new WaitForSeconds(1.5f);

                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
