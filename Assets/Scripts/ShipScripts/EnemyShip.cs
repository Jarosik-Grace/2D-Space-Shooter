using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyShip : Ship
{
    Vector3 targetPosition;
    private enum State{Roam, Chase, Shoot};
    [SerializeField]private float chaseDist, roamDist, shootDist;
    OutOfBounds outOfBounds;

    State currentState;

    //public event Action OnDeath;

    protected override void CustomStart() {
        currentState = State.Roam;
        targetPosition = (Vector2)transform.position + new Vector2(UnityEngine.Random.Range(-roamDist, roamDist), UnityEngine.Random.Range(-roamDist, roamDist));
        outOfBounds = GetComponent<OutOfBounds>();
    }
    
    void Update() {
        if (PlayerShip.Instance == null) {
            return;
        }

        if (currentState == State.Roam) {
            if (outOfBounds.getDistance(transform.position, targetPosition) < 1f) {
                targetPosition = (Vector2)transform.position + new Vector2(UnityEngine.Random.Range(-roamDist, roamDist), UnityEngine.Random.Range(-roamDist, roamDist));
            }

        if (outOfBounds.getDistance(transform.position,PlayerShip.Instance.transform.position) < chaseDist) {
            currentState = State.Chase;
        }
    } else if (currentState == State.Chase) {
        targetPosition = PlayerShip.Instance.transform.position;
        if (outOfBounds.getDistance(transform.position, PlayerShip.Instance.transform.position) < shootDist) {
            currentState = State.Shoot;
        } else if (outOfBounds.getDistance(transform.position, PlayerShip.Instance.transform.position) > chaseDist * 1.2f) {
            currentState = State.Roam;
        }
    } else {
        targetPosition = PlayerShip.Instance.transform.position;
        if (outOfBounds.getDistance(transform.position, PlayerShip.Instance.transform.position) > shootDist) {
            currentState = State.Chase;
        }

        if(canShoot) {
            StartCoroutine(Shoot(moveDirection,shootForce));
        }
    }
    
    targetPosition = outOfBounds.getCoords(targetPosition);
    moveDirection = -outOfBounds.getDirection(transform.position,targetPosition).normalized;
    }
    

    protected override void Move() {
        if (moveDirection.magnitude > 0) {
            rigidBody.linearVelocity = moveDirection * moveSpeed;
        } else {
            rigidBody.linearVelocity -= rigidBody.linearVelocity * friction;
        }

        transform.up += ((Vector3)moveDirection - transform.up) / 5;
    }
}
