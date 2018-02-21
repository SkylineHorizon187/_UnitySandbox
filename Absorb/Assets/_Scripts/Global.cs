using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour {
	
	public static Global instance;
	public GameObject bossDefeatedPrefab;
	public Transform mainCanvas;
	public Text monsterLevelUIText;
	public Button upgradeButton;
	public int monsterLevel = 1;

	void Start() {
		instance = this;
	}

	public void UpdgradeMonsters() {
		upgradeButton.interactable = false;
		upgradeButton.transform.GetChild (0).GetComponent<Text> ().text = "Defeat\r\nBoss!";

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemies.Length; i++) {
			GameObject monster = enemies [i];
			if (monster != PlayerMove.PM.target) {
				monster.transform.localScale = new Vector3 (2, 2, 2);
				monster.tag = "Boss";
				Enemy e = monster.GetComponent<Enemy> ();
				e.myLevel = monsterLevel - 1;
				e.myHealth = 25 * monsterLevel;
				e.currentHealth = e.myHealth;
				e.healthBar.fillAmount = 1;
				e.myAttackSpeed = 4;
				e.myAttackDamage = 2.5f * monsterLevel;
				e.myElement = (Element)Random.Range (0, System.Enum.GetValues (typeof(Element)).Length);
				monster.transform.GetChild (0).GetComponent<Renderer> ().material.color = SpawnParticles.instance.colors [(int)e.myElement];
				enemies [i] = monster;
				break;
			}
		}
	}

	public void BossDefeated() {
		upgradeButton.interactable = true;
		upgradeButton.transform.GetChild (0).GetComponent<Text> ().text = "Upgrade\r\nMonsters";

		monsterLevel++;
		monsterLevelUIText.text = monsterLevel.ToString ();

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemies.Length; i++) {
			GameObject monster = enemies [i];
			if (monster != PlayerMove.PM.target) {
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

		// Show the BossDefeated Text on the Canvas
		Destroy (Instantiate (bossDefeatedPrefab, mainCanvas), 2f);
	}
}
