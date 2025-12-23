using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShip : Ship
{
    public int defaultHealth;
    public static PlayerShip Instance;
    public Animator animator;
    public Text healthText;

  
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    protected override void CustomStart() {
        defaultHealth = health;
        UpdateHealthUI();
    }

    public override void takeDamage() {
        base.takeDamage();

        if (healthBar != null) {
            healthBar.value = health;
        }

        UpdateHealthUI();
        
        if (health <= 0) {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }

    protected override void Move() {
        if (moveDirection.magnitude > 0) {
            rigidBody.linearVelocity = moveDirection * moveSpeed;
        }
        else {
            rigidBody.linearVelocity -= rigidBody.linearVelocity * friction;
        }
    }

    Vector2 GetMousePos() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
        return Worldpos2D;
    }

    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        Vector2 shootDirection = (GetMousePos() - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.eulerAngles = new Vector3(0, 0, -90 + Mathf.Atan2(shootDirection.y, shootDirection.x) * 180 / Mathf.PI);
    
        if (Input.GetMouseButton(0)) {
            if (canShoot) {
                StartCoroutine(Shoot(shootDirection, shootForce));
            }
            animator.SetBool("isShooting", true);
        } else {
            animator.SetBool("isShooting", false);
        }
    }

    private void UpdateHealthUI() {
        if (healthText != null) {
            healthText.text = "Health: " + health.ToString();
        }
    }

    public void SetHealthText(Text newHealthText) {
        healthText = newHealthText;
        UpdateHealthUI();
    }
}
