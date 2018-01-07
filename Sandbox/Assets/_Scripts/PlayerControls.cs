using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    public GameObject DiskPrefab;
    public GameObject CrosshairPrefab;

    private float MovementSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 MoveDirection;
    private Vector2 MouseMove;
    private GameObject Crosshair;

    //[HideInInspector]
    public int Ammo = 5;
    private bool isCharging;

    public float BasePower = 5f;
    private float CurrentCharge;
    public float MaximumCharge = 20f;
    public float ChargeRate = 5f;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        Cursor.visible = false;

        Crosshair = Instantiate(CrosshairPrefab);
    }

    // Update is called once per frame
    void Update() {

        if (Ammo > 0) {
            if (Input.GetButtonDown("Fire1")) {
                isCharging = true;
                CurrentCharge = 0f;
            }
        }

        if (isCharging) {
            MoveDirection = Vector2.zero;

            if (CurrentCharge < MaximumCharge) {
                CurrentCharge += Time.deltaTime * ChargeRate;
            }
            else {
                CurrentCharge = MaximumCharge;
            }
            

            if (Input.GetButtonUp("Fire1")) {
                isCharging = false;
                Ammo -= 1;

                Vector3 DiskDirection = (Crosshair.transform.position - transform.position).normalized;
                GameObject disk = Instantiate(DiskPrefab, transform.position + DiskDirection, Quaternion.identity);
                Rigidbody2D drb = disk.GetComponent<Rigidbody2D>();
                Vector2 force = new Vector2(DiskDirection.x, DiskDirection.y) * (BasePower + CurrentCharge);
                drb.AddForce(force, ForceMode2D.Impulse);
            }
        }
        else {
            // Get Left Input Axis for movement.
            MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }


        // Crosshair (Mouse) Movement
        Vector3 CrosshairPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CrosshairPosition.z = 1;
        Crosshair.transform.position = CrosshairPosition;


    }

    void FixedUpdate() {
        // Move your Pawn based on direction, speed, and time.
        rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + MoveDirection * MovementSpeed * Time.fixedDeltaTime);
    }




}
