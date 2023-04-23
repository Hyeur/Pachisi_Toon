using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    protected RawImage _rawImageImage;

    [SerializeField] protected Vector2 ScrollDir = Vector2.zero;
    void Start()
    {
        _rawImageImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        _rawImageImage.uvRect = new Rect(_rawImageImage.uvRect.x + ScrollDir.x * Time.deltaTime,
                                        _rawImageImage.uvRect.y + ScrollDir.y * Time.deltaTime,
                                        _rawImageImage.uvRect.width,_rawImageImage.uvRect.height);
    }
}
