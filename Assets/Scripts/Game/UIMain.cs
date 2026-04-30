using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] TMP_Text _countTxt;
    [SerializeField] Button _addBtn;

    public void RefreshCubeCount(int value)
    {
        _countTxt.text = "Cube Count: " + value.ToString();
    }

    public void ShowBtn()
    {
        _addBtn.interactable = true;
    }

    public void AddCubeCount()
    {
        Main.Instance.AddCube();
    }
}
