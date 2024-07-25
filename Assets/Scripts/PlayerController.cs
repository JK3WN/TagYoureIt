using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;

    private float stamina = 100;
    private bool isExhausted = false;

    public Rigidbody2D rb;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagement.isPlaying)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !isExhausted && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * sprintSpeed, Input.GetAxis("Vertical") * sprintSpeed);
                stamina -= Time.deltaTime * 20f;
            }
            else
            {
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
                stamina += Time.deltaTime * 11f;
            }
            if (stamina <= 10) isExhausted = true;
            if (stamina >= 30) isExhausted = false;
            if (stamina < 0) stamina = 0;
            if (stamina > 100) stamina = 100;

            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            direction.z = 0f;
            if(direction.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
}
