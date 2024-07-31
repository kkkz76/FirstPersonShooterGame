using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleWeapon : MonoBehaviour
{
    [SerializeField] private Transform gunAttachPoint;
    [SerializeField] private GameObject weaponPrefab; 
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
          
            bool PlayerHasGun = Shooter.getHasGunOnHand();

            if (PlayerHasGun)
            {
                // Log and remove the existing gun
                Debug.Log("Original Gun is deleted");
                Destroy(gunAttachPoint.GetChild(0).gameObject);
                Shooter.setHasGunOnHand(false);
            }

            // Instantiate and attach the new weapon
            GameObject weapon = Instantiate(weaponPrefab, gunAttachPoint.position, gunAttachPoint.rotation);
            weapon.transform.SetParent(gunAttachPoint);

            Shooter.setGunType(weaponPrefab.name);
            Shooter.setHasGunOnHand(true);

            Debug.Log(weaponPrefab.name + " is attached");
        }
    }


}


