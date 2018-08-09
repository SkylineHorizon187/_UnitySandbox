using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour {

    public float UnitSpeed;
    public float UnitTurnRate;
    public float UnitHealth;
    public float UnitResistance;
    public Image healthBar;
    public Gradient SpeedGradient;
    public Canvas myCanvas;

    private GameControl GC;
    private List<Node> Path;
    private int pathPoint;
    private Vector3 worldPoint;
    private float StartHealth;

    void Start () {
        pathPoint = 0;
        StartHealth = UnitHealth;
        GC = FindObjectOfType<GameControl>();
        Path = GC.StartToExitPath;

        worldPoint = GC.TilePosition(Path[pathPoint + 1].x, Path[pathPoint + 1].y);
        transform.LookAt(worldPoint);

        // Unit apperance
        // scale dependant on health
        Vector3 fat = transform.GetChild(0).localScale;
        fat.x += UnitHealth / 125;
        fat.z += UnitHealth / 125;
        transform.GetChild(0).localScale = fat;
        // color dependant on speed
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = SpeedGradient.Evaluate(Mathf.Clamp01(UnitSpeed-1));
	}
	
	void Update () {
        // Rotate Unit towards target (slow it's movement speed until within 2 degrees)
        Quaternion targetRotation = Quaternion.LookRotation(worldPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, UnitTurnRate * Time.deltaTime);
        // Make Canvas (healthbar) always face the camera
        myCanvas.transform.rotation = Quaternion.identity;
        if (Quaternion.Angle(transform.rotation, targetRotation) > 1)
        {
            return;
        } else
        {
            transform.rotation = targetRotation;
            // Make Canvas (healthbar) always face the camera
            myCanvas.transform.rotation = Quaternion.identity;
        }

        // Move Unit towards target
        transform.position = Vector3.MoveTowards(transform.position, worldPoint, UnitSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, worldPoint) < .02f)
        {
            pathPoint++;
            if (pathPoint > Path.Count-2)
            {
                // we have reached the final point
                Destroy(gameObject);
            } else
            {
                worldPoint = GC.TilePosition(Path[pathPoint + 1].x, Path[pathPoint + 1].y);
            }
        }
    }   
    
    public void TakeDamage(float amount)
    {
        UnitHealth -= amount - (amount * UnitResistance);
        healthBar.fillAmount = UnitHealth / StartHealth;

        if (UnitHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
