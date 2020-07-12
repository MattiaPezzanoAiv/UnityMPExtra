using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private List<UIElement> m_elements;

    private readonly Dictionary<Type, UIElement> m_elementsMap = new Dictionary<Type, UIElement>();

    [SerializeField]
    private Transform m_elementsRoot;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void EnsureIsInstantiated<T>() where T : UIElement
    {
        if(m_elementsMap.ContainsKey(typeof(T)))
        {
            return;
        }

        UIElement prefab = null;
        foreach (var element in m_elements)
        {
            if(element.GetType() == typeof(T))
            {
                prefab = element;
                break;
            }
        }

        var e = Instantiate(prefab, m_elementsRoot);
        m_elementsMap.Add(typeof(T), e);
    }

    public T Show<T>() where T : UIElement
    {
        EnsureIsInstantiated<T>();

        if(m_elementsMap.TryGetValue(typeof(T), out var element))
        {
            element.Show();
            return (T)element;
        }

        return null;
    }

    public T ShowAsPopup<T>() where T : UIElement
    {
        EnsureIsInstantiated<T>();

        if (m_elementsMap.TryGetValue(typeof(T), out var element))
        {
            element.RectTransform.SetSiblingIndex(m_elementsRoot.childCount);       // push it in front of everything
            element.Show();
            return (T)element;
        }

        return null;
    }

    public void Hide<T>() where T : UIElement
    {
        if (m_elementsMap.TryGetValue(typeof(T), out var element))
        {
            element.Hide();
        }
    }

    public void HideAll()
    {
        foreach (var e in m_elementsMap)
        {
            e.Value.Hide();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UIManager))]
    private class UIManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Auto Assign Elements"))
            {
                (target as UIManager).AssignAllElementsFromProject();
            }
        }
    }

    private void AssignAllElementsFromProject()
    {
        try
        {
            var elements = new List<UIElement>();

            var guids = AssetDatabase.FindAssets($"t:prefab");
            foreach (var guid in guids)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<UIElement>(AssetDatabase.GUIDToAssetPath(guid));
                if (prefab)
                {
                    elements.Add(prefab);
                }
            }

            m_elements = elements;
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
        
    }
#endif
}
