using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }
    public List<ITask> activeTasks = new List<ITask>();

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; 
        }
    }

    public void AddTask(ITask newTask)
    {
        activeTasks.Add(newTask);
        newTask.StartTask();
    }
}