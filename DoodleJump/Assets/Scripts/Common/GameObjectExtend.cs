using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class GameObjectExtend
{
    public static T Find<T>(this Transform transform, string name) where T : Component
    {
        return transform.Find(name).GetComponent<T>();
    }

    public static T Find<T>(this GameObject gameObject, string name) where T : Component
    {
        return gameObject.transform.Find<T>(name);
    }

    public static Button AddListener(this Button button, UnityAction action)
    {
        button.onClick.AddListener(action);
        return button;
    }
}
