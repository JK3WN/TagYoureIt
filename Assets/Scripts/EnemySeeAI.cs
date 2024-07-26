using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySeeAI : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float farDistance = 10f;

    public float sightRange = 10f;
    public float fieldOfViewAngle = 45f;

    public GameObject player;
    public GameObject EyesOpen, EyesClosed;

    private NavMeshAgent agent;
    private Vector3 fleeDirection;
    private Vector3 newDestination;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine("Blink");
    }

    // Update is called once per frame
    void Update()
    {
        fleeDirection = transform.position - player.transform.position;
        fleeDirection.Normalize();
        newDestination = transform.position + fleeDirection * farDistance;
        agent.SetDestination(newDestination);
        if (IsPlayerInSight())
        {
            player.GetComponent<PlayerController>().stopped = true;
        }
        else
        {
            player.GetComponent<PlayerController>().stopped = false;
        }
    }

    IEnumerator Blink()
    {
        while (GameManager.isPlaying)
        {
            yield return new WaitForSeconds(5f);
            EyesClosed.SetActive(true);
            EyesOpen.SetActive(false);
            yield return new WaitForSeconds(5f);
            EyesClosed.SetActive(false);
            EyesOpen.SetActive(true);
        }
    }

    private bool IsPlayerInSight()
    {
        if (EyesClosed.activeSelf) return false;

        Vector3 directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < sightRange)
        {
            float angle = Vector3.Angle(directionToPlayer, -transform.forward);
            if (angle < fieldOfViewAngle / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, sightRange))
                {
                    if (hit.collider.gameObject == player)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
