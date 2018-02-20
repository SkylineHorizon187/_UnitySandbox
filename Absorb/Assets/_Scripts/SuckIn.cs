using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckIn : MonoBehaviour {
	private Transform player;
	private ParticleSystem ps;
	private float timer;
	private float force;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		ps = GetComponent<ParticleSystem> ();
		timer = 0;
		force = .001f;
	}
	
	void Update () {
		timer += Time.deltaTime;
		if (timer > 2) {
			force += Time.deltaTime;
		}

		ParticleSystem.Particle[] p = new ParticleSystem.Particle[ps.particleCount+1];

		int l = ps.GetParticles(p);
		int i = 0;
		while (i < l) {
			if (p [i].startColor == Color.white) {
				p [i].startColor = SpawnParticles.instance.colors [Random.Range (0, SpawnParticles.instance.colors.Length)];
			}

			Vector3 dir = player.position - p [i].position;
			if (dir.magnitude > 1.5f) {
				Vector3 adjust = dir.normalized * ((p [i].startLifetime / p [i].remainingLifetime) * (force / p[i].remainingLifetime));
				p [i].velocity += adjust;
				p [i].velocity -= (p [i].velocity / 5) * Time.deltaTime;
			} else {
				p [i].remainingLifetime = 0;
				ConvertEssence.CE.AddColor (p[i].GetCurrentColor (ps));
			}
			i++;
		}

		ps.SetParticles(p, l);    
	}
}
