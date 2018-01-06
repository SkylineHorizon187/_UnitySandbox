using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType {Current, Maximum, BulletDamage, Shield}

public class BuffSpawn : MonoBehaviour {
	[System.Serializable]
	public struct Buff {
		public GameObject buffPrefab;
		public BuffType buffType;
		public int buffAmount;
		public float checkSeconds;
		public float spawnChance;
		[HideInInspector] public float lastCheck;
	}
	public Buff[] buffs;
	public float spawnRadius;

	void Start () {
		for (int i = 0; i < buffs.Length; i++) {
			buffs [i].lastCheck = 0;
		}
	}
	
	void Update () {
		for (int i = 0; i < buffs.Length; i++) {
			buffs [i].lastCheck += Time.deltaTime;

			if (buffs [i].lastCheck >= buffs [i].checkSeconds) {
				if (Random.value <= buffs [i].spawnChance) {
					Vector2 loc = Random.insideUnitCircle * spawnRadius;
					GameObject go = Instantiate (buffs[i].buffPrefab, new Vector3(loc.x, loc.y, 1), Quaternion.identity);
					if (buffs [i].buffType == BuffType.Current || buffs [i].buffType == BuffType.Maximum) {
						HealthBuff hb = go.GetComponent<HealthBuff> ();
						hb.buffType = buffs [i].buffType;
						hb.increaseAmount = buffs [i].buffAmount;
					} else if (buffs [i].buffType == BuffType.BulletDamage) {
						BulletBuff bb = go.GetComponent<BulletBuff> ();
						bb.buffType = buffs [i].buffType;
						bb.increaseAmount = buffs [i].buffAmount;
					} else if (buffs [i].buffType == BuffType.Shield) {
						ShieldBuff sb = go.GetComponent<ShieldBuff> ();
						sb.buffType = buffs [i].buffType;
					}
				}
				buffs [i].lastCheck = 0;
			}
		}

	}
}
