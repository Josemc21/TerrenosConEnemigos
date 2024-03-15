using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBehaviour : MonoBehaviour
{
    // Bullet Variables
    public float speed = 10.0f;                 // Bullet Base Speed
    float damage = 1;                           // Bullet Base Damage
    public LayerMask capasDestruir;             // Collision Layer

    void Update()
    {
        // Bullet Movement
        float moveDistance = Time.deltaTime * speed;
        transform.Translate(Vector3.forward * moveDistance);

        // Collision Control
        CheckCollision(moveDistance);
    }

    void CheckCollision(float moveDistance)
    {
        // Collision Control
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, capasDestruir, QueryTriggerInteraction.Collide))
        {
            // Enemy Hit Control
            if(hit.collider.tag == "Enemy")
            {
                IHealthControl healthyObject = hit.collider.GetComponent<IHealthControl>();
                if (healthyObject != null)
                {
                    healthyObject.TakeHit(damage, hit);
                }
            }
            Destroy(gameObject);
        }
        
        // Bullet destruction if no collision
        StartCoroutine(DelayedBulletDestroy());
    }

    // Auxiliar Destruction Function
    IEnumerator DelayedBulletDestroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
