using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] public bool _isOut = false;
    
    [SerializeField] public bool _isReady;
    [SerializeField] public bool _isFinish = false;
    [SerializeField] public bool _isMoving = false;
    [SerializeField] public bool _isMoved = false;
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    [SerializeField] protected Pad currentPad;

    public Team Team;

    public Outline outline;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        _rigidbody.freezeRotation = !(state == GameManager.GameState.RollTheDice);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void Start()
    {
        _isOut = false;
        _isReady = false;
        try
        {
            outline = GetComponent<Outline>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
        try
        {
            _boxCollider = GetComponent<BoxCollider>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }

        Team = GetComponentInParent<Team>();

        recoveryToCurrentPad(.5f);

        if (currentPad)
        {
            currentPad.setPawnCaptured(this);
        }

    }
    
    void FixedUpdate()
    {

        updateOutStatus();

        updateReadyStatus(currentPad);
        
    }

    public Rigidbody getRb()
    {
        return _rigidbody;
    }

    public void setCurrentPad(Pad destinationPad = null)
    {
        if (currentPad)
        {
            currentPad.setPawnCaptured(null);
        }
        
        if (destinationPad)
        {
            currentPad = destinationPad;
        }
        else
        {
            currentPad = null;
        }
    }
    
    public Pad getCurrentPad()
    {
        return currentPad;
    }
    
    public void setPawnStatus(bool isOut)
    {
        if (!_isOut.Equals(isOut))
        {
            _isOut = isOut;
        }
        
    }
    private void updateReadyStatus(Pad _currentPad)
    {
        if (!_currentPad)
        {
            _isReady = false;
            return;
        }
        if ( (_currentPad.actualPos - transform.position).sqrMagnitude > Mathf.Pow(0.3f,0.3f) || Mathf.Abs(transform.rotation.eulerAngles.x) > 10 || Mathf.Abs(transform.rotation.eulerAngles.z) > 10)
        {
            if (_isReady)
            {
                _isReady = false;
            }
        }
        else
        {
            if (!_isReady)
            {
                _isReady = true;
            }
        }
    }

    private void updateOutStatus()
    {
        _isOut = currentPad ? true : false;
    }

    public async Task recoveryToCurrentPad(float duration)
    {
        
        if (!_isReady)
        {
            _boxCollider.enabled = false;
            _rigidbody.useGravity = false;
            if (currentPad)
            {
                await transform.DORotate(new Vector3( 0, transform.rotation.y, 0), 1f).SetEase(Ease.InSine).AsyncWaitForCompletion();
                await transform.DOJump(currentPad.actualPos, 1,1,duration).SetEase(Ease.InSine).AsyncWaitForCompletion();
                _boxCollider.enabled = true;
                _rigidbody.useGravity = true;
                // Debug.Log($"Respawn {name} to {currentPad}");
            }
            else
            {
                await sentToHome(duration);
                // Debug.Log($"Respawn {name} to {Team.name}");
            }
        }
    }

    public async Task sentToHome(float duration)
    {
        var home = Team.transform.position;
        home = new Vector3(Mathf.Sign(home.x), Mathf.Sign(home.y),
            Mathf.Sign(home.z));
        home = Vector3.Scale(new Vector3(6, 1, 6), home);
        
        if (Vector3.Distance(home,transform.position) < 4.5f)
        {
            _boxCollider.enabled = true;
            _rigidbody.useGravity = true;
            return;
        };

        _boxCollider.enabled = false; 
        _rigidbody.useGravity = false;
        transform.rotation = Quaternion.Euler(0,0,0);
        await transform.DOMove(home,duration).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            _boxCollider.enabled = true;
            _rigidbody.useGravity = true;
        }).AsyncWaitForCompletion();
    }
    
    
}
