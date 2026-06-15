using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HoldUIManager : MonoBehaviour
{
    public static HoldUIManager Instance {get; private set;}

    [SerializeField] private Slider progressSlider;
    [SerializeField] Vector2 sliderOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        progressSlider.gameObject.SetActive(false);
    }
    public void StartHold()
    {
        progressSlider.value = 0f;
        progressSlider.gameObject.SetActive(true);
        
        progressSlider.gameObject.transform.position = Mouse.current.position.ReadValue() + sliderOffset; 
    }

    public void UpdateHold(float progressPercentage)
    {
        progressSlider.value = progressPercentage;
        progressSlider.gameObject.transform.position = Mouse.current.position.ReadValue() + sliderOffset; 
    }
    public void StopHold()
    {
        progressSlider.gameObject.SetActive(false);
    }
}
