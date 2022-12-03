using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
[CreateAssetMenu(fileName = "AllMenus", menuName = "Game/All UI Menus", order = 1)]
public class AllMenus : ScriptableObject
{
    [SerializeField] private List<string> _menuNames = new List<string>();

    [SerializeField] private List<GameObject> _menuPrefabs = new List<GameObject>();

    public GameObject GetPrefabByName(string name)
    {
        Debug.Log(name);

        foreach (var menuname in _menuNames)
        {
            Debug.Log(menuname);
        }
        int i = _menuNames.FindIndex(x => x == name);
        if (i == -1)
            return null;

        return _menuPrefabs[i];
    }

#if UNITY_EDITOR
    public void UpdatePanelList()
    {
        var results = AssetDatabase.FindAssets("t:prefab Panel_")
            .Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(x)))
            .Where(x => x.GetComponent<UIPanel>() != null)
            .ToList();

        _menuPrefabs.Clear();

        foreach (var result in results)
        {
            _menuPrefabs.Add(result);
            UIPanel panel = result.GetComponent<UIPanel>();
        }

        _menuNames.Clear();
        foreach (var menu in _menuPrefabs)
        {
            Type t = menu.GetComponent<UIPanel>().GetType();
            _menuNames.Add(t.Name);
        }
    }

    public void OnValidate()
    {
        _menuPrefabs = _menuPrefabs.Where(x => x != null).ToList();
        _menuPrefabs = _menuPrefabs.GroupBy(x => x.GetHashCode()).Select(y => y.FirstOrDefault()).ToList();

        _menuNames.Clear();
        foreach (var menu in _menuPrefabs)
        {
            Type t = menu.GetComponent<UIPanel>().GetType();
            _menuNames.Add(t.Name);
        }
    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(AllMenus))]
public class AllMenusEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);

        if (GUILayout.Button("Update Menu List"))
        {
            var allMenus = (AllMenus) target;
            allMenus.UpdatePanelList();
        }
    }
}
#endif
