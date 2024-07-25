using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 5f;

    public float maxVelocityChange = 10f;
    private float airControl = 1f;

    public float stamina = 100f;
    public bool isExhausted = false;

    private Rigidbody rb;
    private bool grounded = false, sprinting, jumping, crouching;
    private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPlaying)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input.Normalize();
            if (!sprinting) stamina += Time.deltaTime * 11f;
            if (stamina <= 10) isExhausted = true;
            if (stamina >= 30) isExhausted = false;
            if (stamina < 0) stamina = 0;
            if (stamina > 100) stamina = 100;

            if (Input.GetButtonDown("Crouch"))
            {
                crouching = !crouching;
            }
            sprinting = Input.GetButton("Sprint") && !isExhausted;
            jumping = Input.GetButton("Jump");
            if(Input.GetButtonDown("Sprint") || Input.GetButtonDown("Jump"))
            {
                crouching = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    private void FixedUpdate()
    {
        if (GameManager.isPlaying)
        {
            if (crouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
            }

            if (grounded)
            {
                if (jumping)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
                }
                else if (input.magnitude > 0.5f)
                {
                    rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : (crouching ? crouchSpeed : moveSpeed)), ForceMode.VelocityChange);
                    if (sprinting) stamina -= Time.deltaTime * 20f;
                }
                else
                {
                    var velocity1 = rb.velocity;
                    velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                    rb.velocity = velocity1;
                }
            }
            else
            {
                if (input.magnitude > 0.5f)
                {
                    rb.AddForce(CalculateMovement(sprinting ? sprintSpeed * airControl : (crouching ? crouchSpeed * airControl : moveSpeed * airControl)), ForceMode.VelocityChange);
                    if (sprinting) stamina -= Time.deltaTime * 20f;
                }
                else
                {
                    var velocity1 = rb.velocity;
                    velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                    rb.velocity = velocity1;
                }
            }
            grounded = false;
        }
    }
    
    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _speed;
        Vector3 velocity = rb.velocity;
        if(input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            return velocityChange;
        }
        else
        {
            return new Vector3();
        }
    }
}
