using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    bool isJump = false;
    Transform cameraHolder;

    [SerializeField] float playerSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] SkinnedMeshRenderer manHead;
    Vector3 vec;

    Quaternion downRotation;
    Quaternion upRotation;
    bool isPlayerDown = false;
    Vector3 lastPosition;
    Vector3 currentPosition;
    bool isFoodEaten = false;
    private int playerScore = 0;
    [SerializeField] private int multiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraHolder = Camera.main.transform.parent;
        downRotation = Quaternion.Euler(-45, 20, 0);
        upRotation = Quaternion.Euler(45, 20, 0);
        MoveBSValueTo("A", 0, .18f);
        MoveBSValueTo("M", 100, .18f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Food"))
        {
            isFoodEaten = true;
            DestroyFood(col.gameObject);
            playerScore++;
            GameManager.Instance.UpdadeScore(playerScore, multiplier);
            StartCoroutine(MoveMouth());
        }
        if (col.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.Restart();
        }
        if (col.gameObject.CompareTag("FinishCube"))
        {
            //Time.timeScale = 0;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            print(col.gameObject.name);
        }

    }

    IEnumerator MoveMouth()
    {
        MoveBSValueTo("A", 58, .05f);
        MoveBSValueTo("M", 0, .05f);
        yield return new WaitForSeconds(.2f);
        MoveBSValueTo("A", 0, .05f);
        MoveBSValueTo("M", 100, .05f);

    }

    private void DestroyFood(GameObject gameObject)
    {
        GameObject splashedObject = Instantiate(explosionParticle, transform.position, Quaternion.identity) as GameObject;
        splashedObject.transform.Rotate(-45, 90, 0);
        //Get the Particle System from the new GameObject.
        ParticleSystem PartSystem = splashedObject.GetComponent<ParticleSystem>();
        //Get the MainModule of the ParticleSystem
        ParticleSystem.MainModule ma = PartSystem.main;
        float totalDuration = PartSystem.duration + PartSystem.startLifetime;
        //ma.startColor = Food.Instance.FoodColor;
        ma.startColor = gameObject.GetComponent<Renderer>().material.color;

        //Position the Particle System at the Balloons Location.
        splashedObject.transform.position = gameObject.transform.position;

        Destroy(gameObject);
        Destroy(PartSystem.gameObject, totalDuration);
    }
    private void Update()
    {
        currentPosition = transform.position;

        if (Input.GetKeyUp(KeyCode.Space) || Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                isJump = true;
        }
        if (lastPosition.y > currentPosition.y)
        {
            isPlayerDown = true;
            transform.DORotate(new Vector3(20, 20, 0), .4f);
        }
    }

    private void LateUpdate()
    {
        lastPosition = currentPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * playerSpeed * Time.fixedDeltaTime);
        if (isJump)
        {
            rb.velocity = Vector3.zero;
            DOTween.Kill(transform);
            transform.DORotate(new Vector3(-20, 20, 0), .4f);
            rb.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            isJump = false;
        }
        //transform.DORotate(new Vector3(30, 0, 0), .2f);
        //rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    //private void LateUpdate()
    //{
    //    vec.x = cameraHolder.transform.position.x;
    //    vec.y = transform.position.y;
    //    vec.z = transform.position.z;

    //    cameraHolder.transform.position = vec;
    //}

    public void MoveBSValueTo(string bsName, float value, float duration)
    {
        DOTween.To(() => manHead.GetBlendShapeWeight(GetBSIndexByName(manHead, bsName)), x => manHead.SetBlendShapeWeight(GetBSIndexByName(manHead, bsName), x), value, duration);

    }

    public static int GetBSIndexByName(SkinnedMeshRenderer faceMeshRenderer, string bsName)
    {
        Mesh m = faceMeshRenderer.sharedMesh;
        for (int i = 0; i < m.blendShapeCount; i++)
        {
            if (m.GetBlendShapeName(i).Equals(bsName))
            {
                return i;
            }
        }
        return -1;
    }
}
