using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickToWall : MonoBehaviour {

	public float fadeTime;

	private SpriteRenderer sr;
	private float fadePercent;

	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		sr.color = new Color (1, 1, 1, 0);
		fadePercent = 0;

		StartCoroutine ("FadeIn");
	}

	IEnumerator FadeIn() {
		while (fadePercent < 1) {
			fadePercent += Time.deltaTime / fadeTime;
			sr.color = new Color(1,1,1, fadePercent);
			yield return null;

			// merge with parent
			GetComponent<BoxCollider2D> ().usedByComposite = true;
		}
	}

}
