using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootWeapon : MonoBehaviour
{
    // Audio & Animation Control
    public AudioSource audioSource;
    private AudioClip shootClip;
    private new Animation animation;

    // Bullet Shoot Control
    float nextShot = 0f;
    float cooldown = 0.3f;
    public GameObject bulletModel;

    // Ammo Control
    public int totalAmmo;
    private int actualAmmo = 0;
    public Text ammo;
    public AudioClip reload;

    void Start()
    {
        // Audio & Animation Get
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<Animation>();
        actualAmmo = totalAmmo;
        shootClip = audioSource.clip;
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
                if (actualAmmo > 0) {
                    GameObject newBullet = Instantiate(bulletModel, bulletExit.transform.position, Quaternion.LookRotation(shootDirection)); 
                    actualAmmo--;
                    ammo.text = "" + actualAmmo;

                    // Audio & Animation Player
                    audioSource.clip = shootClip;
                    audioSource.Play();
                    animation.wrapMode = WrapMode.Once;
                    animation.Play();
                }
            }
            else
            {
                Debug.LogError("No object found with the 'BulletExit' tag.");
            }
        }

        // Reload
        if (Input.GetKey(KeyCode.R)) 
        { 
            actualAmmo = totalAmmo; 
            ammo.text = "" + actualAmmo; 
            audioSource.clip = reload;
            audioSource.Play();
        }
    }
}
