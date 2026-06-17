using UnityEngine;

[CreateAssetMenu(fileName = "Restore Task", menuName = "Quests/Restore Task")]
public class RestoreTask : ScriptableObject, ITask
{
    [SerializeField] private string taskName;
    [SerializeField] private string description;
    [SerializeField] private int requiredRenovations;
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
    }

    public void CompleteTask()
    {
        state = TaskState.Completed;
        Debug.Log($"Task Completed: {TaskName}!");
        GameManager.Instance.GoodGhost.UpdateIndex();
    }

    public void ObjectRenovated()
    {
        if (state != TaskState.Active) return;

        currentRenovations++;
        Debug.Log($"Progress: {currentRenovations} / {requiredRenovations}");

        if (currentRenovations >= requiredRenovations)
        {
            CompleteTask();
        }
    }
}
