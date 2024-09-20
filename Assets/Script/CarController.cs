using UnityEngine;

public class CarController : MonoBehaviour
{
    private float acceleration = 10f;
    private float turnSpeed = 100f;
    private float deceleration = 5f;
    public float currentSpeed = 0f;

    private float steering = 1.5f;
    private float rotationAngle = 0f;
    public bool isPlayerInCar = false;
     private float maxSpeed = 20f;
     private float driftFactor = 0.95f;
    private Rigidbody2D rb;

     private AudioSource honkSource;
    private AudioSource engineSource;
    public AudioClip honkClip;
    public AudioClip engineClip;



    void Start(){
        rb = GetComponent<Rigidbody2D>();
       AudioSource[] audioSources = GetComponents<AudioSource>();
       honkSource = audioSources[0];
       engineSource = audioSources[1];
       engineSource.loop = true;


    }
    void Update()
    {
      if (isPlayerInCar){
        MoveCar();

        
       if (!engineSource.isPlaying && currentSpeed != 0)
        {
            engineSource.Play(); 
        }
        // Hentikan suara mesin jika kecepatan mobil 0
        else if (engineSource.isPlaying && currentSpeed == 0)
        {
            engineSource.Stop(); 
        }
      } else{
            engineSource.Stop(); 

      }


      if(isPlayerInCar && Input.GetKeyDown(KeyCode.H)){
        PlayHonk();
      }

    }

    void MoveCar(){
       float verticalInput = Input.GetAxis("Vertical");
       float horizontalInput = Input.GetAxis("Horizontal");

      if (verticalInput > 0){
        currentSpeed += acceleration * Time.fixedDeltaTime;
      } else if(verticalInput < 0){
        currentSpeed -= acceleration * Time.fixedDeltaTime;
      } else {
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        PlayEngine();

      }

      currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

       if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float direction = Mathf.Sign(currentSpeed);
            rotationAngle -= horizontalInput * steering * turnSpeed * direction * Time.fixedDeltaTime;
        }

        ApplyMovement();
    }

    void ApplyMovement(){
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

     void PlayHonk()
    {
        honkSource.Stop(); // Hentikan suara jika sedang berjalan
        honkSource.PlayOneShot(honkClip); 
    }
     void PlayEngine()
    {
        engineSource.Stop(); // Hentikan suara jika sedang berjalan
        engineSource.PlayOneShot(engineClip); 
    }





}
