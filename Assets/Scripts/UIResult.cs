using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIResult : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Dictionary<int, Sprite> _diceSet;

    [Header("Dice Set")] 
    [SerializeField] private Sprite one;
    [SerializeField] private Sprite two;
    [SerializeField] private Sprite three;
    [SerializeField] private Sprite four;
    [SerializeField] private Sprite five;
    [SerializeField] private Sprite six;
    [Space]
    [Header("Result")] 
    [SerializeField] private Image result1;
    [SerializeField] private Image result2;

    [Space] [Header("Previous Result")] 
    [SerializeField] private RectTransform prevBoard;
    [SerializeField] private Image prevResult1;
    [SerializeField] private Image prevResult2;

    
    private void Awake()
    {
        GameManager.OnAfterGameStateChanged += afterGameManagerOnBeforeGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnAfterGameStateChanged -= afterGameManagerOnBeforeGameStateChanged;
    }

    private void afterGameManagerOnBeforeGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.PickAPawn || state == GameManager.GameState.RollTheDice)
        {
            updateUI();
        }
    }
    
    void Start()
    {
        _diceSet = new Dictionary<int, Sprite>()
        {
            {1,one},
            {2,two},
            {3,three},
            {4,four},
            {5,five},
            {6,six},
        };
        
    }
    private void updateUI()
    {
        if (DiceManager.Instance.isRolled)
        {
            var sequence = DOTween.Sequence();
            result1.sprite = _diceSet[DiceManager.Instance.twoResult[0]];
            result2.sprite = _diceSet[DiceManager.Instance.twoResult[1]];
            sequence.Append(result1.rectTransform.DOScale(new Vector3(.4f,.2f,1), .5f).SetEase(Ease.InExpo));
            sequence.Append(result2.rectTransform.DOScale(new Vector3(.4f,.2f,1), .5f).SetEase(Ease.InExpo));
        }
        else
        {
            var sequence = DOTween.Sequence();
            prevResult1.sprite = result1.sprite ? result1.sprite : null;
            prevResult2.sprite = result2.sprite ? result2.sprite : null;
            sequence.Append(result1.rectTransform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBounce));
            sequence.Append(result2.rectTransform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBounce));
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        prevBoard.DOAnchorPosY(- 57, .2f).SetEase(Ease.OutExpo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        prevBoard.DOAnchorPosY(0, .2f).SetEase(Ease.OutExpo);
    }
}
