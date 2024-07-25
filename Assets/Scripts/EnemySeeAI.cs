using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySeeAI : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float farDistance = 10f;

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
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (!player.GetComponent<AudioSource>().mute && distanceToPlayer < detectionRadius)
        {
            fleeDirection = transform.position - player.transform.position;
            fleeDirection.Normalize();
            newDestination = transform.position + fleeDirection * farDistance;
            agent.SetDestination(newDestination);
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
}
