using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public bool IsHeld = true;
    private float throwForce = 5f;
    public KeyCode throwKey = KeyCode.Space;

    private float rotationSpeed = 3f;

    private float fadeTrajectory = 1f;
    public int trajectoryPoints = 80;
    public float timeStep = 0.01f;
    public bool lineRenderFade = false;

    private Transform target;

    private Rigidbody rb;
    private LineRenderer lineRenderer;
    void Start()
    {
        target = GameObject.FindWithTag("Finish").GetComponent<Transform>();


        rb = GetComponent<Rigidbody>();

        // Ensure there is a Rigidbody component
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found");
        }

        // Add a LineRenderer component for trajectory visualization
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = trajectoryPoints;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.red;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (IsHeld)
        {
            Vector3 direction = target.position - transform.position;
            //direction.x = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            float verticalInput = Input.GetAxis("Vertical");

            if (verticalInput < 0)
            {
                // Rotate clockwise (positive rotation in x-axis)
                transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            }
            else if (verticalInput > 0)
            {
                // Rotate counterclockwise (negative rotation in x-axis)
                transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
            }

            rf_DisplayTrajectory();
            if (Input.GetKey(throwKey))
            {
                throwForce += Time.deltaTime;
                if (throwForce >= 15f)
                {
                throwForce = 5f;
                }
            }

            if (Input.GetKeyUp(throwKey))
            {
                IsHeld = false;
                rf_ThrowForward();
            }
        }
        else
        {
            rb.isKinematic = false;
            if (!lineRenderFade)
            {
                if (fadeTrajectory > 0)
                {
                    fadeTrajectory -= Time.deltaTime;

                }
                else if (fadeTrajectory <= 0)
                {
                    if (GetComponent<LineRenderer>() != null)
                    {
                        Destroy(GetComponent<LineRenderer>());
                        lineRenderFade = true;
                    }
                }
            }
            
        }
    }

    void rf_ThrowForward()
    {
        // disables kinematic so that it can be thrown
        rb.isKinematic = false;

        // Clear any previous velocity
        rb.velocity = Vector3.zero;

        // Apply force in the forward direction of the object
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }
    void rf_DisplayTrajectory()
    {
        Vector3 startingPosition = transform.position;
        Vector3 startingVelocity = transform.forward*throwForce;
        Vector3 currentPosition = startingPosition;
        Vector3 currentVelocity = startingVelocity;

        // draw trajectory
        for (int i = 0; i < trajectoryPoints; i++)
        {
            lineRenderer.positionCount = trajectoryPoints;
            lineRenderer.SetPosition(i, currentPosition);

            // apply physics to next point
            currentVelocity += Physics.gravity * timeStep;
            currentPosition += currentVelocity * timeStep;
        }
    }
}
