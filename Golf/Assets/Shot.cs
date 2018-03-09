using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public GameObject ball;
	public Text helpText;
	public float shotTime;

	private float yStart, yEnd;
	private float xLeft, xRight;
	private float xAccStart, xAccEnd;
	private Vector2 mPos;
	private bool takingShot;
	private Smack smack;

	void Start () {
		smack = ball.GetComponent<Smack> ();
		RectTransform rt = GetComponent<RectTransform> ();

		float yPos = Screen.height - transform.position.y;
		yStart = yPos - rt.rect.size.y / 2 + 5;
		yEnd = yPos + rt.rect.size.y / 2;

		float xPos = Screen.width - transform.position.x;
		xLeft = xPos - rt.rect.size.x / 2;
		xRight = xPos + rt.rect.size.x / 2;

		takingShot = false;
	}
	
	void Update () {
		mPos = Input.mousePosition;
		if (Input.GetMouseButton (0)) {
			if (mPos.x > xLeft && mPos.x < xRight && mPos.y < yStart) {
				takingShot = true;
			}
		} else {
			takingShot = false;
		}
	}

	public void OnPointerEnter (PointerEventData eventData) 
	{
		if (takingShot) {
			xAccStart = eventData.position.x;
			shotTime = Time.time;
		}
	}

	public void OnPointerExit (PointerEventData eventData) 
	{
		if (takingShot && Input.mousePosition.y > yEnd) {
			shotTime = Time.time - shotTime;
			xAccEnd = eventData.position.x;
			takingShot = false;
			smack.TakeShot (250 - shotTime * 300, xAccStart - xAccEnd);
		}
	}

}
