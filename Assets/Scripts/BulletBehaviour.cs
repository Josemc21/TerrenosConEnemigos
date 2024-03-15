using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float bulletSpeed = 100.0f;
    private float bulletDMG = 1.0f;

    void FixedUpdate()
    {
        float movement = Time.deltaTime * bulletSpeed;
        transform.Translate(Vector3.forward * movement);

        RaycastHit hit;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(transform.position, direction, out hit, movement))
        {
            if (hit.collider.gameObject.tag == "EnemyHead")
            {
                HitEnemy(hit.collider.gameObject, true); // Se pasa true para indicar que es un golpe en la cabeza
            }
            else if (hit.collider.gameObject.tag == "EnemyBody")
            {
                HitEnemy(hit.collider.gameObject, false); // Se pasa false para indicar que es un golpe en el cuerpo
            }
        }
    }

    void HitEnemy(GameObject hitObject, bool isHeadShot)
    {
        GameObject enemy = hitObject.transform.root.gameObject; //Get the root of the object that was hit;

        float damageMultiplier = isHeadShot ? 1.5f : 1f;
        float damage = bulletDMG * damageMultiplier;

        enemy.SendMessage("Hit", damage, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}