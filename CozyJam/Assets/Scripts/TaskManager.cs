using UnityEngine;
using System.Collections.Generic;
using System;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }
    public List<ITask> activeTasks = new List<ITask>();
    public static Action<string> OnItemDestroyed;

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