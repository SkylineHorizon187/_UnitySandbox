using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    // Prefabs
    public GameObject DiskPrefab;
    public GameObject CrosshairPrefab;

    // Quick access to Components
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    // Player Mouse Tracking
    private Vector2 MoveDirection;
    private Vector2 MouseMove;
    private GameObject Crosshair;

    //[HideInInspector]
    public int Ammo = 5;

    // Player Stats
    [Range(1, 2)]
    public int Team = 1;

    public float HealthMax = 10f;
    public float MovementSpeed = 5f;
    public float BasePower = 5f;
    public float MaximumCharge = 20f;
    public float ChargeRate = 5f;

    // Other Stuff
    private bool isCharging;
    private float CurrentCharge;
    private float HealthCurrent;

    // Use this for initialization
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        HealthCurrent = HealthMax;
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

                // Create and set Disk properties
                Vector3 DiskDirection = (Crosshair.transform.position - transform.position).normalized;
                GameObject disk = Instantiate(DiskPrefab, transform.position + DiskDirection, Quaternion.identity);
                disk.GetComponent<SpriteRenderer>().color = sr.color;
                disk.GetComponent<_Tags>().AddTag("Team" + Team);

                Vector2 force = new Vector2(DiskDirection.x, DiskDirection.y) * (BasePower + CurrentCharge);
                disk.GetComponent<DiskLogic>().SetForce(force);
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


    public void TakeDamage(float value) {
        HealthCurrent -= value;
        CheckDead();
    }

    void CheckDead() {
        if (HealthCurrent <= 0f) {
            Destroy(gameObject);
        }
    }

    public void GetAmmo() {
        Ammo += 1;
    }

}
