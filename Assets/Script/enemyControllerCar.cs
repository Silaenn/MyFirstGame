using UnityEngine;

public class enemyControllerCar : MonoBehaviour
{
    private float acceleration = 10f;
    private float turnSpeed = 100f;
    private float currentSpeed = 0f;
    private float maxSpeed = 15f;
    private float driftFactor = 0.95f;
    private float rotationAngle = 0f;
    private Rigidbody2D rb;

    public Transform[] waypoints; // Daftar titik yang harus dilalui musuh
    private int currentWaypointIndex = 0;

     public Transform player;
    public float detectionDistance = 5f;
    public LayerMask playerLayer;
    bool isMovingForward = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (waypoints.Length > 0)
        {
            MoveTowardsWaypoint();
            DetectPlayer();
        }
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 directionToWaypoint = (targetWaypoint.position - transform.position).normalized;

        // Hitung sudut rotasi yang diperlukan untuk menghadap waypoint
        float targetRotationAngle = Mathf.Atan2(directionToWaypoint.y, directionToWaypoint.x) * Mathf.Rad2Deg;
        
        // Update rotasi mobil menuju target
        rotationAngle = Mathf.LerpAngle(rotationAngle, targetRotationAngle, Time.deltaTime * turnSpeed);

        // Update kecepatan mobil menuju target
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        ApplyMovement();

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 1f)
    {
        // Jika bergerak maju dan sudah di waypoint terakhir, ubah arah menjadi mundur
        if (isMovingForward && currentWaypointIndex >= waypoints.Length - 1)
        {
            isMovingForward = false;
        }
        // Jika bergerak mundur dan sudah di waypoint pertama, ubah arah menjadi maju
        else if (!isMovingForward && currentWaypointIndex <= 0)
        {
            isMovingForward = true;
        }

        // Ubah currentWaypointIndex sesuai arah
        if (isMovingForward)
        {
            currentWaypointIndex++;
        }
        else
        {
            currentWaypointIndex--;
        }
    }
    }

    void ApplyMovement()
    {
        // Rotasi mobil berdasarkan input rotasi
        rb.MoveRotation(rotationAngle);

        // Hitung arah maju menggunakan Quaternion berdasarkan rotasi yang ada
        Vector2 forward = new Vector2(Mathf.Cos(rotationAngle * Mathf.Deg2Rad), Mathf.Sin(rotationAngle * Mathf.Deg2Rad));

        // Kalikan arah maju dengan kecepatan saat ini
        Vector2 movement = forward * currentSpeed;

        // Terapkan efek drift untuk membuat pergerakan lebih halus
        Vector2 drift = Vector2.Lerp(rb.velocity, movement, driftFactor);

        // Terapkan kecepatan akhir ke Rigidbody2D
        rb.velocity = drift;
    }

     void DetectPlayer()
{
    // Arah ke player
    Vector2 directionToPlayer = (player.position - transform.position).normalized;
    
    // Arah depan mobil (bisa juga pakai transform.up tergantung orientasi mobil)
    Vector2 forwardDirection = transform.right;

    // Sudut antara arah depan mobil dan arah ke player
    float angle = Vector2.Angle(forwardDirection, directionToPlayer);

    // Hanya lakukan raycast jika player berada dalam sudut pandang (misalnya 30 derajat)
    if (angle < 30f)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionDistance, playerLayer);
        
        if (hit.collider != null && hit.collider.transform == player)
        {
            // Hentikan mobil jika mendeteksi player
            rb.velocity = Vector2.zero;
            rb.freezeRotation = true;
        }
    }
}

}
