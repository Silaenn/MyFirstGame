
using UnityEngine;
using Cinemachine;

public class CarInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject enemyGang;
    public GameObject car;
    

    private bool isNearCar = false;
    private bool isDriving = false;
    
    private CarController carController;

    public CinemachineVirtualCamera virtualCamera;
    private EnemyHelth enemyHelth;
    private EnemyGangHelth enemyGangHelth;
    public bool isSlowingDown = false;


    void Start(){
        carController = car.GetComponent<CarController>();
        enemyHelth = enemy.GetComponent<EnemyHelth>();
        enemyGangHelth = enemyGang.GetComponent<EnemyGangHelth>();
    }

   void Update(){
     if (isNearCar && Input.GetKeyDown(KeyCode.E)){
        EnterCar();
        virtualCamera.Follow = car.transform;
        virtualCamera.LookAt = car.transform;
     } 


     if (isDriving && Input.GetKeyDown(KeyCode.Q)){
        ExitCar();
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
     }

     if(isSlowingDown){
        SlowDownCar();
     }
   }


    void EnterCar(){
        isDriving = true;
        player.SetActive(false);
        carController.isPlayerInCar = true;
        car.GetComponent<CarController>().enabled = true;
         if (transform.parent != null)
    {
        transform.parent.position = transform.position;
    }
    }

    void ExitCar(){
        isDriving = false;
        player.SetActive(true);
        carController.isPlayerInCar = false;
        car.GetComponent<CarController>().enabled = false;
        player.transform.position = car.transform.position + new Vector3(4, 0,0);
        isSlowingDown = true;
        carController.currentSpeed = 0f;
    }

   public void SlowDownCar(){
    Rigidbody2D carRb = car.GetComponent<Rigidbody2D>();
    if (carRb != null) {
        // Lakukan lerp kecepatan mobil menuju 0
        carRb.velocity = Vector2.Lerp(carRb.velocity, Vector2.zero, 0.05f); // Angka 0.05f ini bisa disesuaikan agar lebih cepat atau lambat melambat
        
        // Jika kecepatan sudah sangat kecil, hentikan mobil sepenuhnya
        if (carRb.velocity.magnitude < 0.1f) {
            carRb.velocity = Vector2.zero;
            carRb.angularVelocity = 0f;
            isSlowingDown = false;
        }
    }
 }

    
     void OnTriggerEnter2D(Collider2D other) {
     if (other.gameObject == player)   {
        isNearCar = true;
     }

     if(other.gameObject == enemy){
        enemyHelth.TakeDamage(100);
     }

      if(other.gameObject == enemyGang){
        enemyGangHelth.TakeDamage(100);
     }
    } 

     void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject == player) {
            isNearCar = false;
        }
    }   

    
}
