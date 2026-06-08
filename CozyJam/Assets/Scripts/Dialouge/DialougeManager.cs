using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialougeManager : MonoBehaviour
{
    [SerializeField] GameObject DialougeUI;
    [SerializeField] TMP_Text NameText;
    [SerializeField] Image Icon;
    [SerializeField] Image Background;
    [SerializeField] TMP_Text DialougeText;
    private InputSystem_Actions inputActions;
    private DialougeInfo dinfo;
    bool isTyping;
    string temptxt;

    public static DialougeManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        inputActions = GameManager.Instance.playerInputActions;
        UpdateUIActions();
        DialougeUI.SetActive(false);
    }
    void UpdateUIActions()
    {
        inputActions.Dialouge.Click.performed += OnClick;
        inputActions.Dialouge.ProgressDialouge.performed += OnClick;
        inputActions.Dialouge.Exit.performed += OnDialougeExit;
    }
    void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Dialouge.Click.performed -= OnClick;
            inputActions.Dialouge.ProgressDialouge.performed -= OnClick;
            inputActions.Dialouge.Exit.performed -= OnDialougeExit;
        }
    }
    //called when interact

    public void DialougeSetup(DialougeInfo info)
    {
        GameManager.Instance.Pause();
        dinfo = info;
        DialougeUI.SetActive(true);
        GameManager.Instance.SwapMap(ActionMap.Dialouge);
        dinfo.ResetIndex();
        UpdateDialouge();

    }
    void UpdateDUI()
    {
        if (dinfo.GetIcon() != null)
        {
            Icon.sprite = dinfo.GetIcon();
        }
        if (dinfo.GetName() != null)
        {
            NameText.text = dinfo.GetName();
        }
        if (dinfo.GetBackground() != null)
        {
            Background.enabled = true;
            Background.sprite = dinfo.GetBackground();
        }
        else
        { 
            Background.enabled = false;
        }
    }
    //called on click
    public void UpdateDialouge()
    {
        //updating the dialouge line itself needs to be seperate
        if (!DialougeUI.activeSelf)
        {
            return;
        }
        UpdateDUI();
        temptxt = dinfo.GetDialouge();
        dinfo.IncreaseIndx();
        if (temptxt == null)
        {
            DialougeUI.SetActive(false);
            GameManager.Instance.SwapMap(ActionMap.Player);
            GameManager.Instance.Unpause();
            return;
        }
        else
        {
            StartCoroutine(TypewriterLine(temptxt));
        }
        //DialougeText.text = temptxt;   
    }
    IEnumerator TypewriterLine(string txt)
    {
        isTyping = true;
        for (int i = 0; i < txt.Length; i++)
        {
            DialougeText.text = txt.Substring(0, i + 1);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        isTyping = false;
    }
    void OnClick(InputAction.CallbackContext context)
    {
        if (isTyping)
        {
            StopAllCoroutines();
            DialougeText.text = temptxt;    
            isTyping = false;
        }
        else
        {
            UpdateDialouge();
        }
    }
    void OnDialougeExit(InputAction.CallbackContext context)
    {
        if (isTyping)
        {
            StopAllCoroutines();
            DialougeText.text = temptxt;    
            isTyping = false;
        }
        DialougeUI.SetActive(false);
        GameManager.Instance.SwapMap(ActionMap.Player); 
        GameManager.Instance.Unpause();
    }
}
