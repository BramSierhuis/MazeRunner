using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to put the camera at the right distance from the ground
//Taken from https://answers.unity.com/questions/314049/how-to-make-a-plane-fill-the-field-of-view.html
public class CameraHover : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private float cameraDistance = 0.2f;

    //Gets called by the maze generator after creating the ground
    public void Center()
    {
        //Get the plane to center
        GameObject plane = GameObject.FindGameObjectWithTag("Ground");
        Bounds bounds = plane.GetComponent<Renderer>().bounds;

        //Calculate the distance
        Vector3 objectSizes = bounds.max - bounds.min;
        float objectSize = Mathf.Max((objectSizes.x/2f), objectSizes.y, objectSizes.z);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * GetComponent<Camera>().fieldOfView);
        float distance = cameraDistance * objectSize / cameraView;
        distance += 0.5f * objectSize;

        //Move the camera to the proper distance
        GetComponent<Camera>().transform.position = bounds.center - distance * GetComponent<Camera>().transform.forward;
    }
}
