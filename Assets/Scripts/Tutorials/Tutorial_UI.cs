using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameTutorialSystem
{
    public class Tutorial_UI : Tutorial
    {
        // Start is called before the first frame update
        public override async UniTask Play()
        {
            var addBtn = FindGO("UIMain/Button");
            UI.GetFocusPoint(addBtn, FocusShape.Rectangle).MakeHole().AddTip("Click this button");
            await WaitBtnClick(addBtn.GetComponent<Button>());

            OnFinished();
        }
    }
}
