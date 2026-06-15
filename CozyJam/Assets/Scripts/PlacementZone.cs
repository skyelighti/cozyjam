using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementZone : MonoBehaviour, IDropHandler
{
    public bool canPlace = true;
    public Vector2 itemPlacement;
    public float surfaceY;

    public void OnDrop(PointerEventData eventData)
    {
        return;
    }
}
