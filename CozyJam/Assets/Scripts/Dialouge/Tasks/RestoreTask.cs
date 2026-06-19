using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Restore Task", menuName = "Quests/Restore Task")]
public class RestoreTask : ScriptableObject, ITask
{
    [SerializeField] private string taskName;
    [SerializeField] private int requiredRenovations;
    [SerializeField] List<string> reqRennovationList; 
    [SerializeField] int goodpoints = 0;
    [SerializeField] int badpoints = 0;
    private int currentRenovations = 0;
    private TaskState state = TaskState.Locked;

    public string TaskName => taskName;
    public TaskState State => state;
    public int goodPoints => goodpoints;
    public int badPoints => badpoints;
    public void StartTask()
    {
        state = TaskState.Active;
        currentRenovations = 0;
        Debug.Log($"New Task Started: {TaskName}");
        SwitchMat.OnMatCleaned += ObjectRenovated;
    }

    public void CompleteTask()
    {
        state = TaskState.Completed;
        Debug.Log($"Task Completed: {TaskName}!");
        SwitchMat.OnMatCleaned -= ObjectRenovated;
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
