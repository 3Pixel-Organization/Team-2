using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    public GameObject loadingPanel;
    public Slider loadingBar; 

    public enum GameState { GamePause, GameWon, GameLost};
    public event EventHandler OnPlayerDeathEvent;

    // subscribe to this whenever you want to do something when level starts
    public UnityAction OnLevelStartedEvent;
    // subscribe to this whenever you want to do something when level ends
    public UnityAction OnLevelLostEvent;
    // subscribe to this whenever you want to do something when the player wins
    public UnityAction OnLevelWonEvent;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[GameManager] There is more than one GM Instance");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        OnPlayerDeathEvent += OnPlayerDeath;
        OnLevelLostEvent += OnLevelLostManager;
        OnLevelWonEvent += OnLevelWonManager;

        SaveManager.Instance.playerData.SyncPlayerData();
    }

    private void Update()
    {
        #region Scene Sound

        Scene currentScene = SceneManager.GetActiveScene();
        PlaySceneSound(currentScene.name);

        #endregion
    }

    #region Sound

    private void PlaySceneSound(string sceneName)
    {
        var sound = AudioManager.Instance.IsPlayingSound(sceneName + "Sound_1");
        if (!sound)
        {
            sound = AudioManager.Instance.IsPlayingSound(sceneName + "Sound_2");
            if (!sound)
            {
                var random = UnityEngine.Random.Range(0, 1);
                if (random < 0.5f)
                {
                    AudioManager.Instance.Play(sceneName + "Sound_1");
                }
                else
                {
                    AudioManager.Instance.Play(sceneName + "Sound_2");
                }
            }
        }
    }

    #endregion



    #region Scene Changes

    public void GoToHomeScene(){
        StartCoroutine(AsyncLoadScene("HomeScene"));
    }

    public void GoToGameScene(){
        StartCoroutine(AsyncLoadScene("Desert 2"));
    }

    public void LoadUIGameplay(){
        SceneManager.LoadScene("UIGameplay", LoadSceneMode.Additive);
    }

    private IEnumerator AsyncLoadScene(string loadedScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadedScene);
        loadingPanel.SetActive(true);
        operation.allowSceneActivation = false;
        float targetValue = 0;
        float currentValue = 0;
        while (!Mathf.Approximately(currentValue, 1))
        {
            targetValue = operation.progress/0.9f;
            currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime);
            loadingBar.value = currentValue;
            yield return null;
        }
        operation.allowSceneActivation = true;
        yield return new WaitForSeconds(1);
        loadingPanel.SetActive(false);
    }

    public void GoToRestartScene()
    {
        // SceneChange("GameOver");
        // // player loses everything he earned
        // OnLevelLostEvent?.Invoke();
    }

    // public void GoToGameScene()
    // {
    //     SceneChange("Desert");
    //     OnLevelStartedEvent?.Invoke();
    // }

    // public void GoToMainMenu()
    // {
    //     SceneChange("MainMenu");
    //     OnLevelLostEvent?.Invoke();
    // }

    // private void SceneChange(string sceneName)
    // {
    //     //button animation and other things

    //     if (!_isChangingToLoadScene)
    //     {
    //         StartCoroutine(ChangeSceneOnLoad(sceneName));
    //     }
    // }

    // private bool _isChangingToLoadScene;

    // IEnumerator ChangeSceneOnLoad(string sceneName)
    // {
    //     Scene sceneToUnload = SceneManager.GetActiveScene();
    //     if (SceneManager.GetSceneByName("Scene") == sceneToUnload)
    //         yield break;

    //     if (sceneName == "GameOver")
    //     {
    //         SceneManager.LoadSceneAsync(sceneName);
    //         yield break;
    //     }
    //     _isChangingToLoadScene = true;

    //     SceneManager.LoadScene("LoadScene");

    //     _isChangingToLoadScene = false;

    //     yield return null; //wait one frame so the singleton can be loaded in the StatusBar script

    //     LoadSceneBar.Instance.LoadScene(sceneName);
    // }

    #endregion

    #region End level changes

    public void OnLevelLostManager()
    {
        
    }

    public void OnLevelWonManager()
    {
        OnLevelWonEvent.Invoke();
        // GoToMainMenu();
        GoToHomeScene();
    }

    #endregion

    #region Player Death

    private void OnPlayerDeath(object sender, EventArgs e)
    {
        // SceneChange("GameOver");
    }

    public void DeathEventCall()
    {
        OnPlayerDeathEvent?.Invoke(this, EventArgs.Empty);
    }

    #endregion

}
