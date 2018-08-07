using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour {

    public LayerMask UnitLayer;
    [Range(1f, 3f)] public float TowerRange;
    public float fireRate;
    public GameObject bulletPrefab;
    public GameObject firePos;
    public float bulletDamage;
    public float bulletSpeed;

    private GameObject Target;
    private Collider[] UnitColliders;

	void Start () {
        InvokeRepeating("FireOnTarget", .2f, fireRate);	
	}
	
	void FireOnTarget()
    {
        if (Target == null || Vector3.Distance(transform.position, Target.transform.position) > TowerRange)
        {
            UnitColliders = Physics.OverlapSphere(transform.position, TowerRange, UnitLayer);
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
