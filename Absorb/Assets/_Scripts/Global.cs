using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour {

	public static Global instance;
	public Text monsterLevelUIText;
	public int monsterLevel = 1;

	void Start() {
		instance = this;
	}

	public void UpgradeMonster() {
		bool assignBoss = false;

		monsterLevel++;
		monsterLevelUIText.text = monsterLevel.ToString ();

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemies.Length; i++) {
			GameObject monster = enemies [i];
			if (monster != PlayerMove.PM.target) {
				if (!assignBoss) {
					monster.transform.localScale = new Vector3 (2, 2, 2);
					monster.tag = "Boss";
					Enemy e = monster.GetComponent<Enemy> ();
					e.myLevel = monsterLevel-1;
					e.myHealth = 20 * monsterLevel;
					e.currentHealth = e.myHealth;
					e.healthBar.fillAmount = 1;
					e.myAttackSpeed = 5;
					e.myAttackDamage = 2f * monsterLevel;
					e.myElement = (Element)Random.Range (0, System.Enum.GetValues (typeof(Element)).Length);
					monster.transform.GetChild (0).GetComponent<Renderer> ().material.color = SpawnParticles.instance.colors [(int)e.myElement];
					enemies [i] = monster;
					assignBoss = true;
				} else {
					Enemy e = monster.GetComponent<Enemy> ();
					e.myLevel = monsterLevel;
					e.myHealth = 10 * monsterLevel;
					e.currentHealth = e.myHealth;
					e.healthBar.fillAmount = 1;
					e.myAttackSpeed = 5;
					e.myAttackDamage = 1.5f * monsterLevel;
					e.myElement = (Element)Random.Range (0, System.Enum.GetValues (typeof(Element)).Length);
					monster.transform.GetChild (0).GetComponent<Renderer> ().material.color = SpawnParticles.instance.colors [(int)e.myElement];
					enemies [i] = monster;
				}
			}
		}
	}
}
