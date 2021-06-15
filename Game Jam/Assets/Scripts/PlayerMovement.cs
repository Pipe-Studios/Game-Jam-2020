using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D playerController;
    public Animator animator;
    public GameObject hog;
    public GameObject cheetah;
    public GameObject impala;
    public Animator sky;

    public float playerMoveSpeed = 2f;
    public float oldPlayerMoveSpeed;
    public GameObject[] CheckpointsList;
    float playerHorizontalMove;
    Vector3 startingHogPos;
    Vector3 startingCheetahPos;
    Vector3 startingImpalaPos;
    Vector3 startingPlayerPos;

    private void Awake()
    {
        oldPlayerMoveSpeed = playerMoveSpeed;

        startingPlayerPos = this.transform.position;
        startingHogPos = hog.transform.position;
        startingCheetahPos = cheetah.transform.position;
        startingImpalaPos = impala.transform.position;

        CheckpointsList = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetEntityPosition();
        }

        playerHorizontalMove = Input.GetAxisRaw("Horizontal") * playerMoveSpeed;

        if (playerHorizontalMove != 0)
        {
            if (!hog.GetComponent<WayPoints>().isJumping)
            {
                this.GetComponent<AudioSource>().UnPause();
            }
            
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
            this.GetComponent<AudioSource>().Pause();
        }

        animator.SetBool("Crouching", hog.GetComponent<WayPoints>().isCrouching);
        animator.SetBool("Jumping", cheetah.GetComponent<WayPoints>().isJumping);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerController.Move(playerHorizontalMove * Time.fixedDeltaTime, false, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Death Zone")
        {
            this.transform.position = startingPlayerPos;
            ResetEntityPosition();
        }
    }

    void ResetEntityPosition()
    {
        hog.transform.position = startingHogPos;
        cheetah.transform.position = startingCheetahPos;
        impala.transform.position = startingImpalaPos;

        cheetah.SetActive(false);
        impala.SetActive(false);

        this.transform.position = startingPlayerPos;
        this.playerMoveSpeed = oldPlayerMoveSpeed;

        foreach (GameObject cp in CheckpointsList)
        {
            cp.GetComponent<Checkpoint>().activated = false;
            cp.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        sky.SetTrigger("Checkpoint 0");

        hog.GetComponent<WayPoints>().currentWayPoint = 0;
        cheetah.GetComponent<WayPoints>().currentWayPoint = 0;
        impala.GetComponent<WayPoints>().currentWayPoint = 0;
    }
}
