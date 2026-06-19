using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementZone : MonoBehaviour, IDropHandler
{
    public bool canPlace = true;
    public Vector2 itemPlacement;
    public float surfaceY;
    public bool isGhost = false;      
    public bool isGoodGhost = false;

    public void OnDrop(PointerEventData eventData)
    {
        return;
    }
}
