using UnityEngine;

public enum TaskState { Locked, Active, Completed }
public interface ITask 
{
    string TaskName { get; }
    string Description { get; }
    TaskState State { get; }
    int goodPoints { get; }
    int badPoints { get; }
    
    void StartTask();
    void CompleteTask();
}
