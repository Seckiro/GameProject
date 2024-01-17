using System;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class UIConfig
{
    public Camera camera;
    public Transform uiCanvasTransform;
    public EventSystem eventSystem;
    public RectTransform uiCanvasRectTransform;
}