using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
	public Image healthBar;
	public GameObject deathPrefab;
	public GameObject floatingTextPrefab;
	public Element myElement;
	public int myLevel;
	public float myHealth;
	public float myAttackDamage;
	public float myAttackSpeed;
	public GameObject myAttackEffectPrefab;
	public float currentHealth;

	private bool hasBeenAttacked = false;
	private float timer;
	private ParticleSystem.Burst pb;

	void Start () {
		currentHealth = myHealth;
		healthBar.fillAmount = 1;
		timer = .2f;
		pb = new ParticleSystem.Burst();
	}
	
	void Update () {
		if (hasBeenAttacked) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				timer += myAttackSpeed;
				Destroy(Instantiate (myAttackEffectPrefab, transform.position, Quaternion.identity), 3f);
				PlayerMove.PM.TakeDamage (myAttackDamage);
			}
		}
	}

	public void TakeDamage(float amt, Element typ, bool crit) {
		hasBeenAttacked = true;

		GameObject go = Instantiate (floatingTextPrefab);
		FloatingText ft = go.GetComponent<FloatingText> ();
		Vector3 textPos = transform.position;
		textPos.x += Random.Range (-5f, 5f);

		Color txtColor = SpawnParticles.instance.colors [(int)typ];
		ft.SetFloatingText (amt.ToString (), textPos , txtColor, 30, SpawnParticles.instance.mainCanvas, crit);

		currentHealth -= amt;
		if (currentHealth > 0) {
			healthBar.fillAmount = currentHealth / myHealth;
		} else {
			healthBar.fillAmount = 0;
			GameObject partSys = Instantiate (deathPrefab, transform.position, Quaternion.identity);
			pb.time = 0;
			pb.cycleCount = 1;
			pb.repeatInterval = .010f;
			if (tag == "Boss") {
				pb.count = 8 * myLevel;
			} else {
				pb.count = 5 * myLevel;
			}
			partSys.GetComponent<ParticleSystem> ().emission.SetBurst (0, pb);
			Destroy (partSys, 16f);
			Destroy (gameObject);
		}
	}
}
