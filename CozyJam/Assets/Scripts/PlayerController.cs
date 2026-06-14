using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerController :  MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InputSystem_Actions inputActions;

    [Header("Ability Settings")]
    [SerializeField] float interactRad = 3f;
    [SerializeField] float pickupRad = 3f;
    private PickUp currItem = null;
    public Transform itemholder {get; private set;}
    bool canPickup;
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (ButtonController.Instance.ability == Ability.Restore)
        {
                SwitchMat smat = eventData.pointerCurrentRaycast.gameObject.GetComponent<SwitchMat>();
                if(smat!= null)
                {
                    smat.Interact();
                }   
        }

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        PickUp currdrag = eventData.pointerPress.GetComponent<PickUp>();
        if(ButtonController.Instance.ability == Ability.Move && currdrag != null)
        {
            currdrag.Pickup();
        }
        return;
    }
    public void OnDrag(PointerEventData eventData)
    {
        PickUp currdrag = eventData.pointerPress.GetComponent<PickUp>();
        if(ButtonController.Instance.ability == Ability.Move && currdrag != null)
        {
            currdrag.transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
        }
        return;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
    void FixedUpdate()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
    void OnPickup()
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
                pickupitems[0].gameObject.GetComponent<PickUp>().Pickup();

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
    void OnInteract()
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