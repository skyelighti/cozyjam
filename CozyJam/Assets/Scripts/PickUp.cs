    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.EventSystems;
    using System;

    public class PickUp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //if player presses q, picks up object, if player presses q again, puts the object down.
        //this is the corresponding part on the item, as it should store start position and the radius from the start position it is allowed to move from
        //if it leaves the radius, it should reset onto the original position
        public Vector3 ogPos {get; private set;}
        public float anchorDist;
        public float dropDist;
        public event Action OnForceDrop;
        public bool isHeld;
        float Zpos;
        public bool canMove;
        private Plane dragPlane;
        Rigidbody rb;
        //radius of distance before the object snaps to ogPos
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            ogPos = transform.position;
            rb.isKinematic = true;
            isHeld = false;
            canMove = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(!isHeld && Vector3.Distance(transform.position, ogPos) > anchorDist)
            {
                Drop(true);
                OnForceDrop?.Invoke();
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            
            if (ButtonController.Instance.ability == Ability.Move && canMove)
            {
                Debug.Log("this should begin drag");
                Pickup();

                dragPlane = new Plane(Vector3.up, transform.position);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (ButtonController.Instance.ability == Ability.Move & canMove)
            {
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);

                //math calculations
                if (dragPlane.Raycast(ray, out float enter))
                {
                    transform.position = ray.GetPoint(enter);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (ButtonController.Instance.ability == Ability.Move && canMove)
            {
                Drop();
            }
        }
        public void Pickup()
        {
            isHeld = true;
        }
        public void Drop(bool anchor = false)
        {
            isHeld = false;
            if(!anchor){
                //check if it is in the acceptable dropping area? otherwise or if its an anchor
                Collider[] colliders = Physics.OverlapSphere(transform.position, dropDist);
                List<Collider> areas = new List<Collider>();
                float smallest_dist = float.MaxValue;
                foreach (Collider collider in colliders)
                {
                    PlacementZone p = collider.gameObject.GetComponent<PlacementZone>();
                    if(p != null && p.canPlace){
                        Vector3 direction = (p.transform.position - transform.position).normalized;
                        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, dropDist);

                        foreach (RaycastHit hit in hits)
                        {
                            if (hit.collider.gameObject == p.gameObject)
                            {
                                float dist = Vector3.Distance(p.transform.position, transform.position);
                                if (dist < smallest_dist)
                                {
                                    smallest_dist = dist;
                                    areas.Insert(0, hit.collider);
                                }
                                else
                                {
                                    areas.Add(hit.collider);
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
                if(areas.Count> 0)
                {
                    PlacementZone zone = areas[0].GetComponent<PlacementZone>();
                    
                    Vector3 localOffset = new Vector3(zone.itemPlacement.x, 0, zone.itemPlacement.y);
                    Vector3 worldSnappingPoint = zone.transform.TransformPoint(localOffset);

                    float newX = worldSnappingPoint.x;
                    float newZ = worldSnappingPoint.z;

                    Collider myCollider = GetComponent<Collider>();
                    float pivotOffset = 0f;
                    if (myCollider != null)
                    {
                        pivotOffset = transform.position.y - myCollider.bounds.min.y;
                    }
                    transform.position = new Vector3(newX, zone.surfaceY + pivotOffset, newZ);
                    ogPos = transform.position;
                    return;
                }
            }
            transform.position = ogPos;   
            //dropped at an invalid position?

        }
        void DisableMovement()
        {
            canMove = false;
        }
    }
