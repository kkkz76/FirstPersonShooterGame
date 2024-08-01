using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Explosion : MonoBehaviour
{

    private float radius = 5.0f;
    private float power = 10.0f;
    private float boombDelayTime = 2f;
    private Vector3 explosionPos;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(explosionPos, radius);
    }

    private void Update()
    {
        explosionPos = transform.position;
    }

    private IEnumerator bombTrigger(Collision collision)
    {
        yield return new WaitForSeconds(boombDelayTime); 
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            ChangeColor target = hit.GetComponent<ChangeColor>();
            if (target != null)
            {
                target.SetRandomColour();
            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f, ForceMode.Impulse);
                
                    
            }
        }

        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
       
        StartCoroutine(bombTrigger(collision));     

    }
}
