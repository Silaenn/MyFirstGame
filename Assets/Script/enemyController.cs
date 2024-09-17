using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    public float speed = 2f;
     public float patrolRadius = 15f;
    private Vector2 randomTarget;
    private bool isChasingPlayer = false;


    void Start()
    {
        SetRandomDestination();
    }

    void Update()
    {
        if (!isChasingPlayer)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
        }
    }

    void SetRandomDestination()
    {
        randomTarget = new Vector2(Random.Range(-patrolRadius, patrolRadius), Random.Range(-patrolRadius, patrolRadius));
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, randomTarget, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, randomTarget) < 0.5f)
        {
            SetRandomDestination();
        }

        Flip(randomTarget.x - transform.position.x);

    }

    public void StartChasingPlayer(Transform player)
    {
        isChasingPlayer = true;
    }

        void ChasePlayer()
    {
        // Tambahkan kode untuk mengejar player
    }

    void Flip(float direction)
    {
        if (direction > 0)
        {
            // Jika musuh bergerak ke kanan tetapi menghadap kiri
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);

        }
    }

    }

