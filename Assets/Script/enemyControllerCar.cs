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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (waypoints.Length > 0)
        {
            MoveTowardsWaypoint();
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

        // Jika cukup dekat dengan waypoint, pindah ke waypoint berikutnya
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
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
}
