using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerController :  MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private CharacterController cc;
    [SerializeField] float gravity = -10f;
    [SerializeField] float jumpHeight = 2f;

    [SerializeField] float speed;
    Vector2 input = Vector2.zero;
    private Animator animator;

    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnActionMapChanged += Map_OnActionMapChanged;
        inputActions = GameManager.Instance.playerInputActions;
        UpdateActionMap();
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
    }
    void Map_OnActionMapChanged(object sender, System.EventArgs e)
    {
        //reset player velocity to 0 when changing action maps
        input = Vector2.zero;
    }
    void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Player.Move.performed -= OnMove;
            inputActions.Player.Move.canceled -= OnMove;
            inputActions.Player.Jump.performed -= OnJump;

            GameManager.Instance.OnActionMapChanged -= Map_OnActionMapChanged;
        }
    }
    public void UpdateActionMap()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled  -= OnMove;
        
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled  += OnMove;  
        
    }
    void OnMove(InputAction.CallbackContext value)
    {
        input = value.ReadValue<Vector2>().normalized;
    }
    void OnJump(InputAction.CallbackContext value)
    {
        if (cc.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    void FixedUpdate()
    {
        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.fixedDeltaTime;


        Vector3 move = new Vector3(input.x, 0, input.y) * speed * Time.fixedDeltaTime;
        move.y = velocity.y * Time.fixedDeltaTime;
        cc.Move(move);

        bool isWalking = input.x != 0 || input.y != 0;
        animator.SetBool("iswalking", isWalking);
        if (isWalking)
        {
            animator.SetFloat("x", input.x);
            animator.SetFloat("z", input.y);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}