using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSetup : MonoBehaviour {
	public GameObject monsterPrefab;
	public GameObject[] monsters;

	private Transform[] movePoints;

	void Start () {
		movePoints = new Transform[transform.childCount];
		monsters = new GameObject[movePoints.Length - 1];

		for (int i = 0; i < transform.childCount; i++) {
			movePoints [i] = transform.GetChild (i);
			if (i > 0) {
				GameObject monster = Instantiate (monsterPrefab, movePoints[i].position, Quaternion.identity);
				Enemy e = monster.GetComponent<Enemy> ();
				e.myLevel = Global.instance.monsterLevel;
				e.myHealth = 20f + 10f * Global.instance.monsterLevel;
				e.healthBar.fillAmount = 1;
				e.myAttackSpeed = 5;
				e.myAttackDamage = 1.5f * Global.instance.monsterLevel;
				e.myElement = (Element)Random.Range (0, System.Enum.GetValues (typeof(Element)).Length);
				monster.transform.GetChild (0).GetComponent<Renderer> ().material.color = SpawnParticles.instance.colors [(int)e.myElement];
				monsters [i-1] = monster;
			}
		}
	}

}
