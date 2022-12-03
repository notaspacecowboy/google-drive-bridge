using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{

    [SerializeField] private Canvas _menuRoot;

    [SerializeField] private AllMenus _allMenus;

    private Dictionary<string, UIPanel> _states = new Dictionary<string, UIPanel>();

    public void Init()
    {
    }

    public async UniTask<T> PushAsync<T>() where T: UIPanel
    {
        Type type = typeof(T);
        string typeName = type.Name;
        GameObject panelPrefab = _allMenus.GetPrefabByName(typeName);
        if (!panelPrefab)
        {
            Debug.LogError($"panel with type {typeName} does not exist!");
            return null;
        }

        if (_states.ContainsKey(typeName) && _states[typeName] != null)
        {
            Debug.LogError($"panel with type {typeName} has already been activated in the scene!");
            return null;
        }

        var panelGO = Instantiate(panelPrefab);
        var panel = panelGO.GetComponent<UIPanel>();
        _states[typeName] = panel;

        panelGO.transform.parent = _menuRoot.transform;
        panel.transform.localPosition = Vector3.zero;
        panel.transform.localScale = Vector3.one;
        await panel.InitAsync();
        return panel as T;
    }

    public async UniTask RemoveAsync<T>() where T : UIPanel
    {
        Type t = typeof(T);
        string typeName = t.Name;
        if (!_states.ContainsKey(typeName) || _states[typeName] == null)
            return;

        var panel = _states[typeName];
        await panel.CloseAsync();
        Destroy(panel.gameObject);
        _states[typeName] = null;
    }
}
