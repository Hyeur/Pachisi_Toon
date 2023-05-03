using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadMenuBtn : BaseButton
{
    private void Awake()
    {
        SceneManager.activeSceneChanged += changedActiveScene;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= changedActiveScene;
    }

    private void changedActiveScene(Scene arg0, Scene arg1)
    {
        updateVisible();
    }

    void updateVisible()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    protected override void OnClick()
    {
        SessionManager.Instance.loadScene("MenuScene");
    }
}
