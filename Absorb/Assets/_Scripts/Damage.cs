using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {
	Basic, Fire, Earth, Water, Wind, Light
}

public class Damage : MonoBehaviour {
	public static Damage instance;
	public float[,] ElementLookup = new float[,] {
		{ 1f, 1f, 1f, 1f, 1f, 1f },
		{ 1f, 1f, .75f, 1.75f, .5f, 1.5f  },
		{ 1f, 1.75f, 1f, .5f,  1.5f, .75f },
		{ 1f, 1.5f, 1.75f, 1f,  .75f, .5f },
		{ 1f, .75f, .5f, 1.5f,  1f, 1.75f },
		{ 1f, .5f, 1.5f, .75f,  1.75f, 1f }
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