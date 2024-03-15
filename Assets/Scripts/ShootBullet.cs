using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    // Bullet Variables
    public GameObject bulletModel;                  // Bullet PreFab 
    public Transform bulletExit;                    // Bullet Exit
    public float fireRate = 0.5f;                   // Time between bullets
    float nextFire = 0.0f;                          // Time Checker

    AudioManagement audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagement>();
    }

    // Fire Bullet Control
    public void Shoot()
    {
        if (Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bala = Instantiate(bulletModel,bulletExit.position, bulletExit.rotation);
            bala.GetComponent<Rigidbody>().AddForce(bulletExit.forward * 1000f);
            audioManager.PlaySFX(audioManager.gun);
        }
    }

    
}
