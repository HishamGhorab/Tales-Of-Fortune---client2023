using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TOFLoadManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image bar;

    private static TOFLoadManager _singleton;
    public static TOFLoadManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFLoadManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;

        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Additive);
    }


    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadSession()
    {
        loadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync("Main Menu"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive));
        
        StartCoroutine(GetSceneLoadProgress());
    }

    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count);

                bar.fillAmount = totalSceneProgress;

                yield return null;
            }
        }

        loadingScreen.gameObject.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
    }
}
