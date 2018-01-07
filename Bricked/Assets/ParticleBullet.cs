using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBullet : MonoBehaviour {

    [HideInInspector]
    public float LifeTime;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, LifeTime);
	}
	
    void OnParticleCollision(GameObject other) {
        if (other.tag == "Player") {
            other.SendMessage("TakeDamage", 0.2f, SendMessageOptions.DontRequireReceiver);
        }
    }
}
