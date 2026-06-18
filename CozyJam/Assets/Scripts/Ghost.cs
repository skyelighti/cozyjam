using UnityEngine;
using UnityEngine.EventSystems;
public class Ghost : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GhostPhase[] ghostPhases;
    int indx;
    [SerializeField] GameObject ghost;
    DialougeInfo dialougeInfo;
    Transform newPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialougeInfo = ghostPhases[0].dinfo;
        newPos = ghostPhases[0].appearLocation;
        ghost.transform.position = newPos.position;
        ghost.transform.rotation = newPos.rotation;
    }
  
    public void UpdateIndex()
    {
        indx++;
        if(indx >= ghostPhases.Length)
        {
            indx++;
            dialougeInfo = ghostPhases[indx].dinfo;
            newPos = ghostPhases[indx].appearLocation;
            ghost.transform.position = newPos.position;
            ghost.transform.rotation = newPos.rotation;
            ghost.SetActive(true);
        }
        //what should happen if a ghost finished it's dialouge?
    }
    //call this when triggering next ghost dialouge

    void HideGhost()
    {
        ghost.SetActive(false);
    }
    //call this after dialouge finishes
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.SwapMap(ActionMap.Dialouge);
        ButtonController.Instance.Toggle();
        //sets inactive?
        DialougeManager.Instance.DialougeSetup(dialougeInfo);
        HideGhost();
        ButtonController.Instance.Toggle();
    }
}
