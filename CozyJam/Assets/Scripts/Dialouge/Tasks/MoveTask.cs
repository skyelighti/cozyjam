using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MoveTask", menuName = "Quests/MoveTask")]
public class MoveTask : ScriptableObject, ITask
{
    [SerializeField] private string taskName;
    [SerializeField] private string description;
    [SerializeField] private int requiredRenovations;
    [SerializeField] List<string> reqRennovationList; 
    [SerializeField] int goodpoints = 0;
    [SerializeField] int badpoints = 0;
    private int currentRenovations = 0;
    private TaskState state = TaskState.Locked;

    public string TaskName => taskName;
    public string Description => description;
    public TaskState State => state;
    public int goodPoints => goodpoints;
    public int badPoints => badpoints;
    public void StartTask()
    {
        state = TaskState.Active;
        currentRenovations = 0;
        Debug.Log($"New Task Started: {TaskName}");
        PickUp.OnMoved += ObjectRenovated;
    }

    public void CompleteTask()
    {
        state = TaskState.Completed;
        Debug.Log($"Task Completed: {TaskName}!");
        PickUp.OnMoved -= ObjectRenovated;
        GameManager.Instance.GoodGhost.UpdateIndex();
        GameManager.Instance.addBP(badPoints);
        GameManager.Instance.addGP(goodPoints);
    }

    public void ObjectRenovated(string ID)
    {
        if (state != TaskState.Active) return;
        if(reqRennovationList.Count == 0 || reqRennovationList.Contains(ID))
            currentRenovations++;
        Debug.Log($"Progress: {currentRenovations} / {requiredRenovations}");

        if (currentRenovations >= requiredRenovations)
        {
            CompleteTask();
        }
    }
}
