using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Rennovate Task", menuName = "Quests/Rennovate Task")]

public class RennovateTask : ScriptableObject, ITask
{
    [SerializeField] private string taskName;
    [SerializeField] private string description;
    [SerializeField] private int requiredRenovations;
    [SerializeField] List<string> reqRennovationList; 
    private int currentRenovations = 0;
    private TaskState state = TaskState.Locked;

    public string TaskName => taskName;
    public string Description => description;
    public TaskState State => state;
    public void StartTask()
    {
        state = TaskState.Active;
        currentRenovations = 0;
        Debug.Log($"New Task Started: {TaskName}");
        Rennovate.OnRennovate += ObjectRenovated;
    }

    public void CompleteTask()
    {
        state = TaskState.Completed;
        Debug.Log($"Task Completed: {TaskName}!");
        Rennovate.OnRennovate -= ObjectRenovated;
        GameManager.Instance.GoodGhost.UpdateIndex();
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
