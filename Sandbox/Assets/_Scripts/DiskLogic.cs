using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskLogic : MonoBehaviour {

    _Tags MyTags;
    Rigidbody2D rb;

    public float Damage;

    // Use this for initialization
    void Awake() {
        MyTags = GetComponent<_Tags>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            if (MyTags.Contains("Team1") && other.gameObject.GetComponent<_Tags>().Contains("Team1")) {
                other.gameObject.SendMessage("GetAmmo");
                Destroy(gameObject);
            }
            else if (MyTags.Contains("Team1") && !other.gameObject.GetComponent<_Tags>().Contains("Team1")) {
                other.gameObject.SendMessage("TakeDamage", Damage);
            }
            else if (MyTags.Contains("Team2") && other.gameObject.GetComponent<_Tags>().Contains("Team2")) {
                other.gameObject.SendMessage("GetAmmo");
                Destroy(gameObject);
            }
            else if (MyTags.Contains("Team2") && !other.gameObject.GetComponent<_Tags>().Contains("Team2")) {
                other.gameObject.SendMessage("TakeDamage", Damage);
            }
            else {
                Damage = Mathf.Round(rb.velocity.magnitude * 10f) / 10f;
            }
        }
    }

    public void SetForce(Vector2 force) {
        rb.AddForce(force, ForceMode2D.Impulse);
        Damage = Mathf.Round(rb.velocity.magnitude * 10f) / 10f;
    }

}
