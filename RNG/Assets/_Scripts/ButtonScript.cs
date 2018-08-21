using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public LootScript LS;
    public int LootDrops;
    public int DropPercent;
    public bool LootExplosion;
    public float Force;

    private GameObject go;

    public void GetRandomLoot()
    {
        LootItem li;

        for (int i = 0; i < LootDrops; i++)
        {
            li = LS.GetLoot(DropPercent);
            if (li != null)
            {
                go = Instantiate(li.Item, Vector3.zero, Quaternion.identity);
                go.GetComponent<Renderer>().material.color = LS.GetRarity();

                if (LootExplosion)
                {
                    Vector2 inCircle = Random.insideUnitCircle;
                    Vector3 Dir = new Vector3(inCircle.x/4, 1, inCircle.y/4);

                    go.GetComponent<Rigidbody>().AddForce(Dir * Force, ForceMode.Impulse);
                }
            }
        }
    }
}
