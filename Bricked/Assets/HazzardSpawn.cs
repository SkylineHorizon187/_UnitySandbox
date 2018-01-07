using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazzardSpawn : MonoBehaviour {

    public GameObject PS_WarningPrefab;
    public GameObject PS_BulletsPrefab;
    public GameObject ThePlayer;

    private Vector3 PlayerPosition;
    private float HazzardDuration = 5f;

    // Use this for initialization
    void Start() {
        GetPlayerPosition();
        InvokeRepeating("SpawnHazzardWarning", Random.Range(5f, 6f), Random.Range(10f, 20f));
    }
    
    void GetPlayerPosition() {
        if (ThePlayer != null) {
            PlayerPosition = ThePlayer.transform.position;
            Invoke("GetPlayerPosition", Random.Range(3f, 12f));
        }
    }

    void SpawnHazzardWarning() {
        StartCoroutine("TheBulletHazzard", PlayerPosition);
    }

    IEnumerator TheBulletHazzard(Vector3 targetPosition) {
        Destroy(Instantiate(PS_WarningPrefab, PlayerPosition, Quaternion.identity), 3f);
        yield return new WaitForSeconds(2.2f);

        GameObject PS_Bullet = Instantiate(PS_BulletsPrefab, targetPosition, Quaternion.identity);
        PS_Bullet.GetComponent<ParticleBullet>().LifeTime = HazzardDuration;
        HazzardDuration += 0.5f;
        if (HazzardDuration > 30f) {
            HazzardDuration = 30f;
        }
    }
    
}
