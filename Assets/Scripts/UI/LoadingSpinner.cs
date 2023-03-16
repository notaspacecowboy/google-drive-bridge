using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinner : MonoBehaviour
{
    private RectTransform _rect;

    private RectTransform Rect
    {
        get
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
            return _rect;
        }
    }

    private bool _isActive = false;

    [SerializeField]
    private float _rotateSpeed = 200f;


    private void Update()
    {
        if (!_isActive)
            return;

        Rect.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
    }

    public void Show()
    {
        Rect.gameObject.SetActive(true);
        _isActive = true;
    }


    public void Hide()
    {
        Rect.gameObject.SetActive(false);
        _isActive = false;
    }
}
