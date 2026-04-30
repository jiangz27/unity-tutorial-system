using GameTutorialSystem;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] TutorialSystem _tutorialSystem;
    [SerializeField] UIMain _uiMain;
    public int cubeNum { get; private set; }
    private static Main _instance;
    public static Main Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Main");
                _instance = go.AddComponent<Main>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        _tutorialSystem.Play(new Tutorial_Beginning());
        _uiMain.RefreshCubeNum(0);
    }

    public void AddCube()
    {
        cubeNum += 1;
        _uiMain.RefreshCubeNum(cubeNum);
        if (cubeNum == 3)
        {
            _uiMain.ShowBtn();
            _tutorialSystem.Play(new Tutorial_UI());
        }
    }
}
