﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public enum CatState
    {
        PATROL, RAGE, WAIT, DEAD
    }
    public CatState currentState;
    public float timeDelayToTeleport = 1f;
    public float waitTime = 3f;
    public float offsetY = 3f;
    public float downMass = 30f;
    public bool isTeleportedBack = true; //teleportou para o ponto de patrulha

    [Header("Ground Check")]
    public LayerMask floorLayer;
    public Transform groundCheckA;
    public Transform groundCheckB;

    [Header("Bools")]
    public bool isTouchInGround;
    public bool isLookToPlayer;
    public bool isBacking;

    private Rigidbody2D rb;
    private Vector2 target = Vector2.zero;
    private Transform player;
    private EnemyBehaviour behaviour;
    private float startMass;


    private void Start() 
    {
        behaviour = GetComponent<EnemyBehaviour>();
        rb = GetComponent<Rigidbody2D>();
        startMass = rb.mass;
    }

    void FixedUpdate()
    {
        isTouchInGround = Physics2D.OverlapArea(groundCheckA.position, groundCheckB.position, floorLayer);        
    }

    private void Update()
    {
        switch(currentState)
        {
            case CatState.PATROL:

                if(isTeleportedBack)
                {
                    behaviour.Patrol();
                }
                break;

            case CatState.WAIT:
                    if(player == null) {return;}
                    behaviour.ControlFlip(player);
                break;  

        }    
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        if(!isLookToPlayer && isTouchInGround)
        {
            ChangeState(CatState.PATROL);
        }
    }

    void ChangeState(CatState newState)
    {
        if(!isBacking)
        {
            currentState = newState;
        }

        switch (currentState)
        {
            case CatState.PATROL:
                if(!isBacking)
                {
                    rb.mass = startMass;
                    StartCoroutine(ResetPosition());
                }

            break;

            case CatState.WAIT:
                isTeleportedBack = false;
                StartCoroutine(WaitTime());
            break;

            case CatState.RAGE:
                rb.mass = downMass;
                isTeleportedBack = false;
                StopCoroutine(StartAttack());
                StartCoroutine(StartAttack());
            break;
        }

        if(isLookToPlayer)
        {
            StopCoroutine(WaitTime());
        }
    }

    IEnumerator ResetPosition()
    {
        isBacking = true;
        yield return new WaitForSeconds(timeDelayToTeleport);
        int rand = Random.Range(0, behaviour.wayPoints.Length);
        transform.position = behaviour.wayPoints[rand].position;
        isTeleportedBack = true;
        isBacking = false;
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(timeDelayToTeleport);
        transform.position = target;
    }

    IEnumerator WaitTouchGround()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => isTouchInGround);
        if(currentState != CatState.WAIT)
        {
            ChangeState(CatState.WAIT);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        
        switch(other.gameObject.tag)
        {
            case "Player":
                if(!isTouchInGround) {StopAllCoroutines(); return;}
                RaycastHit2D hit = behaviour.CheckRayCastToPosition(other.transform.position);
                player = hit.transform;

                if(hit.collider != null)
                {
                    print(hit.collider.gameObject.name);
                    
                    if(hit.collider.gameObject.tag == "Player" && isTouchInGround)
                    {
                        isLookToPlayer = true;
                        if(currentState != CatState.RAGE)
                        {
                            StopCoroutine(StartAttack());
                            ChangeState(CatState.RAGE);
                        }

                        Vector2 hitPos = hit.transform.position;
                        target = new Vector2(hitPos.x, hitPos.y + offsetY);
                    }
                    else
                    {
                        isLookToPlayer = false;
                    }
                }
                else
                {
                    isLookToPlayer = false;
                }

            break;
        }        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && currentState == CatState.RAGE)
        {
            isLookToPlayer = false;
            StartCoroutine(WaitTouchGround());
        }
    }
}
