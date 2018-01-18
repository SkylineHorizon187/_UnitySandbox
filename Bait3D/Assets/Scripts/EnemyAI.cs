using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public float MoveSpeed;

    private Transform Player;
    private Rigidbody rb;
    private Vector3 MoveDir;
    private Collider thisCollider;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        thisCollider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Player != null && thisCollider.enabled)
        {
            MoveDir = (Player.transform.position - transform.position).normalized;
            MoveDir.y = .1f;
        }
        else
        {
            MoveDir = Vector3.zero;
        }
	}

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + MoveDir * MoveSpeed * Time.fixedDeltaTime);
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Enemy") {
            transform.GetComponent<Rigidbody>().AddExplosionForce(100f, collision.transform.position, 3f);
        }
    }

}
