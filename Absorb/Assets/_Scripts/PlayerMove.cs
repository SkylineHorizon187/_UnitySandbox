using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {

	public static PlayerMove PM;
	public GameObject planePrefab;
	public float maxHealth;
	public float healthRegen;
	public Image healthBar;
	public float moveSpeed;
	public GameObject prevPlane;
	public GameObject[] elementAttackPrefabs;
	public GameObject floatingTextPrefab;
	[HideInInspector] public GameObject target;

	public List<Attack> attacks;
	private Rigidbody rb;
	private bool isMoving = true;
	private bool isFighting = false;
	private float currentHealth;

	void Start () {
		PM = this;
		currentHealth = maxHealth;
		rb = GetComponent <Rigidbody> ();
		attacks = new List<Attack> ();
		// Create basic attack
		Attack a = new Attack();
		a.attackLevel = 1;
		a.attackDelay = 2;
		a.damageType = Element.Basic;
		a.damageAmount = .8f;
		a.damageMultiplier = 1.2f;
		attacks.Add (a);
		// Create first monster plane
		Instantiate (planePrefab, prevPlane.transform.position + Vector3.forward * 60f, Quaternion.identity);
	}
	
	void Update () {
		// health regeneration
		if (currentHealth * Time.deltaTime < maxHealth) {
			currentHealth += healthRegen * Time.deltaTime;
			//healthBar.fillAmount = currentHealth / maxHealth;
		}

		if (isMoving) {
			rb.MovePosition (transform.position + transform.forward * moveSpeed * Time.deltaTime);
		}

		if (isFighting) {
			if (target == null) {
				isFighting = false;
				isMoving = true;
				return;
			}

			Enemy e = target.GetComponent<Enemy> ();

			for (int i = 0; i < attacks.Count; i++) {
				attacks [i].timer += Time.deltaTime;
				if (i > 0) {
					ConvertEssence.CE.meters [(int)attacks[i].damageType].attackTimer.fillAmount = attacks [i].timer / attacks [i].attackDelay;
				}
				if (attacks [i].timer >= attacks [i].attackDelay) {
					float multiplier = Damage.instance.GetDamageMultiplier (
						attacks[i].damageType,
						e.myElement
					);
					Destroy (Instantiate (elementAttackPrefabs[(int)attacks[i].damageType], target.transform.position, Quaternion.identity), 3f);
					e.TakeDamage (attacks [i].damageAmount * multiplier, attacks[i].damageType);
					attacks [i].timer -= attacks [i].attackDelay;
					if (i > 0) {
						ConvertEssence.CE.meters [(int)attacks[i].damageType].attackTimer.fillAmount = attacks [i].timer / attacks [i].attackDelay;
					}

					if (target == null) {
						isFighting = false;
						isMoving = true;
						return;
					}
				}
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		isMoving = false;

		if (other.tag == "MP1") {
			Instantiate (planePrefab, other.gameObject.transform.parent.position + Vector3.forward * 60f, Quaternion.identity);
			target = other.gameObject.transform.parent.GetComponent<PlaneSetup> ().monsters [0];
			other.enabled = false;
		} else if (other.tag == "MP2") {
			Destroy (prevPlane);
			prevPlane = other.gameObject.transform.parent.gameObject;
			target = other.gameObject.transform.parent.GetComponent<PlaneSetup> ().monsters [1];
		} else if (other.tag == "MP3") {
			target = other.gameObject.transform.parent.GetComponent<PlaneSetup> ().monsters [2];
		} else if (other.tag == "MP4") {
			target = other.gameObject.transform.parent.GetComponent<PlaneSetup> ().monsters [3];
		}

		isFighting = true;
	}

	public void AddUpgrade(int idx) {
		bool found = false;
		Attack a;

		for (int i = 0; i < attacks.Count; i++) {
			if ((int)attacks [i].damageType == idx) {
				a = attacks [i];
				a.attackLevel += 1;
				a.damageAmount = a.attackLevel * a.damageMultiplier;
				attacks [i] = a;
				if (i == 0) {
					maxHealth += 10;
					healthRegen += .2f;
				}
				found = true;
				break;
			}
		}

		if (! found) {
			a = new Attack();
			a.attackDelay = Random.Range (20f, 80f);
			a.damageType = (Element)idx;
			a.damageAmount = Random.Range (3f, 8f);
			a.damageMultiplier = Random.Range (1.3f, 2.5f);
			attacks.Add (a);
		}
	}

	public void TakeDamage(float amt) {
		currentHealth -= amt;
		healthBar.fillAmount = currentHealth / maxHealth;
		if (currentHealth <= 0) {
			// you are dead...what to do, what to do
		}
		GameObject go = Instantiate (floatingTextPrefab);
		FloatingText ft = go.GetComponent<FloatingText> ();
		Vector3 textPos = transform.position;
		textPos.x += Random.Range (-5f, 5f);
		ft.SetFloatingText (amt.ToString (), textPos , Color.grey, 30, SpawnParticles.instance.mainCanvas);
	}

	public void AddHealth(float amt) {
		if (currentHealth + amt < maxHealth) {
			currentHealth += amt;
			healthBar.fillAmount = currentHealth / maxHealth;
		}
	}
}
