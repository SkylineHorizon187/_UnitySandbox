using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    public LootScript LS;
    public GameObject OutputTextGameobject;

    private List<Text> Outs = new List<Text>();
    private Text oneText;

    private void Start()
    {
        for (int i = 0; i < OutputTextGameobject.transform.childCount; i++)
        {
            oneText = OutputTextGameobject.transform.GetChild(i).GetComponent<Text>();
            if (oneText != null)
            {
                Outs.Add(oneText);
            }
        }   
    }

    public void GetRandomLoot()
    {
        LootItem li;

        for (int i = 0; i < Outs.Count; i++)
        {
            li = LS.GetLoot(70);
            if (li == null)
            {
                Outs[i].text = "No Loot";
            }
            else
            {
                Outs[i].text = li.Name;
            }
        }
    }
}
