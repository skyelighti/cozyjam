using UnityEngine;
using UnityEngine.UI;   

public enum Ability{Rennovate, Move, Restore, None}
public class ButtonController : MonoBehaviour
{
    public Ability ability{get; private set;}
    public static ButtonController Instance;
    [SerializeField] GameObject[] buttons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Start()
    {
        Input.multiTouchEnabled = false;
    }
    public void SwitchAbility(Ability newAbility)
    {
        ability = newAbility;  
    }
    public void SwapRennovate(Button button)
    {
        Debug.Log("Rennovate");
        SwitchAbility(Ability.Rennovate);
        button.Select();
    }
    public void SwapMove(Button button)
    {
        Debug.Log("Move");
        SwitchAbility(Ability.Move);
        button.Select();
    }
    public void SwapRestore(Button button)
    {
        Debug.Log("Restore");
        SwitchAbility(Ability.Restore);
        button.Select();
    }
    public void Toggle()
    {
        foreach (GameObject button in buttons)
        {
            if (button.activeInHierarchy)
            {
                button.SetActive(false);
                SwitchAbility(Ability.None);
            }
            else
                button.SetActive(true);
        }
    }
}
