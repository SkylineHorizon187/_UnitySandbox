using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
	public GameObject line;
	public GameObject targetIcon;

	private LineRenderer lr;
	private Vector3 tmpVector;

	void Start() {
		lr = line.GetComponent<LineRenderer> ();
	}

	public void OnBeginDrag(PointerEventData evtData) {
		targetIcon.SetActive (true);
		line.SetActive (true);

		tmpVector = transform.position;
		tmpVector.z = 1;
		lr.SetPosition (0, tmpVector);
	}

	public void OnDrag(PointerEventData evtData) {
		tmpVector = Camera.main.ScreenToWorldPoint (evtData.position);
		tmpVector.z = 1;
		lr.SetPosition (1, tmpVector);
		targetIcon.transform.position = tmpVector;

		if (evtData.pointerCurrentRaycast.gameObject != null) {
			print (evtData.pointerCurrentRaycast.gameObject.name);
			if (evtData.pointerCurrentRaycast.gameObject.name == "PlayerImage") {
				targetIcon.GetComponent<Image> ().color = Color.green;
			} else {
				targetIcon.GetComponent<Image> ().color = Color.red;
			}
		} else {
			targetIcon.GetComponent<Image> ().color = Color.red;
		}
	}

	public void OnEndDrag(PointerEventData evtData) {
		targetIcon.SetActive (false);
		line.SetActive (false);
	}

}
