using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]private float LifeTime;
    private string target;
    protected Rigidbody2D rb;
    protected float speed;

    void Start()
    {

    }

    public void setTarget(string name, Vector3 dir, float force) {
        target = name;
        rb = GetComponent<Rigidbody2D>();
        speed = force;
        transform.up = dir;
        StartCoroutine(startCountdown());
    }

    public IEnumerator startCountdown(){
        yield return new WaitForSeconds(LifeTime);
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag(target)) {
        
           
            Ship hitShip = other.GetComponent<Ship>();

            if (hitShip != null) {
                hitShip.takeDamage();
            }
        }
    }
}
