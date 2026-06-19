using UnityEngine;

[CreateAssetMenu(fileName = "DestroyTask", menuName = "Quests/DestroyTask")]
public class DestroyTask : ScriptableObject, ITask
{
    [SerializeField] private string taskName;
    public string targetItemID = "Picture";
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
        TaskManager.OnItemDestroyed += CheckItem;
    }
    private void CheckItem(string destroyedID)
    {
        if (destroyedID == targetItemID)
        {
            TaskManager.OnItemDestroyed -= CheckItem;
            CompleteTask();
        }
    }

    void OnDestroy()
    {
        TaskManager.OnItemDestroyed -= CheckItem;
    }
    public void CompleteTask()
    {
        state = TaskState.Completed;
        GameManager.Instance.EvilGhost.UpdateIndex();
        GameManager.Instance.addBP(badPoints);
        GameManager.Instance.addGP(goodPoints);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
