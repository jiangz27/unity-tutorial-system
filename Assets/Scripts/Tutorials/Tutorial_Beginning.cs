using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameTutorialSystem
{
    public class Tutorial_Beginning : Tutorial
    {
        public override async UniTask Play()
        {
            var point1 = FindGO("GOs/Point1");
            UI.GetFocusPoint(point1, FocusShape.Circle).MakeHole().AddTip("Click Here");
            await WaitCondition(() => InputManager.Instance.LastClickedObject != null);

            UI.HideMask();
            var player = FindGO("GOs/Player").GetComponent<Player>();
            await WaitCondition(() => player.Velocity == Vector3.zero);

            UI.ShowMask();
            var cube1 = FindGO("GOs/Cube1");
            var focusPoint = UI.GetFocusPoint(cube1, FocusShape.Rectangle).MakeHole().AddTip("Close it");
            await WaitCondition(() => InputManager.Instance.LastClickedObject != null);

            UI.HideMask();
            await WaitCondition(() => cube1 == null);

            UI.ShowMask();
            var cube2 = FindGO("Cube2");
            var cube3 = FindGO("Cube3");
            UI.GetFocusPoint(cube2, FocusShape.Rectangle);
            focusPoint = UI.GetFocusPoint(cube3, FocusShape.Rectangle).AddTip("Get all cubes").AddConfirmBtn();
            await WaitBtnClick(focusPoint.ConfirmBtn);

            OnFinished();
        }
    }
}
