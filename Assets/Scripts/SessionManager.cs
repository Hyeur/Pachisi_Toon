using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    [SerializeField] protected GameObject _BackgroundCanvas;
    [SerializeField] protected GameObject _MenuCanvas;
    [SerializeField] protected GameObject _SettingCanvas;
    [SerializeField] protected GameObject _RuleCanvas;
    [SerializeField] protected GameObject _loaderCanvas;
    [SerializeField] protected Image _progressBar;

    public bool isOpened = false;

    public string playerCount;
    private float _target;

    [Header("Audio")] 
    [SerializeField] protected AudioSource _musicSource;
    [SerializeField] protected AudioSource _effectSource;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerCount = "2";
    }

    private void Start()
    {
        _SettingCanvas.SetActive(false);
        isOpened = _SettingCanvas && _SettingCanvas.activeSelf;
    }

    public void playMusic(AudioClip clip)
    {
        _musicSource.PlayOneShot(clip);
    }
    public void playSound(AudioClip clip,float adjust)
    {
        float currentVolume = _effectSource.volume;
        if (adjust > 0)
        {
            _effectSource.volume += adjust;
        }

        if (adjust < 0)
        {
            _effectSource.volume -= adjust;
        }
        _effectSource.PlayOneShot(clip);
        
        _effectSource.volume = currentVolume;
    }

    public void stopSound()
    {
        _effectSource.Stop();
    }

    public void setMusicVolume(int level)
    {
        _musicSource.volume = level * 0.15f;
    }    
    public void setSoundVolume(int level)
    {
        _effectSource.volume = level * 0.20f;
    }

    public void toggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }    
    public void toggleSound()
    {
        _effectSource.mute = !_effectSource.mute;
    }

    public async void loadScene(string sceneName)
    {
        _progressBar.fillAmount = 0;
        _target = 0;
        
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        
        _BackgroundCanvas.SetActive(true);
        _loaderCanvas.SetActive(true);
        _SettingCanvas.SetActive(false);
        _RuleCanvas.SetActive(false);

        if (_MenuCanvas)
        {
            _MenuCanvas.SetActive(!_MenuCanvas.activeSelf);
        }
        else
        {
            _BackgroundCanvas.GetComponent<Canvas>().sortingOrder = 3;
        }

        do
        {
            await Task.Delay(200);
            _target = scene.progress;
        } while (scene.progress < 0.9f);
        
        scene.allowSceneActivation = true;
        await Task.Delay(1000);

        _BackgroundCanvas.GetComponent<Canvas>().sortingOrder = 0;
        
        _loaderCanvas.SetActive(false);
        if (sceneName.Contains("Game"))
        {
            _BackgroundCanvas.SetActive(false);
            isOpened = false;
        }
    }

    void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }

    public void toggleSettingPanel()
    {
        if (_SettingCanvas.activeInHierarchy)
        {
            _SettingCanvas.SetActive(false);
            isOpened = false;
        }
        else
        {
            _SettingCanvas.SetActive(true);
            isOpened = true;
        }
    }
}
