using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLoot : MonoBehaviour {

    public float TimeToDie;

	void Update () {
        TimeToDie -= Time.deltaTime;
        if (TimeToDie <= 0)
        {
            Destroy(gameObject);
        }
	}
}
