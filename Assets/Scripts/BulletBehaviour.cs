using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float bulletSpeed = 100.0f;
    private float bulletDMG = 1.0f;

    public float rotX;
    public float rotY;
    public float rotZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Time.deltaTime * bulletSpeed;
        transform.Translate(Vector3.forward * movement);
        rotX = gameObject.transform.rotation.x;
        rotY = gameObject.transform.rotation.y;
        rotZ = gameObject.transform.rotation.z;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy hit");
            collider.SendMessage("Hit", bulletDMG, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
