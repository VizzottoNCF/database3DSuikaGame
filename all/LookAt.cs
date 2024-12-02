using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    private Camera c1;

    public float rotationSpeed = 2f;
    public Vector3 Offset = new Vector3 (0, 1, 4);

    private float yaw;   // Horizontal rotation (yaw angle)
    private float pitch; // Vertical rotation (pitch angle)

    void Start()
    {
        c1 = GetComponent<Camera>();

        // Initialize the yaw and pitch based on the current rotation
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        // Get input for yaw and pitch adjustments
        //yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        yaw += Input.GetAxis("Horizontal") * rotationSpeed;
        //pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -89f, 89f);  // Limit vertical rotation

        // Calculate the new rotation and position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 position = target.position + rotation * Offset;

        // Apply position and look at the target
        transform.position = position;
        transform.LookAt(target.position);
    }
}
