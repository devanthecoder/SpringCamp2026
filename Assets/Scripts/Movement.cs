using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float currSpeed;
    public float sprintMultipler;
    public Transform cam;
    float xRotation;
    float yRotation;
    public float sensX;
    public float sensY;
    public float jumpForceAmount;
    bool isGrounded;
    public int jumpCount;
    private int currJumpCount;
    public Transform Legs;
    public LayerMask GroundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currSpeed = speed;
        currJumpCount = jumpCount;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift)) currSpeed = sprintMultipler * speed;
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        // Debug.Log(hor);
        // Debug.Log(ver);
        rb.linearVelocity = (transform.forward * ver + transform.right * hor).normalized * currSpeed + transform.up * rb.linearVelocity.y;
        LookAround();
        GroundCheck();
        Jump();
    }
    void LookAround()
    {
        float moX = Input.GetAxis("Mouse X");
        float moY = Input.GetAxis("Mouse Y");
        xRotation -= moY * sensX * Time.deltaTime;
        yRotation += moX * sensY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(Legs.position, 0.2f, GroundLayer);
        if(isGrounded && rb.linearVelocity.y <= 0.1f) currJumpCount = jumpCount;
        // Debug.Log(isGrounded);
    }
    void Jump()
    {
        // Debug.Log(currJumpCount);
        if(Input.GetKeyDown(KeyCode.Space) && currJumpCount > 0)
        {
            currJumpCount--;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForceAmount, ForceMode.Impulse);
        }
    }
}
