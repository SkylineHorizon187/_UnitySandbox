using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour {
	// Internal references
	public TextMeshProUGUI playerNameRef;
	public Image playerColorRef;
	public Image playerPortraitRef;
	public TextMeshProUGUI playerLevelRef;
	public Image playerXPRef;
	public Image healtBarRef;
	public Image resourceBarRef;
	public GameObject statusEffects;
	public GameObject statusPrefab;
	public GameObject skillPanel;

	public string PlayerName {
		get { return playerNameRef.text; }
		set { playerNameRef.text = value; }
	}

	public Color PlayerColor {
		get { return playerColorRef.color; }
		set { playerColorRef.color = value; }
	}

	public Sprite PlayerPortrait {
		get { return playerPortraitRef.GetComponent<Image> ().sprite; }
		set { playerPortraitRef.GetComponent<Image> ().sprite = value; }
	}

	public int PlayerLevel {
		get { return int.Parse (playerLevelRef.text); }
		set { playerLevelRef.text = value.ToString (); }
	}
		
	public float NextLevelPercent {
		get { return playerXPRef.GetComponent<Image> ().fillAmount; }
		set { playerXPRef.GetComponent<Image> ().fillAmount = value; }
	}

	public float HealthPercent {
		get { return healtBarRef.GetComponent<Image> ().fillAmount; }
		set { healtBarRef.GetComponent<Image> ().fillAmount = value; }
	}

	public float ResourcePercent{
		get { return resourceBarRef.GetComponent<Image> ().fillAmount; }
		set { resourceBarRef.GetComponent<Image> ().fillAmount = value; }
	}

	public GameObject AddStatus(Sprite sprite) {
		GameObject statusGO = Instantiate (statusPrefab);
		statusGO.GetComponent<Image> ().sprite = sprite;
		statusGO.transform.GetChild (0).GetComponent<Image> ().fillAmount = 0;
		statusGO.transform.SetParent (statusEffects.transform);
		return statusGO;
	}

	public GameObject SetSkill(int slot, Sprite icon, string hotkey) {
		GameObject skillSlot = skillPanel.transform.GetChild (slot-1).gameObject;
		skillSlot.GetComponent<Image> ().sprite = icon;
		skillSlot.transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = hotkey;
		return skillSlot;
	}
}
