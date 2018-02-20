using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct statusEffects {
	public GameObject statusEffect;
	public float statusTime;
	public float timer;
}

public class ClickTest : MonoBehaviour {
	public Sprite portriat;
	public Sprite status;
	public Sprite skill;

	public GameObject myPanel;
	private PlayerUI pui;
	private List<statusEffects> badMedicne;

	void Start() {
		badMedicne = new List<statusEffects> ();

		pui = myPanel.GetComponent<PlayerUI> ();
		pui.PlayerName = "Testing";
		pui.PlayerColor = Color.green; 
		pui.PlayerPortrait = portriat;
		pui.PlayerLevel = 2;
		pui.NextLevelPercent = .5f; 
		pui.HealthPercent = .9f;
		pui.ResourcePercent = .3f;
		statusEffects s = new statusEffects ();
		s.statusEffect = pui.AddStatus (status);
		s.statusTime = 8f;
		s.timer = 0;
		badMedicne.Add (s);
		pui.SetSkill (1, skill, "Q"); 
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
			}
		}

		for (int i = 0; i < badMedicne.Count; i++) {
			statusEffects s = badMedicne [i];
			s.timer += Time.deltaTime;
			s.statusEffect.transform.GetChild (0).GetComponent<Image> ().fillAmount = s.timer / s.statusTime;
			badMedicne [i] = s;

			if (s.timer >= s.statusTime) {
				badMedicne.Remove (s);
				Destroy (s.statusEffect);
				i--;
			}
		}
	}
}
