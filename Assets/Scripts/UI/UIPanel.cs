using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    private Canvas _canvas;

    public Canvas Canvas
    {
        get
        {
            if (_canvas != null)
                return _canvas;

            _canvas = GetComponent<Canvas>();
            if (!_canvas)
                _canvas = this.gameObject.AddComponent<Canvas>();

            return _canvas;
        }
    }

    private CanvasGroup _canvasGroup;

    public CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup != null)
                return _canvasGroup;

            _canvasGroup = GetComponent<CanvasGroup>();
            if (!_canvasGroup)
                _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

            return _canvasGroup;
        }
    }
    
    public virtual async UniTask InitAsync()
    {
    }

    public virtual async UniTask CloseAsync()
    {
    }

    public virtual void EnableInput()
    {

    }

    public virtual void DisableInput()
    {
             
    }
}


public class UIPanelData
{
    public virtual string PanelName
    {
        get { return "UIPanel"; }
    }
}