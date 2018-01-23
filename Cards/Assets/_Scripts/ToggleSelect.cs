using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleSelect : MonoBehaviour, IPointerDownHandler {

	private Outline o;
	private Dragable d;

	void Start () {
		o = GetComponent<Outline> ();
		d = GetComponent<Dragable> ();
	}
	
	public void OnPointerDown(PointerEventData evtData) {
		if (Input.GetKey (KeyCode.LeftControl)) {
			if (d != null) {
				if (d.attackType != General.TargetTypes.Player) {
					o.enabled = !o.enabled;
				}
			}
		}
	}
}
