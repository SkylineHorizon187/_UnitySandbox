using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour {

    public Image progressBar;
    public GameObject GameControllerGO;
    public GameObject EnemyPrefab;
    [HideInInspector] public static int NameSeed;

    private AudioSource AS;
    private Vector3 spawnPos;
    private float songTime;

	void Start () {
        AS = GetComponent<AudioSource>();
        NameSeed = AS.clip.name.GetHashCode();
        //AS.Play();
        songTime = AS.clip.length;
	}
	
	void Update () {        
		if (progressBar != null)
        {
            progressBar.fillAmount = AS.time / songTime;
        }
	}

    public void SpawnEnemyUnit(float[] unitData)
    {
        if (spawnPos == Vector3.zero)
        {
            spawnPos = GameControllerGO.GetComponent<GameControl>().SpawnPosition();
        }
        GameObject Enemy = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);
        EnemyAI AI = Enemy.GetComponent<EnemyAI>();
        AI.UnitHealth = unitData[1];
        AI.UnitSpeed = unitData[2] / 20;
        AI.UnitTurnRate = unitData[3];
        AI.UnitResistance = unitData[4] / 25;
    }

    public void StartStopMusic()
    {
        if (AS.isPlaying)
        {
            AS.Stop();
        } else
        {
            AS.Play();
        }
    }
}
