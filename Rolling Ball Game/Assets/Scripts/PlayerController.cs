using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Diagnostics;
using System.Runtime.Hosting;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Material playerColor;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private float movementZ;
    private bool onGround;
    private float jumpTime;
    private float winTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerColor = GetComponent<Renderer>().material;
        count = 0;
        movementZ = 0.0f;

        SetCountText();
        winTextObject.SetActive(false);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnJump()
    {
        if (onGround)
        {
            onGround = false;
            movementZ = 2;
            jumpTime = .5f;
        }
    }

    private void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if(jumpTime > 0)
        {
            jumpTime -= Time.deltaTime;
        }
        else
        {
            movementZ = -2;
        }

        if (winTextObject.activeSelf)
        {
            winTimer += Time.deltaTime;

            if (winTimer >= 1f)
            {
                UnityEngine.Debug.Log("Reached WinTimer");
                Application.Quit();
            }
        }

        Vector3 movement = new Vector3(movementX, movementZ, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++; // increase score
            SetCountText();

            playerColor.color = other.gameObject.GetComponent<Renderer>().material.color;
        }
    }
}
