using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] TMP_Text _countTxt;
    [SerializeField] Button _addBtn;

    public void RefreshCubeNum(int value)
    {
        _countTxt.text = value.ToString();
    }

    public void ShowBtn()
    {
        _addBtn.interactable = true;
    }

    public void AddCubeNum()
    {
        Main.Instance.AddCube();
    }
}
