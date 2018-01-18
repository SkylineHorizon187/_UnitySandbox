using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {
    
    public float fadeTime = 2f;

    private float fadePercent;
    private Material mat;
    private Collider thisCollider;

    // Use this for initialization
    void Start () {
        mat = GetComponent<MeshRenderer>().material;
        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = false;
        StartCoroutine("Fade");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Fade() {
        Color matColor = mat.color;
        while (fadePercent < 1) {
            fadePercent += Time.deltaTime / fadeTime;
            matColor.a = fadePercent;
            mat.color = matColor;
            yield return null;
        }

        thisCollider.enabled = true;
    }

}
