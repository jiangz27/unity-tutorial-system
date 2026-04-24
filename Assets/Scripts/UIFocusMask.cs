using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameTutorialSystem
{
    public class UIFocusMask : MonoBehaviour, ICanvasRaycastFilter
    {
        List<UIFocusPoint> _points = new List<UIFocusPoint>();

        Image _image;
        float _alpha;

        void Awake()
        {
            _image = this.GetComponent<Image>();
            _alpha = _image.color.a;
        }

        public void Add(UIFocusPoint point)
        {
            _points.Add(point);
        }

        public void Show()
        {
            _SetAlpha(true);
        }

        public void Hide()
        {
            _SetAlpha(false);
        }

        private void _SetAlpha(bool b)
        {
            var color = _image.color;
            color.a = b ? _alpha : 0f;
            _image.color = color;
        }

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (_points.Count == 0) return true;

            foreach (var item in _points)
            {
                if (item.AcceptRaycast == false) continue;
                bool isInsideHole = RectTransformUtility.RectangleContainsScreenPoint(item.CurrentShape, screenPoint, eventCamera);
                if (isInsideHole) return false;
            }

            return true;
        }
    }
}