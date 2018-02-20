using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float MoveSpeed;

    private Rigidbody rb;
    private Vector3 MoveDir;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	}

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + MoveDir * MoveSpeed * Time.fixedDeltaTime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            GameController.PlayerLives = false;
        }
    }
}
