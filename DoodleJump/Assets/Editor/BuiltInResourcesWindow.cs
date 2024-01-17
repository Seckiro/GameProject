using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class BuiltInResourcesWindow : EditorWindow
    {
        private struct Drawing
        {
            public Rect rect;
            public Action draw;
        }

        private bool _showingStyles = true;
        private bool _showingIcons = false;

        private float _scrollValue = 0;

        private string _search = string.Empty;

        private Rect _oldPosition;
        private Vector2 _currentCellMaxPos = Vector2.zero;

        private List<Drawing> _listDrawing = null;

        /// <summary>
        /// 根据高度排序
        /// </summary>
        private Dictionary<float, List<Drawing>> _dictDrawing = new Dictionary<float, List<Drawing>>();

        [MenuItem("Tools/GUIStyles Preview", priority = 4)]
        public static void ShowWindow()
        {
            GetWindow<BuiltInResourcesWindow>("GUIStyles Preview").Show();
        }

        void OnGUI()
        {
            if (position.width != _oldPosition.width && Event.current.type == EventType.Layout)
            {
                _listDrawing = null;
                _oldPosition = position;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Toggle(_showingStyles, "Styles", EditorStyles.toolbarButton) != _showingStyles)
            {
                _showingStyles = !_showingStyles;
                _showingIcons = !_showingStyles;
                _listDrawing = null;
            }

            if (GUILayout.Toggle(_showingIcons, "Icons", EditorStyles.toolbarButton) != _showingIcons)
            {
                _showingIcons = !_showingIcons;
                _showingStyles = !_showingIcons;
                _listDrawing = null;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            string newSearch = GUILayout.TextField(_search);
            if (newSearch != _search)
            {
                _search = newSearch;
                _listDrawing = null;
            }

            GUILayout.EndHorizontal();

            if (_listDrawing == null)
            {
                string lowerSearch = _search.ToLower();

                float cellMargins = 5.0f;
                float cellInterval = 20.0f;
                float marginsOffect = 15.0f;
                float contentInterval = 30.0f;

                Vector2 currentCellScale = Vector2.zero;

                Vector2 currentCellPos = new Vector2(cellMargins, cellMargins);

                GUIContent activeText = new GUIContent("active");
                GUIContent inactiveText = new GUIContent("inactive");

                _currentCellMaxPos = Vector2.zero;
                _listDrawing = new List<Drawing>();
                _dictDrawing = new Dictionary<float, List<Drawing>>();
                if (_showingStyles)
                {
                    foreach (GUIStyle skin in GUI.skin)
                    {
                        if (!string.IsNullOrEmpty(lowerSearch) && !skin.name.ToLower().Contains(lowerSearch))
                        {
                            continue;
                        }

                        skin.alignment = TextAnchor.MiddleLeft;

                        float cellWidth = Mathf.Max(GUI.skin.button.CalcSize(new GUIContent(skin.name)).x, skin.CalcSize(inactiveText).x + skin.CalcSize(activeText).x) + contentInterval;

                        float cellHeight = Mathf.Max(GUI.skin.button.CalcSize(new GUIContent(skin.name)).y, skin.CalcSize(inactiveText).y, skin.CalcSize(activeText).y) + contentInterval;

                        currentCellScale = new Vector2(cellWidth, cellHeight);

                        _currentCellMaxPos = new Vector2(currentCellPos.x + cellWidth, Mathf.Max(currentCellPos.y + cellHeight + (cellInterval / 2), _currentCellMaxPos.y));

                        if (_currentCellMaxPos.x + marginsOffect > position.width)
                        {
                            currentCellPos.x = cellMargins;
                            currentCellPos.y = _currentCellMaxPos.y;
                        }

                        Drawing draw = new Drawing();

                        draw.rect = new Rect(currentCellPos, currentCellScale);

                        cellWidth -= 8.0f;

                        draw.draw = () =>
                        {
                            if (GUILayout.Button(skin.name, GUILayout.Width(cellWidth)))
                            {
                                CopyText($"new GUIStyle(\"{skin.name}\");");
                            }
                            GUILayout.BeginHorizontal();
                            GUILayout.Toggle(true, activeText, skin, GUILayout.Width(cellWidth / 2));
                            GUILayout.Toggle(false, inactiveText, skin, GUILayout.Width(cellWidth / 2));
                            GUILayout.EndHorizontal();
                        };

                        currentCellPos.x += cellWidth + cellInterval;

                        _listDrawing.Add(draw);

                        //if (_dictDrawing.TryGetValue(cellHeight, out List<Drawing> listDrawing))
                        //{
                        //    listDrawing.Add(draw);
                        //}
                        //else
                        //{
                        //    _dictDrawing[cellHeight] = new List<Drawing>() { draw };
                        //}
                    }

                    //List<float> listDictDrawingKey = new List<float>();

                    //foreach (var item in _dictDrawing.Keys)
                    //{
                    //    listDictDrawingKey.Add(item);
                    //}

                    //listDictDrawingKey.Sort();

                    //foreach (var key in listDictDrawingKey)
                    //{
                    //    foreach (var item in _dictDrawing[key])
                    //    {
                    //        _listDrawing.Add(item);
                    //    }
                    //}
                }
            }

            Rect scrollRect = position;
            scrollRect.y = 42;// 上边 距离窗口的宽度
            scrollRect.width = 16;// 侧边 滑动条宽度
            scrollRect.height = position.height - scrollRect.y;
            scrollRect.x = position.width - scrollRect.width;

            // 窗口滑动数值
            _scrollValue = GUI.VerticalScrollbar(scrollRect, _scrollValue, scrollRect.height, 0.0f, _currentCellMaxPos.y);

            Rect scrollArea = new Rect(0, scrollRect.y, scrollRect.x, scrollRect.height);

            GUILayout.BeginArea(scrollArea);

            foreach (Drawing draw in _listDrawing)
            {
                Rect cellAreaRect = draw.rect;
                cellAreaRect.y -= _scrollValue;

                if (cellAreaRect.y + cellAreaRect.height > 0 && cellAreaRect.y < scrollRect.height)
                {
                    GUILayout.BeginArea(cellAreaRect, GUI.skin.textArea);
                    draw.draw();
                    GUILayout.EndArea();
                }
            }

            GUILayout.EndArea();
        }

        private void CopyText(string pText)
        {
            TextEditor editor = new TextEditor();
            editor.text = pText;
            editor.SelectAll();
            editor.Copy();
        }
    }
}
