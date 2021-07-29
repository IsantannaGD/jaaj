﻿using System;
using System.Collections;
using UnityEngine;

public class Bull : MonoBehaviour, ISkill
{   
    public enum BullState
    {
        PATROL, RUN, STUN, DEAD, FOLLOW
    }
        
    private Rigidbody2D rb;
    private EnemyBehaviour behaviour;
    private Animator Anim;
    private int changeAnimation;
    public BullState currentState;
    public ParticleSystem chargeParticle;
    public float chargeSpeed;
    public bool isRevived;
    
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        behaviour = GetComponent<EnemyBehaviour>();
        Anim = GetComponent<Animator>();
        behaviour.target = behaviour.wayPoints[0];        
    }

    private void Update()
    {
        switch (currentState)
        {
            case BullState.PATROL:
                behaviour.Patrol();
            break;
            
            case BullState.RUN:
                Skill();
            break;
        }
        AnimationChanger();
                
    }
    
    public void Skill()
    {
        if(behaviour.isLookLeft) { behaviour.side = 1; } else { behaviour.side = -1;}
        transform.position += transform.right * behaviour.side * chargeSpeed * Time.deltaTime;
    }

    void Dead()
    {
        //animacao de morte
        //indicador que pode ser revivido
    }

    void ChangeState(BullState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case BullState.STUN:
                StopCoroutine(Delay(BullState.PATROL, 2.5f));
                StartCoroutine(Delay(BullState.PATROL, 2.5f));
            break;

            case BullState.DEAD:
                behaviour.Dead();
                Dead();
            break;
        }
    }

    IEnumerator Delay(BullState nextState, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(nextState);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch(other.gameObject.tag)
        {
            case "Wall":
                if(currentState == BullState.RUN)
                {
                    ChangeState(BullState.STUN);
                }

            break;

            case "PlayerHit":
                if(currentState != BullState.DEAD)
                {   
                   ChangeState(BullState.DEAD);
                }

            break;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        
        switch(other.gameObject.tag)
        {
            case "Player":
                RaycastHit2D hit = behaviour.CheckRayCastHorizontal();

                if(hit.collider != null)
                {
                    print(hit.collider.gameObject.name);

                    if(hit.collider.gameObject.tag == "Player" && currentState != BullState.RUN)
                    {
                        ChangeState(BullState.RUN);
                        chargeParticle.Play();
                    }
                }
            break;
        }        
    } 
    
    void AnimationChanger ()
    {
        changeAnimation = ((int)currentState);
        Anim.SetInteger("State", changeAnimation);
    }
}
