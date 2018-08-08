using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour {

    public LayerMask UnitLayer;
    [Range(3f, 6f)] public float TowerRange;
    public float fireRate;
    public GameObject bulletPrefab;
    public GameObject firePos;
    public float bulletDamage;
    public float bulletSpeed;
    public GameObject rangeDisplay;
    public cakeslice.Outline outlineScript;

    private GameObject Target;
    private Collider[] UnitColliders;

    void Start () {
        rangeDisplay.transform.localScale = new Vector3(TowerRange, TowerRange, 1f);
        rangeDisplay.SetActive(false);
        SelectUnit(false);
        InvokeRepeating("FireOnTarget", .2f, fireRate);	
	}

    public void SelectUnit(bool tf)
    {
        outlineScript.enabled = tf;
        rangeDisplay.SetActive(tf);
    }

    void FireOnTarget()
    {
        if (Target == null || Vector3.Distance(transform.position, Target.transform.position) > TowerRange)
        {
            Target = null;
            UnitColliders = Physics.OverlapSphere(transform.position, TowerRange/2, UnitLayer);
            float closeDist = Mathf.Infinity;
            foreach (Collider c in UnitColliders)
            {
                float dist = Vector3.Distance(transform.position, c.gameObject.transform.position);
                if (dist < closeDist)
                {
                    closeDist = dist;
                    Target = c.gameObject;
                }
            }
        }

        if (Target != null)  // fire on target
        {
            GameObject bullet = Instantiate(bulletPrefab, firePos.transform.position, Quaternion.identity);
            bulletScript BS = bullet.GetComponent<bulletScript>();
            BS.target = Target.transform;
            BS.speed = bulletSpeed;
            BS.damage = bulletDamage;
        }
    }
}
