using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to rotate the player using mouse inputs
//Code from https://www.youtube.com/watch?v=_QajrabyTJc
public class FirstPersonCamera : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private float mouseSensitivity = 150f;

    [Header("References")]
    [SerializeField]
    private Transform playerBody;

    private float xRotation = 0f;
    private bool locked = false; //used to disable mouse movements when the topdown view is active

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            //Get mouse inputs
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            //Rotate the player
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    //Setter for the locked variable
    public void SetLocked(bool locked)
    {
        this.locked = locked;
    }
}
