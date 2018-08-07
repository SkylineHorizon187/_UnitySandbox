using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

    public Transform target;
    public float speed;
    public float damage;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
        if (target != null)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        other.gameObject.transform.parent.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }
}
