using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour {
    public int Charges;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameController.PlayerLives = false;
        }
        Destroy(other.gameObject);
        Charges--;
        if(Charges < 1)
        {
            Destroy(gameObject);
        }
    }
}
