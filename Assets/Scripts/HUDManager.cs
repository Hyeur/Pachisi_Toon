using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    
    private Dictionary<Team, Sprite> _teamAvatar = new Dictionary<Team, Sprite>();

    [SerializeField] private Image currentAvatar;

    public Canvas scoreBoard;
    void Awake()
    {
        HUDManager.Instance = this;
        
        GameManager.OnAfterGameStateChanged += afterGameManagerOnBeforeGameStateChanged;

        scoreBoard.gameObject.SetActive(false);
        
        loadAvatar();
    }

    private void Start()
    {
        try
        {
            if (currentAvatar == null)
            {
                currentAvatar = GameObject.Find("TeamImage").GetComponent<Image>();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    private void afterGameManagerOnBeforeGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.RollTheDice)
        {
            updateAvatar();
        }
    }

    private async void updateAvatar()
    {
        if (!GameManager.Instance)
        {
            Debug.Log("no GameManager.Instance");
            currentAvatar.color = new Color(0, 0, 0, 0);
            return;
        }
        if (GameManager.Instance.currentTeam)
        {
            var sequence = DOTween.Sequence();
            await sequence.Append(currentAvatar.rectTransform.DOAnchorPosX(280, .2f).SetEase(Ease.OutExpo))
                .AsyncWaitForCompletion();
            
            setAvatar();
            
            await sequence.Append(currentAvatar.rectTransform.DOAnchorPosX(0, .3f).SetEase(Ease.InExpo)).AsyncWaitForCompletion();
        }

    }

    private void setAvatar()
    {
        currentAvatar.color = Color.white;
        currentAvatar.sprite = _teamAvatar[GameManager.Instance.currentTeam];
    }
    
    private void addTeamAvatar(Team team,Sprite avatar)
    {
        _teamAvatar.Add(team,avatar);
    }
    
    private void loadAvatar()
    {
        foreach (var team in Spawner.Instance.playTeam)
        {
            switch (team.teamName)
            {
                case "Team1":
                    addTeamAvatar(team, Spawner.Instance.team1Avatar);
                    break;
                case "Team2":
                    addTeamAvatar(team, Spawner.Instance.team2Avatar);
                    break;
                case "Team3":
                   addTeamAvatar(team, Spawner.Instance.team3Avatar);
                    break;
                case "Team4":
                    addTeamAvatar(team, Spawner.Instance.team4Avatar);
                    break;
            }
        }
    }
    
    private void OnDestroy()
    {
        GameManager.OnAfterGameStateChanged -= afterGameManagerOnBeforeGameStateChanged;
    }
}
