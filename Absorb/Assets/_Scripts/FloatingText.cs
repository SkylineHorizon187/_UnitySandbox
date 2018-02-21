using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {
	public float lifeTime;
	public float fadeTime;
	public float floatSpeed;

	private bool isSet = false;
	private RectTransform rt;

	public void SetFloatingText(string text, Vector3 position, Color color, int size, Transform canvas, bool crit) {
		transform.SetParent (canvas);
		transform.position = Camera.main.WorldToScreenPoint (position);

		TextMeshProUGUI txtcomp = GetComponent<TextMeshProUGUI> ();

        if (crit)
        {
            size += 10;
            text += " !!";
			txtcomp.outlineWidth = .3f;
        }

        txtcomp.SetText (text);
		txtcomp.faceColor = color;
		txtcomp.fontSize = size;
        

		isSet = true;
	}

	void Start() {
		rt = GetComponent<RectTransform>();
	}

	void Update() {
		if (isSet) {
			lifeTime -= Time.deltaTime;
			if (lifeTime <= 0) {
				Destroy (gameObject);
			}

			if (lifeTime <= fadeTime) {
				TextMeshProUGUI txtcomp = GetComponent<TextMeshProUGUI> ();
				Color c = txtcomp.faceColor;
				c.a = lifeTime / fadeTime;
				txtcomp.faceColor = c;
			}

			rt.anchoredPosition = new Vector2 (rt.anchoredPosition.x, rt.anchoredPosition.y + floatSpeed * Time.deltaTime);
		}
	}

}