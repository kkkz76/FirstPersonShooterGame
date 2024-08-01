using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Camera cam;                   // for camera
    public GameObject particleSysPrefab;  // for bullet effect
    public GameObject grenadePrefab;      // for grenade prefab
    private bool isFiring;                // check for machine gun firing
    private static bool hasGunOnHand;     // check for gun on hand  
    private static string gunType;        // check gun type
    public float impulseStrength = 5.0f;  // for bullet impulse
    private int bulletDamage = 1;

    // For machine gun
    private float rateOfFire = 0.2f;
    private float startDelay = 0.5f;

    // For shotgun
    private const int shotgunPelletCount = 4;
    private const float shotgunSpreadAngle = 0.1f ;

    // For handgun and machine gun
    private const int handgunPellets = 1;
    private const float HandgunSpreadAngle = 0.02f;


    //For machine gun
    private const int machinegunPellets = 1;
    private const float machinegunSpreadAngle = 0.05f;

    //For Grenade
    private float minGrenadeImpulse = 3.0f;
    private float maxGrenadeImpulse = 8.0f;
    private float guageFillrate = 4.0f;
    private float currentImpulse = 0.0f;


    void Start()
    {
        // Gets the GameObject's camera component
        cam = GetComponent<Camera>();

        // Hides the mouse cursor at the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        int size = 12;

        // Center of screen and caters for font size
        float posX = cam.pixelWidth / 2 - size / 4;
        float posY = cam.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

   

    //----------------------------------------------------------------------------------------------

    // Common Shoot Function and particle system
    private void Shoot(int pellets, float spread)
    {
        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        // noraml ray pointing to center of screen
        Ray baseRay = cam.ScreenPointToRay(point);

        for (int i = 0; i < pellets; i++)
        {
            Vector3 randomSpread = Random.insideUnitCircle * spread;
            Vector3 direction = baseRay.direction + cam.transform.TransformDirection(randomSpread);

            //  a new ray for each pellet with differenct spread angle.
            Ray ray = new Ray(baseRay.origin, direction);
            

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1.0f);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                Shootable target = hitObject.GetComponent<Shootable>();

                if (target != null)
                {
                    Vector3 impulse = Vector3.Normalize(hit.point - transform.position) * impulseStrength;
                    hit.rigidbody?.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);
                    target.SetHealth(bulletDamage);
                    StartCoroutine(GeneratePS(hit));
                }

            }
           

        }
    }

    private IEnumerator GeneratePS(RaycastHit hit)
    {
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1);
        Destroy(ps);
    }

    //----------------------------------------------------------------------------------------------


    //----------------------------------------------------------------------------------------------

    // For HandGun  
    private void HandleHandgunShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(handgunPellets, HandgunSpreadAngle);
        }
    }

    //----------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------

    //For Machine Gun
    private void HandleMachineGunShoot()
    {
        if (Input.GetMouseButtonDown(0) && !isFiring)
        {
            InvokeRepeating(nameof(ShootMachineGun), startDelay, rateOfFire);
            isFiring = true;
        }

        if (Input.GetMouseButtonUp(0) && isFiring)
        {
            CancelInvoke(nameof(ShootMachineGun));
            isFiring = false;
        }
    }

    // For MachineGun invoke function
    private void ShootMachineGun()
    {
        Shoot(machinegunPellets, machinegunSpreadAngle);
    }
    //----------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------

    // For ShotGun
    private void HandleShotgunShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(shotgunPelletCount, shotgunSpreadAngle);
        }
    }

    //----------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------

    //For Grenade
    private void HandleGrenadeThrow()
    {
       
        if (Input.GetMouseButton(1))
        {
            if (currentImpulse < maxGrenadeImpulse)
            {
                currentImpulse += guageFillrate * Time.deltaTime;
                // Clamp the impulse to the max value
                currentImpulse = Mathf.Clamp(currentImpulse, minGrenadeImpulse, maxGrenadeImpulse);
                Debug.Log(currentImpulse);
            }
           
        }
        else if (Input.GetMouseButtonUp(1))

        {
            
            Vector3 grenadePosition = cam.transform.position + cam.transform.forward * 2;
            GameObject grenade = Instantiate(grenadePrefab, grenadePosition, Quaternion.identity);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            Vector3 impulse = cam.transform.forward * currentImpulse;
            rb.AddForceAtPosition(impulse, cam.transform.position, ForceMode.Impulse);
            currentImpulse = 0.0f;
        }


    }

    //----------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------

    // Weapon type check

    public static void setHasGunOnHand(bool hasGunOnHand)
    {
        Shooter.hasGunOnHand = hasGunOnHand;
    }

    public static bool getHasGunOnHand ()
    {
        return hasGunOnHand;
    }

    public static void setGunType(string gunType)
    {
        Shooter.gunType = gunType;
    }

    

    private void chechWeaponType()
    {
        if(hasGunOnHand)
        {
            if(gunType == "Handgun")
            {
                HandleHandgunShoot();
            }
            else if(gunType == "Machinegun" || gunType == "Rifle")
            {
                HandleMachineGunShoot();
            }
            else if(gunType == "Shotgun")
            {
                HandleShotgunShoot();
            }
      
        }
    }

    void Update()
    {
        chechWeaponType();
        HandleGrenadeThrow();
       
    }
}
