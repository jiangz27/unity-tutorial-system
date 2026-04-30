using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("InputManager");
                _instance = go.AddComponent<InputManager>();
            }
            return _instance;
        }
    }


    public GameObject LastClickedObject { get; private set; }
    public Vector3 LastClickWorldPosition { get; private set; }
    public bool IsPointerOverUI { get; private set; }
    private Camera _mainCamera;
    private int _groundMask;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

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

            PerformRaycast();
        }
    }


    private void PerformRaycast()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
        {
            LastClickedObject = hit.collider.gameObject;
            LastClickWorldPosition = hit.point;
            Debug.Log($"点击了物体: {LastClickedObject.name}, 世界坐标: {LastClickWorldPosition}");
        }
    }
}