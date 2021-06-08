using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
    [SerializeField]
    private float playerSpeed = 2.6f;

    [Header("References")]
    [SerializeField]
    private Camera fpCam; //The camera that shows the FirstPerson view
    [SerializeField]
    private FirstPersonCamera fpCamScript; //The script that controls the fpCam
    [SerializeField]
    private GameObject lookDirection; //The triangle that shows where the player is looking
    [SerializeField]
    private GameObject particles; //Particles that spawn when a coin is picked up

    private Camera topDownCam; //The camera that shows the top down view
    private CharacterController controller;

    private Quaternion oldRotation;
    #endregion

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        topDownCam = Camera.main;

        //Cam was enabled so it could be located by Camera.main
        topDownCam.enabled = false;
    }

    private void Update()
    {
        //Change camera to the topdown cam
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Change rotation, but store old rotation
            oldRotation = transform.rotation;
            transform.rotation = Quaternion.identity;

            //Enable topdown cam
            topDownCam.enabled = true;
            fpCam.enabled = false;
            fpCamScript.SetLocked(true);

            //Show the triangle to show the players rotation
            lookDirection.SetActive(true);
            lookDirection.transform.rotation = Quaternion.Euler(90, oldRotation.eulerAngles.y, 0);

            //Let the time manager know that the topdown cam is active
            TimeManager.Instance.SetTopDownActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            //Give the player his old rotation
            if (oldRotation != null)
                transform.rotation = oldRotation;

            //Disable top down cam
            topDownCam.enabled = false;
            fpCam.enabled = true;
            fpCamScript.SetLocked(false);

            //Remove the triangle
            lookDirection.SetActive(false);

            //Let the time manager know that the topdown cam is deactivated
            TimeManager.Instance.SetTopDownActive(false);
        }

        //Move the player around based on current rotation
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * playerSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //On pickup of coin
        if(other.gameObject.tag == "Coin")
        {
            //Play sound, remove, and show particles
            SoundsPlayer.Instance.PlayCoinPickup();
            Instantiate(particles, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);

            //Tell the game manager a coin has been picked up
            GameManager.Instance.LowerCoins(1);
        }
    }
}
