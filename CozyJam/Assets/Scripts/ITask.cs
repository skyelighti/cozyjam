using UnityEngine;
using System;

public enum TaskState { Locked, Active, Completed }
public interface ITask 
{
    string TaskName { get; }
    TaskState State { get; }
    int goodPoints { get; }
    int badPoints { get; }
    
    
    void StartTask();
    void CompleteTask();
}
