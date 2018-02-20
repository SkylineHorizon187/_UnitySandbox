using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Meters {
	public float curProgress;
	public float nextLvlAmt;
	public Image progBar;
	public Image convertImg;
	public Image attackTimer;
	public Text levelText;
}

public class ConvertEssence : MonoBehaviour {

	public static ConvertEssence CE;
	[SerializeField] public Meters[] meters;

	private float conversionRate = .3f;

	void Start() {
		CE = this;
	}

	public void ConvertClick(Image img) {
		Color curColor = img.color;
		int idx = ColorToIdx (curColor);

		if (idx != -1) {
			if (idx < SpawnParticles.instance.colors.Length - 1) {
				img.color = SpawnParticles.instance.colors [idx + 1];
			} else {
				img.color = SpawnParticles.instance.colors [0];
			}
		}
	}

	public int ColorToIdx(Color c) {
		for (int i = 0; i < SpawnParticles.instance.colors.Length; i++) {
			if (SpawnParticles.instance.colors [i] == c) {
				return i;
			}
		}
		return -1;
	}

	public void AddColor(Color c) {
		int idx = ColorToIdx (c);
		int cvrtIdx;
		float convert;

		if (idx != -1) {
			// Get converted color index
			if (idx == 0) {
				cvrtIdx = 0;
			} else {
				cvrtIdx = ColorToIdx (meters [idx].convertImg.color);
			}

			if (cvrtIdx != -1) {
				if (idx == cvrtIdx) {
					convert = 1;
					if (idx == 0) {
						convert = .3f;
					}
				} else {
					convert = conversionRate;
				}
				int colorLevel = int.Parse (meters [cvrtIdx].levelText.text);
				PlayerMove.PM.AddHealth (3/(colorLevel+.01f));
				meters [cvrtIdx].curProgress += 1 * convert;
				if (meters [cvrtIdx].curProgress >= meters [cvrtIdx].nextLvlAmt) {
					meters [cvrtIdx].levelText.text = (colorLevel + 1).ToString ();
					meters [cvrtIdx].curProgress -= meters [cvrtIdx].nextLvlAmt;
					meters [cvrtIdx].nextLvlAmt *= 1.75f;
					// add/upgrade attack in player's attack list
					PlayerMove.PM.AddUpgrade(cvrtIdx);
				}
				meters [cvrtIdx].progBar.fillAmount = meters [cvrtIdx].curProgress / meters [cvrtIdx].nextLvlAmt;
			}
		}
	}

}
