using UnityEngine;

public class SwitchMat : Interactable
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
    public override void Interact()
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
