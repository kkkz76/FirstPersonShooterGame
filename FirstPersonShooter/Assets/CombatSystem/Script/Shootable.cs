using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Shootable : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    [SerializeField] int health = 10;
    public void SetHealth(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Debug.Log("Dead");
            if(rb != null)
            {
                rb.isKinematic = true;
            }
           
            transform.position = startPosition;
            transform.rotation = startRotation;
            health = 10;
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        if(rb.isKinematic == true)
        {
            rb = null;
        }
        
    }

    
}
