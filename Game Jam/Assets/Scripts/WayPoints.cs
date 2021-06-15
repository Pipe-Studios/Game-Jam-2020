using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    // Points from unity interface
    public Animator animator;
    public GameObject player;

    // Waypoint variables
    public Transform[] wayPointList;
    public int currentWayPoint = 0;
    Transform targetWayPoint;

    // Constants
    public float speed = 10f;
    public float sprintSpeed = 3f;
    public float crouchSpeed = 0.5f;
    public float prepareTime = 1.5f;
    public float animationSpeedModifier = 1.5f;

    // Trigger variables
    public bool isPreparing = false;
    public bool isJumping = false;
    public bool isSprinting = false;
    public bool isCrouching = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if we have somewere to walk
        if (currentWayPoint < this.wayPointList.Length)
        {
            targetWayPoint = wayPointList[currentWayPoint];
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        // rotate towards the target
        //transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (transform.position == targetWayPoint.position)
        {
            if (wayPointList[currentWayPoint].tag == "Jumping Waypoint Start")
            {
                isJumping = true;
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, player.GetComponent<CharacterController2D>().m_JumpForce * this.GetComponent<Rigidbody2D>().gravityScale / 2));
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, player.GetComponent<CharacterController2D>().m_JumpForce * player.GetComponent<Rigidbody2D>().gravityScale / 2));
            }
            else if (wayPointList[currentWayPoint].tag == "Jumping Waypoint End")
            {
                isJumping = false;
            }
            else if (wayPointList[currentWayPoint].tag == "Sprinting Waypoint Start")
            {
                StartCoroutine(sprint());
                player.GetComponent<Animator>().speed = animationSpeedModifier;
            } 
            else if (wayPointList[currentWayPoint].tag == "Sprinting Waypoint End")
            {
                isSprinting = false;
                animator.SetBool("Sprinting", false);
                animator.speed = 1f;
                speed /= sprintSpeed;
                player.GetComponent<PlayerMovement>().playerMoveSpeed /= sprintSpeed;
                player.GetComponent<Animator>().speed = 1f;
            }
            else if (wayPointList[currentWayPoint].tag == "Crouching Waypoint Start")
            {
                isCrouching = true;
                speed *= crouchSpeed;
                player.GetComponent<PlayerMovement>().playerMoveSpeed *= crouchSpeed;
            }
            else if (wayPointList[currentWayPoint].tag == "Crouching Waypoint End")
            {
                isCrouching = false;
                speed /= crouchSpeed;
                player.GetComponent<PlayerMovement>().playerMoveSpeed /= crouchSpeed;
            }

            currentWayPoint++;
            targetWayPoint = wayPointList[currentWayPoint];
        }
    }

    // Async function for starting sprint and preparation (cheetah)
    IEnumerator sprint()
    {
        isPreparing = true;
        animator.SetBool("Preparing", true);
        float previousSpeed = speed;
        speed = 0f;
        Collider2D coll = this.GetComponent<Collider2D>();
        coll.sharedMaterial.friction = 100f;

        yield return new WaitForSeconds(prepareTime);

        isPreparing = false;
        
        animator.SetBool("Preparing", false);
        animator.SetBool("Sprinting", true);
        animator.speed = animationSpeedModifier;
        isSprinting = true;
        speed = previousSpeed * sprintSpeed;
        player.GetComponent<PlayerMovement>().playerMoveSpeed *= sprintSpeed;
    }
}
