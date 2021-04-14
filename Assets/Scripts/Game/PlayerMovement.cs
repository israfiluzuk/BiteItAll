using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    bool isJump = false;
    Transform cameraHolder;

    [SerializeField] float playerSpeed;
    [SerializeField] float playerForce;
    Vector3 vec;

    [SerializeField] GameObject obstacle;
    [SerializeField] float obstacleDistance;
    [SerializeField] float obstaclePosY;
    [SerializeField] int numberOfObstacle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraHolder = Camera.main.transform.parent;

        CreateObstacle();
    }

    private void CreateObstacle()
    {
        vec.z = 0;
        for (int i = 0; i < numberOfObstacle; i++)
        {
            vec.x += obstacleDistance;
            vec.y = UnityEngine.Random.Range(-obstaclePosY, obstaclePosY);

            Instantiate(obstacle, vec, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.right * playerSpeed * Time.fixedDeltaTime);

        if (isJump)
        {
            rb.AddForce(Vector3.up * playerForce * 1000f * Time.fixedDeltaTime);
            isJump = false;
        }
    }

    private void LateUpdate()
    {
        vec.x = transform.position.x;
        vec.y = transform.position.y;
        vec.z = cameraHolder.transform.position.z;

        cameraHolder.transform.position = vec;
    }
}
