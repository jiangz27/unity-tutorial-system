using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed;
    bool _hasTargetPoint;
    Vector3 _targetPoint;
    public Vector3 Velocity { get; private set; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Velocity = Vector3.zero;

        var targetObject = InputManager.Instance.LastClickedObject;
        if (targetObject != null)
        {
            var point = InputManager.Instance.LastClickWorldPosition;
            if ((this.transform.position - point).sqrMagnitude > 0.1f)
            {
                _hasTargetPoint = true;
                _targetPoint = point;
            }
        }

        if (_hasTargetPoint)
        {
            if ((this.transform.position - _targetPoint).sqrMagnitude < 0.1f)
            {
                _hasTargetPoint = false;
            }
            else
            {
                var dir = (_targetPoint - this.transform.position).normalized;
                Velocity = dir * _speed * Time.deltaTime;
                this.transform.Translate(dir * _speed * Time.deltaTime, Space.World);
            }
        }
    }
}
