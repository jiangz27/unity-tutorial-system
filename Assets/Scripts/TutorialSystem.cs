using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTutorialSystem
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField] UITutorial _ui;

        void Start()
        {
            Play(new Tutorial_Beginning());
        }

        public void Play(Tutorial tutorial)
        {
            tutorial.Init(_ui);
            tutorial.Play();
        }
    }
}
