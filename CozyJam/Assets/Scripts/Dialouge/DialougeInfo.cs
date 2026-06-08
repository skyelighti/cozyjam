using UnityEngine;

[CreateAssetMenu(fileName = "DialougeInfo", menuName = "Scriptable Objects/DialougeInfo")]
public class DialougeInfo : ScriptableObject
{
    [SerializeField] DialougeLine[] dialouge;
    public bool advancesProgress;
    int indx = 0;
    public string ID;
    //this is for keeping stuff synced
    public string GetDialouge()
    {
        string dia = indx < dialouge.Length ? dialouge[indx].dialouge : null;
        return dia;
    }
    public string GetName()
    {
        string name = indx < dialouge.Length ? dialouge[indx].Name : null;
        return name;
    }
    public Sprite GetIcon()
    {
        Sprite icon = indx < dialouge.Length ? dialouge[indx].sprite : null;
        return icon;
    }
    public Sprite GetBackground()
    {
        Sprite bg = indx < dialouge.Length ? dialouge[indx].Bg : null;
        return bg;
    }
    public void IncreaseIndx()
    {
        indx++;
    }
    public void ResetIndex()
    {
        indx = 0;
    }
}
