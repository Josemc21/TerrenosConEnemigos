using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]             // Creates this components if missing when compiling
[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : HealthyEntity
{
    // Player Variables
    CharacterController characterController;                // Character Movement
    Rigidbody rb;                                 
    ShootBullet bulletControler;                            // Bullet Exit Object
    public delegate void OnPlayerDeath();                   // Subscription event for Player Death
    public static event OnPlayerDeath PlayerDeath;

    // Player Movement Control
    Vector3 moveInput, moveVelocity;
    public float speed = 1.0f;

    // Camera Control
    private Vector3 offset;
    public Camera mainCamera;

    // Overridden Start function 
    protected override void Start()
    {
        base.Start();                                                   // HealthyEntity Start function execute
        characterController = GetComponent<CharacterController>();      
        rb = GetComponent<Rigidbody>();
        bulletControler = GetComponent<ShootBullet>();

        // Camera Control
        offset = mainCamera.transform.position;
    }

    void Update()
    {
        // Player Vision Direction
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);
            transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
        }

        // Shoot Bullet on LeftClick
        if (Input.GetMouseButton(0)) { bulletControler.Shoot(); }

        // Camera Fixed Movement
        mainCamera.transform.position = transform.position + offset;
    }

    void FixedUpdate()
    {
        // Player Movement Control
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        characterController.Move(moveVelocity * speed * Time.deltaTime);
    }
    
    // Player Death Notification
    void OnDestroy() { if (PlayerDeath != null) { PlayerDeath(); } }
}
