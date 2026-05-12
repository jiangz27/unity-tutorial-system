using GameTutorialSystem;
using UnityEngine;

public class Main : Singleton<Main>
{
    [SerializeField] TutorialSystem _tutorialSystem;
    [SerializeField] UIMain _uiMain;
    public int cubeNum { get; private set; }

    protected override void Awake()
    {
        _tutorialSystem.Play(new Tutorial_Beginning());
        _uiMain.RefreshCubeCount(0);
    }

    public void AddCube()
    {
        cubeNum += 1;
        _uiMain.RefreshCubeCount(cubeNum);
        if (cubeNum == 3)
        {
            _uiMain.ShowBtn();
            _tutorialSystem.Play(new Tutorial_UI());
        }
    }
}
