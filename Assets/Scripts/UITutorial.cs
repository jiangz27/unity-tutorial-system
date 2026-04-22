using System.Collections;
using System.Collections.Generic;
using GameTutorialSystem;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    [SerializeField] UIFocusMask _mask;
    [SerializeField] GameObject _focusPointPrefab;

    List<UIFocusPoint> _pointCache = new List<UIFocusPoint>();

    public UIFocusPoint GetFocusPoint(GameObject go, FocusShape shape)
    {
        var point = _GetFocusPoint();
        point.Init(go, shape);
        _mask.Add(point);
        return point;
    }

    UIFocusPoint _GetFocusPoint()
    {
        if (_pointCache.Count > 0)
        {
            for (int i = 0; i < _pointCache.Count; i++)
            {
                var p = _pointCache[i];
                if (p.gameObject.activeSelf == false)
                {
                    p.Ready();
                    return p;
                }
            }
        }

        var point = Instantiate(_focusPointPrefab, _focusPointPrefab.transform.parent).GetComponent<UIFocusPoint>();
        _pointCache.Add(point);

        point.Ready();
        return point;
    }

}
