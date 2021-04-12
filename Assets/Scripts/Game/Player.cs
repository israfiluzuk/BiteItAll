using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    bool isJump = false;
    Transform cameraHolder;

    [SerializeField] float playerSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraHolder = Camera.main.transform.parent;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJump = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * playerSpeed * Time.fixedDeltaTime);
        if (isJump)
        {
            rb.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            isJump = false;
        }

        //rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {
        vec.x = cameraHolder.transform.position.x;
        vec.y = cameraHolder.transform.position.y;
        vec.z = transform.position.z;

        cameraHolder.transform.position = vec;
    }
}
