using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace GameTutorialSystem
{
    public class Tutorial_UI : Tutorial
    {
        public override async UniTask Play()
        {
            var addBtn = FindGO("UIMain/Button");
            UI.GetFocusPoint(addBtn, FocusShape.Rectangle).MakeHole().AddTip("Click this button", RectanglePosition.BottomLeft);
            await WaitBtnClick(addBtn.GetComponent<Button>());

            var countTxt = FindGO("UIMain/Count");
            var focusPoint = UI.GetFocusPoint(countTxt, FocusShape.Rectangle).AddTip("This value is changed").AddConfirmBtn();
            await WaitBtnClick(focusPoint.ConfirmBtn);

            OnFinished();
        }
    }
}
