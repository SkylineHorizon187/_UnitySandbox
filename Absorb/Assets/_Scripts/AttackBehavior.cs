using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior {

    public _Attack data;
    public int attackLevel;
    public float timer;

    public AttackBehavior(_Attack dataPass)
    {
        data = dataPass;
    }

    public bool AttackReady()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += data.attackDelay;
            return true;
        }
        return false;
    }

    public float DamageRoll(out bool crit)
    {
        crit = false;
        float damage = data.damageAmount + Random.Range(0f, data.damageRange);
        damage *= 1 + attackLevel / 10f;
        if (Random.value <= data.criticalChance/100f)
        {
            crit = true;
            damage *= 1 + data.criticalDamage/100f;
        }
        return damage;
    }

}
