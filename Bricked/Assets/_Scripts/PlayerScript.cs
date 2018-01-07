using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public static PlayerScript ThePlayer;
    public bool isAlive = true;

    public float moveSpeed;
    public GameObject firePoint;
    public GameObject bulletPrefab;
    public GameObject playerDeath;
    public int playerHealth;
    public int currentHealth;
    public int bulletDamage;
    public Image healthGlobe;
    public GameObject shield;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Collider2D col;
    private bool isShielded;
    private float shieldTimer = 6f;
    private float shieldTimeRemaining = 0;

    private SpriteRenderer shieldRenderer;

    void Start() {
        ThePlayer = this;
        rb = GetComponent<Rigidbody2D>();
        shieldRenderer = transform.Find("shield").GetComponent<SpriteRenderer>();
        currentHealth = playerHealth;
    }

    void Update() {
        // WASD Movement
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Rotation
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Fire Bullet
        if (Input.GetMouseButtonDown(0)) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
            BulletScript BS = bullet.GetComponent<BulletScript>();
            Vector3 fp = firePoint.transform.position - transform.position;
            BS.direction = new Vector2(fp.x, fp.y);
            BS.startVelocity = 12f;
            BS.deathMagnitude = 3f;
            BS.damage = bulletDamage;
        }

        // Use shiled charge
        if (Input.GetMouseButtonDown(1)) {
            if (shieldHolder.SH.GetCharges() > 0) {
                shieldHolder.SH.RemoveCharge();
                shieldTimeRemaining += shieldTimer;
                shieldRenderer.enabled = true;
            }
        }

        // turn on/off shiled
        if (shieldTimeRemaining > 0) {
            isShielded = true;
            shield.SetActive(true);
            shieldTimeRemaining -= Time.deltaTime;

            // Add Flashing at end of time.
            if (shieldTimeRemaining < 1.5f) {
                float temp = Mathf.Round(shieldTimeRemaining * 10);
                if (temp % 2 == 0) {
                    shieldRenderer.enabled = false;
                }
                else {
                    shieldRenderer.enabled = true;
                }
            }

        }
        else {
            isShielded = false;
            shield.SetActive(false);
        }
    }

    void FixedUpdate() {
        Vector2 curPos = transform.position;
        rb.MovePosition(curPos + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (!isShielded) {
            if (other.collider.tag == "Bullet") {
                Destroy(other.gameObject);
                currentHealth -= other.collider.gameObject.GetComponent<BulletScript>().damage;
            }
            else if (other.collider.tag == "Wall") {
                currentHealth -= 1;
            }
        }

        UpdateHealthGlobe();
        if (currentHealth <= 0) {
            KillPlayer();
        }
    }

    void KillPlayer() {
        isAlive = false;
        Destroy(Instantiate(playerDeath, transform.position, Quaternion.identity), 5f);
        Destroy(gameObject);
    }

    public void UpdateHealthGlobe() {
        healthGlobe.fillAmount = (float)currentHealth / (float)playerHealth;
    }
}
