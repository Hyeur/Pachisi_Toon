using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    [SerializeField] protected GameObject _MenuCanvas;
    [SerializeField] protected GameObject _loaderCanvas;
    [SerializeField] protected Image _progressBar;

    private float _target;
    
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
    }

    public async void LoadScene(string sceneName)
    {
        _progressBar.fillAmount = 0;
        _target = 0;
        
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        
        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);
        
        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        
        _loaderCanvas.SetActive(false);
    }

    void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }
}
