using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated = false;
    public static GameObject[] CheckPointsList;

    public GameObject cheetah;
    public GameObject impala;
    public Animator sky;

    void Start()
    {
        CheckPointsList = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    private void ActivateCheckPoint()
    {
        foreach (GameObject cp in CheckPointsList)
        {
            cp.GetComponent<Checkpoint>().activated = false;
            //cp.SetActive(false);
        }

        activated = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            ActivateCheckPoint();
            
            if (this.gameObject.name == "CheckpointC1")
            {
                sky.SetTrigger("Checkpoint 1");
                cheetah.SetActive(true);
            } 
            else if (this.gameObject.name == "CheckpointC2")
            {
                sky.SetTrigger("Checkpoint 2");
                impala.SetActive(true);
            }

            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public static Vector3 GetActiveCheckPointPosition()
    {
        Vector3 result = new Vector3(0, 0, 0);

        if (CheckPointsList != null)
        {
            foreach (GameObject cp in CheckPointsList)
            {
                if (cp.GetComponent<Checkpoint>().activated)
                {
                    result = cp.transform.position;
                    break;
                }
            }
        }

        return result;
    }
}
