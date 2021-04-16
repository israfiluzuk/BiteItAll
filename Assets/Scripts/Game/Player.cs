using System;
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
    [SerializeField] GameObject explosionParticle;
    Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraHolder = Camera.main.transform.parent;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Food"))
        {
            DestroyFood(collision.gameObject);
        }
    }

    private void DestroyFood(GameObject gameObject)
    {
        GameObject splashedObject =  Instantiate(explosionParticle, transform.position, Quaternion.identity) as GameObject;

        //Get the Particle System from the new GameObject.
        ParticleSystem PartSystem = splashedObject.GetComponent<ParticleSystem>();
        //Get the MainModule of the ParticleSystem
        ParticleSystem.MainModule ma = PartSystem.main;
        float totalDuration = PartSystem.duration + PartSystem.startLifetime;
        ma.startColor = gameObject.GetComponent<Renderer>().material.color;

        //Position the Particle System at the Balloons Location.
        splashedObject.transform.position = gameObject.transform.position;

        Destroy(gameObject);
        Destroy(PartSystem.gameObject,totalDuration);
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

    //private void LateUpdate()
    //{
    //    vec.x = cameraHolder.transform.position.x;
    //    vec.y = transform.position.y;
    //    vec.z = transform.position.z;

    //    cameraHolder.transform.position = vec;
    //}
}
