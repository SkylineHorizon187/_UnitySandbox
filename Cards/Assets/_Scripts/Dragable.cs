using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler {
	public GameObject line;
	public GameObject targetIcon;
	public GameObject placeHolder;
	public GameObject moveHolder;
	[MaskEnum] public General.TargetTypes attackType;

	private LineRenderer lr;
	private Vector3 tmpVector;
	private Image dragObject;
	private List<RaycastResult> hits = new List<RaycastResult>();
	private Outline o;

	void Start() {
		lr = line.GetComponent<LineRenderer> ();
		dragObject = GetComponent<Image> ();
		StartCoroutine (AnimateLine ());
	}

	public void OnBeginDrag(PointerEventData evtData) {
		dragObject.raycastTarget = false;
		placeHolder.SetActive (true);
		placeHolder.transform.SetSiblingIndex (dragObject.transform.GetSiblingIndex ());
		dragObject.transform.SetParent (dragObject.transform.parent.parent);
	}

	public void OnDrag(PointerEventData evtData) {
		tmpVector = Camera.main.ScreenToWorldPoint (evtData.position);
		tmpVector.z = 1;

		bool inParent = false;
		EventSystem.current.RaycastAll (evtData, hits);
		for (int i = 0; i < hits.Count; i++) {
			if (hits[i].gameObject == placeHolder.transform.parent.gameObject) {
				inParent = true;
				break;
			}
		}

		if (inParent) {
			transform.position = tmpVector;

			if (targetIcon.activeSelf) {
				targetIcon.SetActive (false);
				line.SetActive (false);
			}
			int SI = placeHolder.transform.parent.childCount;
			for (int i = 0; i < placeHolder.transform.parent.childCount; i++) {
				if (transform.position.x < placeHolder.transform.parent.GetChild (i).position.x) {
					SI = i;
					if (placeHolder.transform.GetSiblingIndex () < SI) {
						SI--;
					}
					break;
				}
			}
			placeHolder.transform.SetSiblingIndex (SI);
		} else {
			if (attackType == General.TargetTypes.Player) {
				if (!targetIcon.activeSelf) {
					targetIcon.SetActive (true);
					line.SetActive (true);

					tmpVector = transform.position;
					tmpVector.z = 1;
					lr.SetPosition (0, tmpVector);
				} else {
					lr.SetPosition (1, tmpVector);
					targetIcon.transform.position = tmpVector;
					targetIcon.GetComponent<Image> ().color = Color.red;

					GameObject go = evtData.pointerCurrentRaycast.gameObject;
					if (go != null) {
						Target t = go.GetComponent<Target> ();
						if (t != null) {
							if (t.targetType == attackType) {
								targetIcon.GetComponent<Image> ().color = Color.green;
							}
						}
					}
				}
			} else {
				if (dragObject.transform.parent != moveHolder.transform) {
					dragObject.transform.SetParent (moveHolder.transform);
					placeHolder.SetActive (false);
					for (int i = 0; i < placeHolder.transform.parent.childCount; i++) {
						o = placeHolder.transform.parent.GetChild (i).GetComponent<Outline> ();
						if (o != null) {
							if (o.enabled) {
								placeHolder.transform.parent.GetChild (i).SetParent (moveHolder.transform);
								i--;
							}
						}
					}
				}
				moveHolder.transform.position = tmpVector;
			}
		}
	}

	public void OnEndDrag(PointerEventData evtData) {
		if (targetIcon.activeSelf) {
			targetIcon.SetActive (false);
			line.SetActive (false);
		}
		dragObject.raycastTarget = true;
		dragObject.transform.SetParent (placeHolder.transform.parent);
		dragObject.transform.SetSiblingIndex (placeHolder.transform.GetSiblingIndex ());
		placeHolder.SetActive (false);
	}

	public void OnDrop(PointerEventData evtData) {

	}

	private IEnumerator AnimateLine() {
		float framesPerSecond = 30;
		Renderer r = line.GetComponent<Renderer> ();
		Vector2 offset = Vector2.zero;

		while (true) {
			if (line.activeSelf) {
				offset.x -= .05f;
				if (offset.x < -1) {
					offset.x += 1;
				}
				r.sharedMaterial.SetTextureOffset ("_MainTex", offset);
			}
			yield return new WaitForSeconds(1f / framesPerSecond);
		}	
	}
}
