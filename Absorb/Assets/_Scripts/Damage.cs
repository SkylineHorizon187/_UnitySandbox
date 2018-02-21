using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {
	Basic, Fire, Earth, Water, Wind, Light
}

public class Damage : MonoBehaviour {
	public static Damage instance;
	public float[,] ElementLookup = new float[,] {
                 /*Basic     Fire        Earth       Water       Air         Lightning */
		/*Basic*/{ 1f,       1f,         1f,         1f,         1f,         1f },
		/*Fire*/ { 1f,       1f,         1.35f,      0.50f,      1.65f,      1.2f  },
		/*Earth*/{ 1f,       0.85f,      1f,         1.25f,      0.85f,      1.75f },
		/*Water*/{ 1f,       1.60f,      0.85f,      1f,         1.35f,      0.9f },
		/*Air*/  { 1f,       0.95f,      1.6f,       1.3f,       1f,         0.85f },
		/*Light*/{ 1f,       1.3f,       0.9f,       1.65f,      0.85f,      1f }
	};

	void Start() {
		instance = this;
	}

	public float GetDamageMultiplier(Element AttackType, Element MonsterType) {
		return ElementLookup [(int)AttackType, (int)MonsterType];
	}

}

public class Attack {
	public int attackLevel;
	public Element damageType;
	public float damageAmount;
	public float damageMultiplier;
	public float attackDelay;
	public GameObject attackEffectPrefab;
	public float timer;
}