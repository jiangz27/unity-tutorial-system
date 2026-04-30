using System.Collections;
using System.Collections.Generic;
using GameTutorialSystem;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    [SerializeField] UIFocusMask _mask;
    [SerializeField] GameObject _focusPointPrefab;

    List<UIFocusPoint> _points = new List<UIFocusPoint>();

    public UIFocusPoint GetFocusPoint(GameObject go, FocusShape shape)
    {
        var point = _GetFocusPoint();
        point.Init(go, shape);
        _mask.Add(point);
        return point;
    }

    UIFocusPoint _GetFocusPoint()
    {
        if (_points.Count > 0)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                var p = _points[i];
                if (p.gameObject.activeSelf == false)
                {
                    p.Ready();
                    return p;
                }
            }
        }

        var point = Instantiate(_focusPointPrefab, _focusPointPrefab.transform.parent).GetComponent<UIFocusPoint>();
        _points.Add(point);

        point.Ready();
        return point;
    }

    public void HidePoints()
    {
        foreach (var point in _points)
        {
            if (!point.gameObject.activeSelf) continue;
            point.gameObject.SetActive(false);
        }
    }

    public void ShowMask() => _mask.Show();
    public void HideMask() => _mask.Hide();
}
