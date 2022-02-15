using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCarController : MonoBehaviour
{
    public Rigidbody sphereRB;
    public Rigidbody carRB;

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;

    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;

    private float normalDrag;
    public float modifiedDrag;
    
    public float alignToGroundTime;

    // Start is called before the first frame update
    void Start()
    {
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;

        normalDrag = sphereRB.drag;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        float isMoving = (sphereRB.velocity.magnitude * (Input.GetAxisRaw("Vertical") >= 0 ? 1f : -1.5f)) / 20f; 

        // movement direction
        moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

        float newRotation = turnInput * turnSpeed * Time.deltaTime * isMoving;

        if(isCarGrounded)
            transform.Rotate(0, newRotation, 0, Space.World);

        

        // Raycast
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        // Rotation
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);

        // transform.Rotate(0, newRotation, 0, Space.World);

        // Drag
        sphereRB.drag = isCarGrounded ? normalDrag : modifiedDrag;
    }

    private void FixedUpdate() {
        if (isCarGrounded)
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        else
            sphereRB.AddForce(Vector3.up * -40f); // For gravity

        carRB.MoveRotation(transform.rotation);

        // car position to the sphere
        transform.position = sphereRB.transform.position;
    }
}
