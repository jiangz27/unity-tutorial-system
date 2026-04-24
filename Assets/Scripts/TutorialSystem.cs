using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTutorialSystem
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField] GameObject _uiPrefab;

        void Start()
        {
            Play(new Tutorial_Beginning());
        }

        public void Play(Tutorial tutorial)
        {
            var GO = Instantiate(_uiPrefab);
            var canvas = GO.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 1;

            var ui = GO.GetComponent<UITutorial>();

            tutorial.Init(ui);
            tutorial.Play();
        }
    }
}
