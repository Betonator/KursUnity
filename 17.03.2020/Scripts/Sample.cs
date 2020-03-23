using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    public Rigidbody body;
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public bool jumped = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(horizontal, 0.0f, vertical).normalized * speed;
        body.velocity = new Vector3(moveVector.x, body.velocity.y, moveVector.z);

        if (Input.GetKeyDown(KeyCode.Space) && !jumped)
        {
            jumped = true;
            body.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            jumped = false;
        }
    }
}
