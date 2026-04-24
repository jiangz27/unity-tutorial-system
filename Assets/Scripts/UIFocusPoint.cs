using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;

namespace GameTutorialSystem
{
    public enum FocusShape
    {
        Circle,
        Rectangle,
    }

    public enum RectanglePosition
    {
        BottomLeft = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomRight = 3,
    }

    public class UIFocusPoint : MonoBehaviour
    {
        Transform _target;
        RectTransform _targetRect;
        Canvas _canvas;

        [SerializeField] RectTransform _shapeCircle;
        [SerializeField] RectTransform _shapeRectangle;
        [SerializeField] RectTransform _dialogueBox;
        [SerializeField] TMP_Text _dialogueText;
        [SerializeField] Button _btn;

        public bool AcceptRaycast { get; private set; }
        public RectTransform CurrentShape { get; private set; }
        public Button ConfirmBtn => _btn;

        public void Ready()
        {
            _shapeCircle.gameObject.SetActive(false);
            _shapeRectangle.gameObject.SetActive(false);
            _dialogueBox.gameObject.SetActive(false);
            _btn.gameObject.SetActive(false);
            _canvas = this.GetComponentInParent<Canvas>();
            AcceptRaycast = false;
            CurrentShape = null;
            this.gameObject.SetActive(true);
        }

        public void Init(GameObject GO, FocusShape shape)
        {
            _targetRect = GO.GetComponent<RectTransform>();
            CurrentShape = shape == FocusShape.Circle ? _shapeCircle : _shapeRectangle;

            if (_targetRect != null)
            {
                Vector3[] worldCorners = new Vector3[4];
                _targetRect.GetWorldCorners(worldCorners);

                Vector2 center = (worldCorners[0] + worldCorners[2]) / 2f;
                CurrentShape.position = center;
                var localPosition = CurrentShape.localPosition;
                localPosition.z = 0f;
                CurrentShape.localPosition = localPosition;

                var size = _targetRect.rect.size;
                if (shape == FocusShape.Circle)
                {
                    var diameter = math.sqrt(size.x * size.x + size.y * size.y);
                    CurrentShape.sizeDelta = new Vector2(diameter, diameter);
                }
                else
                {
                    var padding = Vector2.one * 10f;
                    CurrentShape.sizeDelta = size + padding;
                }
            }
            else
            {
                _target = GO.transform;
                // 1. 获取物体在世界空间中的边界盒 (兼容2D和3D)
                Bounds worldBounds;
                if (!GetWorldBounds(_target, out worldBounds)) return;

                // 2. 将世界坐标的角点转换为屏幕坐标
                Vector3[] screenCorners = GetScreenCornersFromBounds(worldBounds);

                // 3. 计算屏幕空间中的最小/最大X和Y值
                float minX = float.MaxValue;
                float maxX = float.MinValue;
                float minY = float.MaxValue;
                float maxY = float.MinValue;

                foreach (Vector3 screenPoint in screenCorners)
                {
                    // 只处理在相机前方的点
                    if (screenPoint.z > 0)
                    {
                        minX = Mathf.Min(minX, screenPoint.x);
                        maxX = Mathf.Max(maxX, screenPoint.x);
                        minY = Mathf.Min(minY, screenPoint.y);
                        maxY = Mathf.Max(maxY, screenPoint.y);
                    }
                }

                var padding = 10f;
                minX -= padding;
                maxX += padding;
                minY -= padding;
                maxY += padding;

                Vector2 localMin, localMax;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvas.GetComponent<RectTransform>(),
                    new Vector2(minX, minY),
                    _canvas.worldCamera,
                    out localMin
                );
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvas.GetComponent<RectTransform>(),
                    new Vector2(maxX, maxY),
                    _canvas.worldCamera,
                    out localMax
                );

                CurrentShape.localPosition = (localMin + localMax) * 0.5f;
                CurrentShape.sizeDelta = (localMax - localMin) + Vector2.one * 10f;
            }

            CurrentShape.gameObject.SetActive(true);
        }

        public UIFocusPoint MakeHole()
        {
            AcceptRaycast = true;
            return this;
        }

        public UIFocusPoint AddTip(string line, RectanglePosition position = RectanglePosition.BottomRight)
        {
            _dialogueText.text = line;

            var offset = Vector2.zero;
            switch (position)
            {
                case RectanglePosition.BottomLeft: offset = Vector2.zero; break;
                case RectanglePosition.TopLeft: offset = new Vector2(0, 1); break;
                case RectanglePosition.TopRight: offset = new Vector2(1, 1); break;
                case RectanglePosition.BottomRight: offset = new Vector2(1, 0); break;
            }


            _dialogueBox.pivot = Vector2.one - offset;
            Vector3[] corners = new Vector3[4];
            CurrentShape.GetWorldCorners(corners);
            _dialogueBox.position = corners[(int)position];
            _dialogueBox.gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_dialogueBox);

            return this;
        }

        public UIFocusPoint AddConfirmBtn()
        {
            if (_dialogueBox.gameObject.activeSelf)
            {
                var corners = new Vector3[4];
                LayoutRebuilder.ForceRebuildLayoutImmediate(_dialogueBox);
                _dialogueBox.GetWorldCorners(corners);
                _btn.GetComponent<RectTransform>().position = corners[3];
                _btn.gameObject.SetActive(true);
            }

            return this;
        }

        /// <summary>
        /// 兼容获取2D或3D物体的世界边界
        /// </summary>
        private bool GetWorldBounds(Transform target, out Bounds bounds)
        {
            bounds = new Bounds();

            // 优先检查3D渲染器
            Renderer renderer3D = target.GetComponent<Renderer>();
            if (renderer3D != null)
            {
                bounds = renderer3D.bounds;
                return true;
            }

            // 如果没有3D渲染器，则检查2D精灵渲染器
            SpriteRenderer renderer2D = target.GetComponent<SpriteRenderer>();
            if (renderer2D != null)
            {
                bounds = renderer2D.bounds;
                return true;
            }

            Debug.LogWarning($"物体 {target.name} 既没有 Renderer 也没有 SpriteRenderer 组件，无法获取边界。");
            return false;
        }

        /// <summary>
        /// 从边界盒获取8个角点的世界坐标
        /// </summary>
        private Vector3[] GetBoundsCorners(Bounds bounds)
        {
            Vector3[] corners = new Vector3[8];
            Vector3 center = bounds.center;
            Vector3 size = bounds.size;
            Vector3 extents = size * 0.5f;

            corners[0] = center + new Vector3(-extents.x, -extents.y, -extents.z);
            corners[1] = center + new Vector3(extents.x, -extents.y, -extents.z);
            corners[2] = center + new Vector3(extents.x, extents.y, -extents.z);
            corners[3] = center + new Vector3(-extents.x, extents.y, -extents.z);
            corners[4] = center + new Vector3(-extents.x, -extents.y, extents.z);
            corners[5] = center + new Vector3(extents.x, -extents.y, extents.z);
            corners[6] = center + new Vector3(extents.x, extents.y, extents.z);
            corners[7] = center + new Vector3(-extents.x, extents.y, extents.z);

            return corners;
        }

        /// <summary>
        /// 将世界边界转换为屏幕坐标角点
        /// </summary>
        private Vector3[] GetScreenCornersFromBounds(Bounds worldBounds)
        {
            Vector3[] worldCorners = GetBoundsCorners(worldBounds);
            Vector3[] screenCorners = new Vector3[worldCorners.Length];
            for (int i = 0; i < worldCorners.Length; i++)
            {
                screenCorners[i] = _canvas.worldCamera.WorldToScreenPoint(worldCorners[i]);
            }
            return screenCorners;
        }
    }
}