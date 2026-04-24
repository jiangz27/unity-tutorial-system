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
        protected UITutorial UI { get; private set; }

        public void Init(UITutorial UI)
        {
            this.UI = UI;
            UI.gameObject.SetActive(true);
        }

        public abstract UniTask Play();
        protected virtual void OnStepCompleted()
        {
            UI.HidePoints();
        }

        protected virtual void OnFinished()
        {
            UI.gameObject.SetActive(false);
        }

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
            OnStepCompleted();
        }

        protected async UniTask WaitGOActive(GameObject GO)
        {
            await UniTask.WaitUntil(() => GO.activeSelf);
            OnStepCompleted();
        }

        protected async UniTask WaitCondition(Func<bool> condition)
        {
            await UniTask.WaitUntil(() => condition());
            OnStepCompleted();
        }

        protected async UniTask WaitTask(UniTask task)
        {
            await task;
            OnStepCompleted();
        }

        protected async UniTask<int> WaitTasksAny(List<UniTask> tasks)
        {
            var value = await UniTask.WhenAny(tasks);
            OnStepCompleted();
            return value;
        }

        void Interrupt()
        {

        }
    }
}
