using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Explosion : MonoBehaviour
{
    private float radius = 3.0f;
    private float power = 7.0f;
    private float boombDelayTime = 2.2f;
    public GameObject particleSysPrefab;
    [SerializeField] GameObject bombSound;
    private Vector3 explosionPos;

    private void Start()
    {
           
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Update()
    {
        explosionPos = transform.position;
    }

    private IEnumerator bombTrigger(Collision collision)
    {
        yield return new WaitForSeconds(boombDelayTime);
        
        GameObject bs = Instantiate(bombSound, explosionPos, Quaternion.identity);
        GameObject ps = Instantiate(particleSysPrefab, explosionPos, Quaternion.identity);
        Destroy(ps, 2f);
        Destroy(bs, 2f);

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

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(bombTrigger(collision));
       
    }
}
