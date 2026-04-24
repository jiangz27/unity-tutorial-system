using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameTutorialSystem
{
    public class Tutorial_Beginning : Tutorial
    {
        public override async UniTask Play()
        {
            var GO1 = FindGO("Cube1");
            var point = GetFocusPoint(GO1, FocusShape.Rectangle).AddTip("You are a cube!\nNobody Knows\nGood!").AddConfirmBtn();
            await WaitBtnClick(point.ConfirmBtn);

            var GO2 = FindGO("Cube2");
            GetFocusPoint(GO2, FocusShape.Circle);

            var GO3 = FindGO("UIMain/Button");
            GetFocusPoint(GO3, FocusShape.Rectangle).MakeHole().AddTip("Click Here", 0).AddConfirmBtn();

            await WaitGOActive(GO1);
        }
    }
}
