using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    // Audio & Animation Control
    public AudioSource audioSource;
    private new Animation animation;

    // Bullet Shoot Control
    float nextShot = 0f;
    float cooldown = 0.3f;
    public GameObject bulletModel;

    void Start()
    {
        // Audio & Animation Get
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= nextShot && Input.GetMouseButtonDown(0))
        {
            // Find the object with the "BulletExit" tag
            GameObject bulletExit = GameObject.FindWithTag("BulletExit");

            if (bulletExit != null)
            {
                // Get the direction the player is looking
                Vector3 shootDirection = Camera.main.transform.forward;

                // Bullet Creation
                nextShot = Time.time + cooldown;
                GameObject newBullet = Instantiate(bulletModel, bulletExit.transform.position, Quaternion.LookRotation(shootDirection));

                // Audio & Animation Player
                audioSource.Play();
                animation.wrapMode = WrapMode.Once;
                animation.Play();
            }
            else
            {
                Debug.LogError("No object found with the 'BulletExit' tag.");
            }
        }
    }
}
