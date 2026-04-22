using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GameTutorialSystem
{
    public class UITextBubble : MonoBehaviour
    {
        [SerializeField] float _typewriterSpeed = 25f;
        [SerializeField] RectTransform _rect;
        [SerializeField] Vector2 _heightRange;
        TMP_Text _textComp;
        float _fontSize;


        public void Init()
        {
            _textComp = this.GetComponentInChildren<TMP_Text>();
            _fontSize = _textComp.fontSize;
            _textComp.fontSizeMax = _fontSize;
        }

        void BestFit(string content)
        {
            _textComp.enableAutoSizing = false;
            _textComp.fontSize = _fontSize;
            _textComp.text = content;
            var hight = _textComp.preferredHeight;

            if (hight < _heightRange.x)
            {
                hight = _heightRange.x;
            }
            else if (hight > _heightRange.y)
            {
                _textComp.enableAutoSizing = true;
                hight = _heightRange.y;
            }

            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, hight + 10f);
        }

        public async UniTask Play(string line)
        {
            BestFit(line);
            if (this.gameObject.activeSelf == false) this.gameObject.SetActive(true);

            float t = 0;
            int charIndex = 0;
            while (charIndex < line.Length)
            {
                t += Time.deltaTime * _typewriterSpeed;
                charIndex = Mathf.FloorToInt(t);
                charIndex = Mathf.Clamp(charIndex, 0, line.Length);
                _textComp.text = line.Substring(0, charIndex);
                await UniTask.Yield();
            }

            _textComp.text = line;
        }
    }
}