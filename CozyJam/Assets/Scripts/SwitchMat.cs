using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchMat : MonoBehaviour, IPointerDownHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Material[] newmats;
    Material[] ogmats;
    Renderer matRenderer;


    // Update is called once per frame
    void Start()
    {
        matRenderer = GetComponent<Renderer>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ButtonController.Instance.ability == Ability.Restore)
        {
            ogmats = matRenderer.materials;
            for (int i = 0; i < ogmats.Length; i++)
            {
                if (i < newmats.Length && newmats[i] != null)
                {
                    ogmats[i] = newmats[i];
                }
            }

            matRenderer.materials = newmats;
        }
    }
}
