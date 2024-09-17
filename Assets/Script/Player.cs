using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 10f;
    private Rigidbody2D rb;

    public bool isInCar = false;
    private Animator anim;

    public GameObject senjata;
    public GameObject bulletPrefab;
    private bool isWeaponEquipped = false;
    public Transform handPosition;
    public Animator playerAnimator;
    public Transform firePoint; 
    
    private float bulletSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
       if (!isInCar){
       MovePlayer();
       }

       if(Input.GetKeyDown(KeyCode.Alpha1)){
        ToggleWeapon();
       }

       if(Input.GetKeyDown(KeyCode.Space) && isWeaponEquipped){
            Shoot();
        }

         if (isWeaponEquipped)
        {
            senjata.transform.position = handPosition.position;
            senjata.transform.rotation = handPosition.rotation;
        }
    }


    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = firePoint.right * bulletSpeed;

        Destroy(bullet, 2f); 
    }

    void MovePlayer(){
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        bool isRunning = moveX != 0 || moveY != 0;
        anim.SetBool("run", isRunning);
        

     if (moveX > 0.01f)
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0); // Normal orientation
    }
    else if (moveX < -0.01f)
    {
        transform.localRotation = Quaternion.Euler(0, 180, 0); // Flip horizontally
    }

        rb.velocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
    }

    void ToggleWeapon(){
        isWeaponEquipped = !isWeaponEquipped;
        senjata.SetActive(isWeaponEquipped);
    }

}
