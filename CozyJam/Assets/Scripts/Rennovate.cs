using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Rennovate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float timer;
    float time;
    bool isRennovating = false;
    [SerializeField] GameObject oldObj;
    [SerializeField] GameObject newObj;
    private bool isAlreadyRenovated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isRennovating = false;
        oldObj.SetActive(true);
        newObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isRennovating && time < timer)
        {
            time += Time.deltaTime;
            HoldUIManager.Instance.UpdateHold(time/(float)timer);
        }
        if(time >= timer)
        {
            isRennovating = false;
            RennovateObj();
            time = 0;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(ButtonController.Instance.ability == Ability.Rennovate)
        {
            Debug.Log("rennovate started");
            isRennovating = true;   
            HoldUIManager.Instance.StartHold();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isRennovating = false;
        time = 0;
        HoldUIManager.Instance.StopHold();
    }
    void RennovateObj()
    {
        isAlreadyRenovated = !isAlreadyRenovated;
        isRennovating = false;
        if (isAlreadyRenovated)
        {
            oldObj.SetActive(false);
            newObj.SetActive(true);
        }
        else
        {
            oldObj.SetActive(true);
            newObj.SetActive(false);
        }
    }
}
