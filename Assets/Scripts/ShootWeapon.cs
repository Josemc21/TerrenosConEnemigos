using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    // Audio & Animation Control
    public AudioSource audioSource;
    private new Animation animation;

    // Bullet Shoot Control
    Transform bulletExit;
    float nextShot = 0f;
    float cooldown = 0.3f;
    public GameObject bulletModel;

    void Start()
    {
        // Audio & Animation Get
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<Animation>();

        // Bullet Exit Get
        bulletExit = gameObject.transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextShot && Input.GetMouseButtonDown(0))
        {
            // Bullet Creation 
            nextShot = Time.time + cooldown;
            Debug.Log("Shot fired");
            GameObject newBullet = Instantiate(bulletModel, bulletExit.position, bulletExit.rotation);

            // Audio & Animation Player
            audioSource.Play();
            animation.wrapMode = WrapMode.Once;
            animation.Play();
        }
    }
}
