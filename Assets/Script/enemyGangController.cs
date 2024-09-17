using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class enemyGangController : MonoBehaviour
{
    public float speed = 2f;
    public float patrolRadius = 15f;
    private Vector2 randomTarget;
    private bool isChasingPlayer = false;
    public Transform player;
    public float detectionDistance = 5f;
    public LayerMask playerLayer;
    public GameObject senjata;
    public Transform handPosition;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float bulletSpeed = 10f;
    public float fireRate = 1f; // Tembak satu kali setiap detik
    private float nextFireTime = 0f;

    void Start()
    {
        SetRandomDestination();
    }

    void Update()
    {
        // Jika sedang mengejar pemain, musuh akan mengikuti pemain
        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        senjata.transform.position = handPosition.position;
        senjata.transform.rotation = handPosition.rotation;

        DetectPlayer();
    }

    void SetRandomDestination()
    {
        randomTarget = new Vector2(Random.Range(-patrolRadius, patrolRadius), Random.Range(-patrolRadius, patrolRadius));
    }

    void DetectPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, playerLayer);

        if (hit.collider != null && hit.collider.transform == player)
        {
            isChasingPlayer = true;

            // Jika sudah waktunya menembak, maka tembak
            if (Time.time >= nextFireTime)
            {
                Shoot();
            }
        }
        else
        {
            isChasingPlayer = false;
        }

        // Atur tampilan senjata berdasarkan apakah musuh sedang mengejar atau tidak
        senjata.SetActive(isChasingPlayer);
    }

    void ChasePlayer()
    {
        // Bergerak menuju posisi pemain
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Flip karakter agar menghadap ke arah pemain
        Flip(player.position.x - transform.position.x);
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

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletSpeed;

        Destroy(bullet, 2f);

        // Set waktu untuk tembakan berikutnya
        nextFireTime = Time.time + fireRate;
    }
}
