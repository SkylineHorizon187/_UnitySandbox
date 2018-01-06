using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickScript : MonoBehaviour {

	public GameObject wallBrick;
	public float fadeTime = 2f;
	public float moveSpeed;
	public int brickHealth;

	private int currentHealth;
	private bool active = false;
	private bool collidable = false;
	private SpriteRenderer sr;
	private float fadePercent;
	private BoxCollider2D bc;
	private Collider2D col;
	private Vector2 moveDirection;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		sr.color = new Color (1, 1, 1, 0);
		fadePercent = 0;
		moveDirection = new Vector2 (0f, -1f);
		currentHealth = brickHealth;

		StartCoroutine ("FadeIn");
	}
	
	void Update () {
		if (active) {
			if (!collidable) {
				Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
				col = Physics2D.OverlapBox (pos, new Vector2(1.88f, .71f), transform.rotation.eulerAngles.z);
				if (col == null) {
					sr.color = Color.white;
					gameObject.AddComponent<BoxCollider2D> ();
					Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D> ();
					rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
					rb.bodyType = RigidbodyType2D.Kinematic;
					rb.useFullKinematicContacts = true;
					collidable = true;
				}
			}
			transform.Translate (moveDirection * moveSpeed * Time.deltaTime, Space.Self);
			if (transform.position.y < -11f || transform.position.y > 11f ||
				transform.position.x < -18f || transform.position.x > 18f) {
				Destroy (gameObject);
			}
		}
	}

	IEnumerator FadeIn() {
		while (fadePercent < 1) {
			fadePercent += Time.deltaTime / fadeTime;
			sr.color = new Color(1,0,0, fadePercent);
			yield return null;
		}

		active = true;
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (active && collidable) {
			if (other.collider.tag == "Bullet") {
				currentHealth -= other.gameObject.GetComponent<BulletScript> ().damage;
				if (currentHealth <= 0) {
					BrickSpawn.BSpawn.Score(Mathf.RoundToInt (brickHealth * moveSpeed) * 10);
					Destroy (gameObject);
				}
			} else {
				Destroy (GetComponent<Rigidbody2D> ());
				active = false;
				GameObject wb = Instantiate (wallBrick, transform.position, transform.rotation);
				wb.transform.parent = transform.parent;
				wb.GetComponent<BrickToWall> ().fadeTime = 2f;
				Destroy (gameObject, 2f);
			}
		}
	}
}
