using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 5f;

    public float maxVelocityChange = 10f;

    public float stamina = 100f;
    public bool isExhausted = false;

    private Rigidbody rb;
    public Transform groundCheck;
    public AudioSource walkAudio;
    public GameObject soundImg, iceUI;
    public Image stamin;

    private bool grounded = false, sprinting, jumping, crouching;
    public bool stopped = false;
    private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine("Walking");
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

            stamin.rectTransform.sizeDelta = new Vector2(stamina * 6, 40);

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
            grounded = Physics.CheckSphere(groundCheck.position, 0.1f, 8);
            if (!grounded)
            {
                walkAudio.mute = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.isPlaying && !stopped)
        {
            if (crouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
                walkAudio.mute = true;
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
                    walkAudio.mute = true;
                }
                else if (input.magnitude > 0.5f)
                {
                    rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : (crouching ? crouchSpeed : moveSpeed)), ForceMode.VelocityChange);
                    if (sprinting)
                    {
                        stamina -= Time.deltaTime * 20f;
                        walkAudio.mute = false;
                        walkAudio.pitch = 1.5f;
                    }
                    else if(!crouching)
                    {
                        walkAudio.mute = false;
                        walkAudio.pitch = 0.75f;
                    }
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
        }
        grounded = false;
        if (input.magnitude == 0 || stopped || !GameManager.isPlaying) walkAudio.mute = true;
        if(stopped) iceUI.SetActive(true);
        else iceUI.SetActive(false);
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

    IEnumerator Walking()
    {
        while (GameManager.isPlaying)
        {
            if (walkAudio.mute)
            {
                soundImg.SetActive(false);
                yield return null;
            }
            else if (walkAudio.pitch == 1.5f)
            {
                soundImg.SetActive(true);
                yield return new WaitForSeconds(0.125f);
                soundImg.SetActive(false);
                yield return new WaitForSeconds(0.125f);
            }
            else
            {
                soundImg.SetActive(true);
                yield return new WaitForSeconds(0.25f);
                soundImg.SetActive(false);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
