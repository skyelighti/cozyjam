using UnityEngine;
using UnityEngine.EventSystems;
public class Ghost : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] DialougeInfo[] ghostPhases;
    int indx;
    [SerializeField] GameObject ghost;
    DialougeInfo dialougeInfo;
    Transform newPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialougeInfo = ghostPhases[0];
    }
  
    public void UpdateIndex()
    {
        indx++;
        if(indx < ghostPhases.Length)
        {
            dialougeInfo = ghostPhases[indx];
        }
        //what should happen if a ghost finished it's dialouge?
    }
    //call this when triggering next ghost dialouge

    //call this after dialouge finishes
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.SwapMap(ActionMap.Dialouge);
        ButtonController.Instance.Toggle();
        //sets inactive?
        DialougeManager.Instance.DialougeSetup(dialougeInfo);
        ButtonController.Instance.Toggle();
    }
}
