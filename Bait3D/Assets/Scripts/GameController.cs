﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public int EnemySpawnTime;
    public int TrapSpawnTime;
    public GameObject EnemyPrefab;
    public GameObject TrapPrefab;
    public static bool PlayerLives = true;
	public Text ScoreText;
	public static int score;

    private float EnemyCountDown = 0;
    private float TrapCountDown = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerLives)
        {
            EnemyCountDown += Time.deltaTime;
            if (EnemyCountDown >= EnemySpawnTime)
            {
                EnemyCountDown -= EnemySpawnTime;
                Instantiate(EnemyPrefab, new Vector3(Random.Range(-4f, 4f), .1f, Random.Range(-4f, 4f)), Quaternion.identity);
            }

            TrapCountDown += Time.deltaTime;
            if (TrapCountDown >= TrapSpawnTime)
            {
                TrapCountDown -= TrapSpawnTime;
                Instantiate(TrapPrefab, new Vector3(Random.Range(-4f, 4f), .1f, Random.Range(-4f, 4f)), Quaternion.Euler(90,0,0));
            }

			// Update score text
			ScoreText.text = score.ToString ();
        }
    }
}
