using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public float MoveSpeed;

    private Transform Player;
    private Rigidbody rb;
    private Vector3 MoveDir;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (Player != null)
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
}
