using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody sphereRB;
    public GameObject pausemenu;

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;

    private float moveInput;
    private float turnInput;
    private bool escapeKey;
    private bool isCarGrounded;

    private float normalDrag;
    public float modifiedDrag;
    
    public float alignToGroundTime;
    public float airAlignTime;
    public float gravity;
    public Vector3 fallTo;

    // Start is called before the first frame update
    void Start()
    {
        sphereRB.transform.parent = null;

        normalDrag = sphereRB.drag;
    }

    // Update is called once per frame
    void Update()
    {
        // Get inputs
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        escapeKey = Input.GetButtonDown("Cancel");

        if (escapeKey) {
            Debug.Log("Escape was pressed");
            if (Time.timeScale == 1)  {
                pausemenu.SetActive(true);
                Time.timeScale = 0;
            }
            else {
                Time.timeScale = 1;
                pausemenu.SetActive(false);
            }
        }


        float movement = (sphereRB.velocity.magnitude * (Input.GetAxisRaw("Vertical") >= 0 ? 1f : -1.5f)) / 20f; 

        // movement direction
        moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

        float newRotation = turnInput * turnSpeed * movement * Time.deltaTime;

        // Raycast
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        if(isCarGrounded) { // Rotation on ground
            transform.Rotate(0, newRotation, 0, Space.World);
            Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);
        }
        // Rotation in the air (rotate towards up direction)
        else {
            Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, fallTo) *  transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, airAlignTime * Time.deltaTime);
        }

        // Drag
        sphereRB.drag = isCarGrounded ? normalDrag : modifiedDrag;

        
    }

    // Update physics here 
    private void FixedUpdate() {
        if (isCarGrounded)
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        else
            sphereRB.AddForce(Vector3.up * -gravity); // For gravity
        // car position to the sphere
        transform.position = sphereRB.transform.position;
    }
}
