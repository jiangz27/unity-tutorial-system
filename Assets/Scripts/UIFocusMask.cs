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

        public UIFocusMask Open()
        {
            SetAlpha(true);
            return this;
        }

        public UIFocusMask Hide()
        {
            SetAlpha(false);
            return this;
        }

        public void Add(UIFocusPoint point)
        {
            _points.Add(point);
        }

        public void Clear()
        {
            _points.Clear();
        }

        void SetAlpha(bool b)
        {
            var color = _image.color;
            color.a = b ? _alpha : 0f;
            _image.color = color;
        }

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            /*
            // 将屏幕点击点 sp 转换为 holeRect 内部的局部点
            bool isInsideHole = RectTransformUtility.RectangleContainsScreenPoint(holeRect, sp, eventCamera);

            // 如果点击在洞里 (isInsideHole为true)，我们需要返回 false (表示射线不被黑幕阻挡，穿透过去)
            // 如果点击在洞外 (isInsideHole为false)，我们需要返回 true (表示被黑幕阻挡)
            return !isInsideHole;*/

            // 1. 先判断点是否在 RectTransform 的矩形范围内
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