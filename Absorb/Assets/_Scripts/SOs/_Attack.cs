using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack", menuName ="Custom/Attack")]
public class _Attack : ScriptableObject {

    public Element damageType;
    public GameObject attackEffectPrefab;

    public float damageAmount;
    public float damageRange;
    public float attackDelay;
    public float criticalChance;
    public float criticalDamage;

}
