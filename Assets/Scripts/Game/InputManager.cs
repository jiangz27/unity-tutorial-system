using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    public GameObject LastClickedObject { get; private set; }
    public Vector3 LastClickWorldPosition { get; private set; }
    public bool IsPointerOverUI { get; private set; }
    private Camera _mainCamera;
    private int _groundMask;
    protected override void Awake()
    {
        _mainCamera = Camera.main;
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        LastClickedObject = null;
        LastClickWorldPosition = Vector3.zero;
        IsPointerOverUI = false;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                IsPointerOverUI = true;
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
            {
                LastClickedObject = hit.collider.gameObject;
                LastClickWorldPosition = hit.point;
            }
        }
    }
}