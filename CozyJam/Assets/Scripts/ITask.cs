using UnityEngine;

public enum TaskState { Locked, Active, Completed }
public interface ITask 
{
    string TaskName { get; }
    string Description { get; }
    TaskState State { get; }
    
    void StartTask();
    void CompleteTask();
}
