using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RVN_SceneManager : RVN_Singleton<RVN_SceneManager>
{
    [SerializeField] private RVN_LevelManager currentLevel;

    [SerializeField] private CanvasGroup loadingScreen;

    public static Action ToDoAfterLoad;

    public static RVN_LevelManager CurrentLevel => instance.currentLevel;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TimerManager.CreateRealTimer(Time.deltaTime, () => OnSceneLoaded(null));
    }

    public static void LoadMainMenu()
    {
        instance.LaunchSceneLoading(0, null);
    }

    public static void LoadBattle(RVN_LevelManager newLevel)
    {
        instance.currentLevel = newLevel;

        instance.LaunchSceneLoading(1, null);
    }

    public static void LoadScene(int sceneIndex)
    {
        instance.LaunchSceneLoading(sceneIndex, null);
    }

    private void LaunchSceneLoading(int sceneIndex, Action callback)
    {
        StartCoroutine(LoadAsyncScene(sceneIndex, callback));
    }

    private IEnumerator LoadAsyncScene(int sceneIndex, Action callback)
    {
        loadingScreen.alpha = 1;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        OnSceneLoaded(callback);
    }

    private void OnSceneLoaded(Action callback)
    {
        ToDoAfterLoad?.Invoke();
        ToDoAfterLoad = null;

        callback?.Invoke();

        loadingScreen.alpha = 0;
    }
}
