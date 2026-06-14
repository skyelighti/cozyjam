using UnityEngine;
using System.Collections.Generic;
using System;

public class PickUp : MonoBehaviour
{
    //if player presses q, picks up object, if player presses q again, puts the object down.
    //this is the corresponding part on the item, as it should store start position and the radius from the start position it is allowed to move from
    //if it leaves the radius, it should reset onto the original position
    public Vector3 ogPos {get; private set;}
    public float anchorDist;
    public float dropDist;
    public event Action OnForceDrop;
    public bool isHeld;
    Rigidbody rb;
    //radius of distance before the object snaps to ogPos
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ogPos = transform.position;
        rb.isKinematic = true;
        isHeld = false;
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
    public void Pickup(Transform t)
    {
        transform.SetParent(t);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        isHeld = true;
    }
    public void Drop(bool anchor = false)
    {
        transform.SetParent(null);
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
                float newX = areas[0].GetComponent<PlacementZone>().itemPlacement.x;
                float newZ = areas[0].GetComponent<PlacementZone>().itemPlacement.y;

                Collider myCollider = GetComponent<Collider>();
                float newY = 0f;
                if (myCollider != null)
                {
                    newY = myCollider.bounds.extents.y;
                }

                transform.position = new Vector3(newX, areas[0].GetComponent<PlacementZone>().surfaceY + newY, newZ);
                ogPos = transform.position;
                return;
            }
        }
        transform.position = ogPos;   
        //dropped at an invalid position?

    }
}
