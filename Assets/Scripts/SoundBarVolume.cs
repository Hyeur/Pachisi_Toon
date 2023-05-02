using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SoundBarVolume : MonoBehaviour
{

    [SerializeField] private Image[] _bar;
    [SerializeField] private Sprite fillImage;
    [SerializeField] private Sprite emptyImage;
        
    [SerializeField] private Image checkBox;
    [SerializeField] private Sprite checkBoxFill;
    [SerializeField] private Sprite checkBoxEmpty;
    
    public bool isEnabled;

    void Start()
    {
        _bar = GetComponentsInChildren<Image>();
        checkBox = transform.parent.transform.GetComponentInChildren<Image>();
    }

    public void setVolume(int value)
    {
        for (int i = 1; i < _bar.Length + 1; i++)
        {
            if (i <= value)
            {
                _bar[i-1].sprite = fillImage;
            }
            else
            {
                _bar[i-1].sprite = emptyImage;
            }
        }
        
        SessionManager.Instance.setSoundVolume(value);

        if (!isEnabled)
        {
            toggleMute();
        }
    }

    public void toggleMute()
    {
        if (isEnabled)
        {
            checkBox.sprite = checkBoxEmpty;
            foreach (var bar in _bar)
            {
                bar.sprite = emptyImage;
            }
            SessionManager.Instance.setSoundVolume(0);
            isEnabled = false;
        }
        else
        {
            checkBox.sprite = checkBoxFill;
            if (!_bar.Any(bar => bar.sprite == fillImage))
            {
                setVolume(1);
            }
            isEnabled = true;
        }
    }
}
