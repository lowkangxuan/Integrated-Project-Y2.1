/******************************************************************************
Author: Kang Xuan

Name of Class: Player

Description of Class: This class will control the movement and actions of a 
                        player avatar based on user input.

Date Created: 23/07/2021
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Player settings to adjust speed, stamina and rate of stamina regen, 
    /// as well as the length of the raycast.
    /// </summary>
    [Header("Player Settings")]
    public float moveSpeed;
    public float stamina;
    public float staminaRegen;
    [SerializeField]
    private int raycastLength;

    /// <summary>
    /// The camera attached to the player model.
    /// Should be dragged in from Inspector.
    /// Settings can be played around.
    /// </summary>
    [Header("Camera Settings")]
    public Camera playerCamera;
    public float rotationSpeed;

    private string currentState;
    private string nextState;
    private float storedRotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        storedRotationSpeed = rotationSpeed;
        nextState = "Idle";
    }

    // Update is called once per frame
    void Update()
    {
        if (nextState != currentState)
        {
            SwitchState();
        }

        CheckRotation();
        CheckSprint();
        Raycast();
    }

    /// <summary>
    /// Sets the current state of the player
    /// and starts the correct coroutine.
    /// </summary>
    private void SwitchState()
    {
        StopCoroutine(currentState);

        currentState = nextState;
        StartCoroutine(currentState);
    }

    private IEnumerator Idle()
    {
        while (currentState == "Idle")
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                nextState = "Moving";
            }
            yield return null;
        }
    }

    private IEnumerator Moving()
    {
        while (currentState == "Moving")
        {
            if (!CheckMovement())
            {
                nextState = "Idle";
            }
            yield return null;
        }
    }

    private void CheckRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        Vector3 playerRotation = transform.rotation.eulerAngles;
        playerRotation.y += mouseX;

        transform.rotation = Quaternion.Euler(playerRotation);

        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        cameraRotation.x -= mouseY;

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
    }

    /// <summary>
    /// Checks and handles movement of the player
    /// </summary>
    /// <returns>True if user input is detected and player is moved.</returns>
    private bool CheckMovement()
    {
        Vector3 newPos = transform.position;

        Vector3 xMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 zMovement = transform.forward * Input.GetAxis("Vertical");

        Vector3 movementVector = xMovement + zMovement;
        //Debug.Log(movementVector);
        //Vector3 movementVector = xMovement;

        if (movementVector.sqrMagnitude > 0)
        {
            movementVector *= moveSpeed * Time.deltaTime;
            newPos += movementVector;

            transform.position = newPos;
            return false;
        }
        else
        {
            return true;
        }

    }

    private void CheckSprint()
    {

    }

    /// <summary>
    /// Raycast for player interaction.
    /// </summary>
    private void Raycast()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * raycastLength);

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, raycastLength))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Debug.Log("hi");
            }
        }
    }
}