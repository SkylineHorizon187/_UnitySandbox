using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {
	public Camera cam;
	public float camSpeed;
	public float zoomSpeed;
	public float zoomMin;
	public float zoomMax;

	struct camBounds {
		public Vector3 TopLeft;
		public Vector3 BottomRight;
	}
	private camBounds bounds = new camBounds();
	private Vector2 offSet = new Vector2(5f, -3f);

    private void Start()
    {
        UpdateBounds();
    }

    void LateUpdate() {
		float hTrans = 0f, vTrans = 0f;
		// verticle movement
		vTrans += (Input.GetKey (KeyCode.W)) ? camSpeed * Time.deltaTime : 0f;
		vTrans -= (Input.GetKey (KeyCode.S)) ? camSpeed * Time.deltaTime : 0f;
		// horizontal movement
		hTrans -= (Input.GetKey (KeyCode.A)) ? camSpeed * Time.deltaTime : 0f;
		hTrans += (Input.GetKey (KeyCode.D)) ? camSpeed * Time.deltaTime : 0f;
		// translate and clamp controller
        if (Menu.buildMenu.isOpen && hTrans+vTrans != 0) {
            Menu.buildMenu.MenuClose();
        }
		transform.Translate(hTrans, 0f, vTrans);
		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, bounds.TopLeft.x, bounds.BottomRight.x), 
			0f,
			Mathf.Clamp (transform.position.z, bounds.BottomRight.z + offSet.y/2, bounds.TopLeft.z + offSet.y/2));

		// mousewheel zoom (the camera)
		float zoom = Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        if (Menu.buildMenu.isOpen && zoom != 0)
        {
            Menu.buildMenu.MenuClose();
        }
        cam.transform.Translate (0f, -zoom, 0f, Space.Self);
		cam.transform.localPosition = new Vector3 (0f, Mathf.Clamp (cam.transform.position.y, zoomMin, zoomMax), 0f);
	}

	public void UpdateBounds() {
		GameObject gameBoard = GameObject.Find ("GameControl");
		if (gameBoard != null) {
            GameControl GC = gameBoard.GetComponent<GameControl>();

            bounds.TopLeft = new Vector3(-GC.floorX / 2 - 2, 0, GC.floorY / 2 - 2);
            bounds.BottomRight = new Vector3(GC.floorX / 2 + 2, 0, -GC.floorY / 2 - 2);

		}
	}

}
