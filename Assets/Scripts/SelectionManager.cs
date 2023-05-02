using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;
    [SerializeField] private string[] SelectableTag;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Transform _selection;
    [SerializeField] private Transform _highlight;
    [SerializeField] private bool _active = false;
    
    private Camera _camera;

    public Pawn pawnSelected;
    
    private void Awake()
    {
        Instance = this;
        
        GameManager.OnBeforeGameStateChanged += beforeGameManagerOnBeforeGameStateChanged;
    }

    private void beforeGameManagerOnBeforeGameStateChanged(GameManager.GameState state)
    {
        _active = (state == GameManager.GameState.PickAPawn);
    }

    private void OnDestroy()
    {
        GameManager.OnBeforeGameStateChanged -= beforeGameManagerOnBeforeGameStateChanged;
    }
    
    void Start()
    {
        _camera = Camera.main;
        
        //add Outline componenet to Selectable GameObjects
        foreach (string _selectableTag in SelectableTag)
        {
            GameObject[] allGO = GameObject.FindGameObjectsWithTag(_selectableTag);
            foreach (GameObject gameObject in allGO)
            {
                if (gameObject.GetComponent<Outline>() == null)
                {
                    Outline outline = gameObject.AddComponent<Outline>();
                    outline.OutlineMode = Outline.Mode.OutlineVisible;
                    outline.OutlineWidth = 2f;
                    outline.enabled = false;
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        handleSelector();
    }

    private void handleSelector()
    {
        if (_camera != null && _active)
        {
            if (_highlight != null)
            {
                _highlight.gameObject.GetComponent<Outline>().enabled = false;
                _highlight = null;
            }
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = default;
            
            
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit))
            {
                _highlight = hit.transform;
                
                if (SelectableTag.Contains(_highlight.tag) && _highlight != _selection)
                {
                    if (_highlight.gameObject.GetComponent<Outline>() != null)
                    {
                        _highlight.gameObject.GetComponent<Outline>().enabled = true;
                    }
                    else
                    {
                        Outline outline = _highlight.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                        _highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.red;
                        _highlight.gameObject.GetComponent<Outline>().OutlineWidth = 2.0f;
                    }
                }
                else
                {
                    _highlight = null;
                }
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (_highlight)
                {
                    if (_selection != null)
                    {
                        _selection.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
                        _selection.gameObject.GetComponent<Outline>().enabled = false;
                    }
                    _selection = hit.transform;
                    if (_selection.CompareTag("Pawn"))
                    {
                        pawnSelected = _selection.gameObject.GetComponent<Pawn>();
                        GameManager.Instance.updateGameState(GameManager.GameState.Pawning);
                    }
                    _selection.gameObject.GetComponent<Outline>().OutlineColor = Color.red;
                    _selection.gameObject.GetComponent<Outline>().enabled = true;
                    _highlight = null;
                }
                else
                {
                    if (_selection)
                    {
                        _selection.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
                        _selection.gameObject.GetComponent<Outline>().enabled = false;
                        _selection = null;
                    }
                }
            }
        }
    }

    public void emptyPawnSelection()
    {
        pawnSelected = null;
    }
    private void handleSelection(RaycastHit hit)
    {
        
    }

    private void handleHighlight(Ray ray,RaycastHit hit)
    {
        
    }
}
