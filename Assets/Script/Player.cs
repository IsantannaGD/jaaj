﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FMODUnity;

[Serializable]
public struct PlayerStruct
{
    [Header("Player")]
    public SpriteRenderer sr;
    public Rigidbody2D rigB;
    public Animator anim;
    public Transform groundCheckA;
    public Transform groundCheckB;    

    [Header("FX")]
    public ParticleSystem jumpParticle;

    [Header("Bools")]
    public bool IsJumping;
    public bool Doublejump;
}


public class Player : MonoBehaviour
{
    public PlayerStruct[] players;

    public SoundController PlayerSound;
    public float Speed;
    public float JumpForce;

    public int IdPlayer;

    public LayerMask FloorCheck;

    public GameObject[] Power; // Lista com sprites dos projeteis.

    private Vector3 movement;

    public CinemachineVirtualCamera CMCam;


    void Update()
    {
        Move();
        Changer();
        GroundCheck();
        Jump();
        Skill();       
    }

    void Move()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        players[IdPlayer].rigB.transform.position += movement * Time.deltaTime * Speed;
        players[IdPlayer].anim.SetBool("Jumping", false);

        if (movement != Vector3.zero)// Checkagem de movimento para animação.
        {
            players[IdPlayer].anim.SetBool("Move", true);            
        }
        else
        {
            players[IdPlayer].anim.SetBool("Move", false);            
        }
        if (Input.GetAxis("Horizontal") < 0f)
        {
            players[IdPlayer].rigB.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (Input.GetAxis("Horizontal") > 0f)
        {
            players[IdPlayer].rigB.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }


        if (players[IdPlayer].IsJumping != false)
        {
            players[IdPlayer].anim.SetBool("Jumping", true);
        }
    }

    void Skill()
    {
        if (Input.GetKeyDown(KeyCode.E) && IdPlayer == 0)
        {
            //Instantiate(Power[0], transform.position, transform.rotation); ////Opção para Gerar projetil.
            return;
        }
        else if (Input.GetKeyDown(KeyCode.E) && IdPlayer == 1)
        {
            //Instantiate(Power[1], transform.position, transform.rotation); //Opção para Gerar projetil.
            return;
        }

    }

    void Changer() //Metodo de troca do heroi.
    {
        
        if (Input.GetKeyDown(KeyCode.Q) && IdPlayer == 0)
        {
            ChangerSortingOrder(1);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && IdPlayer == 1)
        {
            ChangerSortingOrder(-1);
            return;
        }
        
        CameraFollow();
    }

    void ChangerSortingOrder(int nextId)
    {
        players[IdPlayer].sr.sortingOrder = 0;
        IdPlayer += nextId;
        players[IdPlayer].sr.sortingOrder = 1;
    }

    void GroundCheck()
    {
        players[IdPlayer].IsJumping = !Physics2D.OverlapArea(players[IdPlayer].groundCheckA.position, players[IdPlayer].groundCheckB.position, FloorCheck);
    }


    void Jump()
    {       

        if (Input.GetButtonDown("Jump"))
        {
            if(!players[IdPlayer].IsJumping)
            {
                players[IdPlayer].rigB.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                players[IdPlayer].jumpParticle.Play();
                //PlayerSound.PlayerJump();

                players[IdPlayer].IsJumping = true;
                players[IdPlayer].Doublejump = true;                                
                return;
            }
            else if(players[IdPlayer].Doublejump)
            {
                //zera a velocidade para manter a estabilidade do corpo
                players[IdPlayer].rigB.velocity = new Vector2(players[IdPlayer].rigB.velocity.x, 0);
                players[IdPlayer].rigB.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                players[IdPlayer].Doublejump = false;                
                return;
            }            
        }
    }

    void CameraFollow ()
    {
        CMCam.Follow = players[IdPlayer].rigB.transform;
    }
}



