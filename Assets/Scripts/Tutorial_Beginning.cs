using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace GameTutorialSystem
{
    public class Tutorial_Beginning : Tutorial
    {
        public override async UniTask Play()
        {
            var GO1 = FindGO("Cube1");
            var point = UI.GetFocusPoint(GO1, FocusShape.Circle).AddTip("You are a cube!\nNobody Knows\nGood!").AddConfirmBtn();
            await WaitBtnClick(point.ConfirmBtn);

            var GO2 = FindGO("UIMain/Button");
            UI.GetFocusPoint(GO2, FocusShape.Rectangle).MakeHole().AddTip("Click Here", RectanglePosition.BottomLeft);
            await WaitBtnClick(GO2.GetComponent<Button>());

            var GO3 = FindGO("Cube2");
            point = UI.GetFocusPoint(GO3, FocusShape.Rectangle).AddTip("This is a cube!").AddConfirmBtn();
            await WaitBtnClick(point.ConfirmBtn);

            OnFinished();
        }
    }
}
