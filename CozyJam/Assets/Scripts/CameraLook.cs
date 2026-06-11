using UnityEngine;
using UnityEngine.InputSystem;
public class CameraLook : MonoBehaviour
{
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float clampAngle = 80f;

    private float rotX = 0f;
    private Vector2 lookInput;
    private InputSystem_Actions inputActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = GameManager.Instance.playerInputActions;
        inputActions.Player.Look.performed += OnLook;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnDestroy()
    {
        inputActions.Player.Look.performed -= OnLook;
    }
    // Update is called once per frame
    void Update()
    {
        rotX -= lookInput.y * sensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        transform.parent.Rotate(Vector3.up * lookInput.x * sensitivity * Time.deltaTime);
    }
    void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

}
