using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;

namespace GameTutorialSystem
{
    public abstract class Tutorial
    {
        UITutorial _UI;

        public void Init(UITutorial UI)
        {
            _UI = UI;
        }

        public abstract UniTask Play();
        protected virtual void OnFinishWait() { }

        protected GameObject FindGO(string path)
        {
            var GO = GameObject.Find(path);
            if (GO == null)
            {
                Interrupt();
                return null;
            }

            return GO;
        }

        protected T FindComponent<T>(string path) where T : Component
        {
            var GO = FindGO(path);
            if (GO != null) return GO.GetComponent<T>();
            else return null;
        }


        protected async UniTask WaitBtnClick(Button btn)
        {
            await btn.OnClickAsync();
            OnFinishWait();
        }

        protected async UniTask WaitGOActive(GameObject GO)
        {
            await UniTask.WaitUntil(() => GO.activeSelf);
            OnFinishWait();
        }

        protected async UniTask WaitCondition(Func<bool> condition)
        {
            await UniTask.WaitUntil(() => condition());
            OnFinishWait();
        }

        protected async UniTask WaitTask(UniTask task)
        {
            await task;
            OnFinishWait();
        }

        protected async UniTask<int> WaitTasksAny(List<UniTask> tasks)
        {
            var value = await UniTask.WhenAny(tasks);
            OnFinishWait();
            return value;
        }

        void Interrupt()
        {

        }
    }
}
