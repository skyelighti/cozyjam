using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerController :  MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private CharacterController cc;
    [Header("Movement Settings")]
    [SerializeField] float gravity = -10f;
    [SerializeField] float jumpHeight = 2f;

    [SerializeField] float speed;

    [Header("Ability Settings")]
    [SerializeField] float interactRad = 3f;
    [SerializeField] float pickupRad = 3f;
    private PickUp currItem = null;
    public Transform itemholder {get; private set;}
    bool canPickup;
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
            inputActions.Player.Interact.performed -= OnInteract;
            inputActions.Player.Pickup.performed -= OnPickup;

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

        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Pickup.performed += OnPickup;
        
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
    void OnPickup(InputAction.CallbackContext value)
    {
        if(canPickup)
        {
            Debug.Log("Pickup");
            Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRad);
            List<Collider> pickupitems = new List<Collider>();
            float smallest_dist = float.MaxValue;
            foreach (Collider collider in colliders)
            {
                //send a raycast in the direction of the interactable, if the first result returned is the interactable, then interact with it
                PickUp pickup = collider.GetComponent<PickUp>();
                if (pickup != null)
                {
                    Vector3 direction = (pickup.transform.position - transform.position).normalized;
                    RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, pickupRad);

                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.gameObject == pickup.gameObject)
                        {
                            float dist = Vector3.Distance(pickup.transform.position, transform.position);
                            if (dist < smallest_dist)
                            {
                                smallest_dist = dist;
                                pickupitems.Insert(0, hit.collider);
                            }
                            else
                            {
                                pickupitems.Add(hit.collider);
                            }
                            break;
                        }
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                        {
                            break;
                        }
                    }

                }

            }
            if (pickupitems.Count > 0)
            {
                pickupitems[0].gameObject.GetComponent<PickUp>().Pickup(itemholder);

                canPickup = false;
                currItem = pickupitems[0].gameObject.GetComponent<PickUp>();
                currItem.OnForceDrop += HandleForceDrop;
            }
            return;
        }
        else
        {
            currItem.Drop();
            canPickup = true;
            currItem.OnForceDrop -= HandleForceDrop;
        }
    }
    
    void HandleForceDrop()
    {
        currItem.OnForceDrop -= HandleForceDrop;
        canPickup = true;
        currItem = null;
    }
    void OnInteract(InputAction.CallbackContext value)
    {
        Debug.Log("Interacted");
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRad);
        List<Collider> interactables = new List<Collider>();
        float smallest_dist = float.MaxValue;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                //send a raycast in the direction of the interactable, if the first result returned is the interactable, then interact with it
                Interactable interactable = collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    Vector3 direction = (interactable.transform.position - transform.position).normalized;
                    RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, 3f);

                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.gameObject == interactable.gameObject)
                        {
                            float dist = Vector3.Distance(interactable.transform.position, transform.position);
                            if (dist < smallest_dist)
                            {
                                smallest_dist = dist;
                                interactables.Insert(0, hit.collider);
                            }
                            else
                            {
                                interactables.Add(hit.collider);
                            }
                            break;
                        }
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                        {
                            break;
                        }
                    }

                }

            }

        }
        if (interactables.Count > 0)
        {
            interactables[0].gameObject.GetComponent<Interactable>().Interact();
        }
        return;
    }
}