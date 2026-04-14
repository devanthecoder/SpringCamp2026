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
    public float scaleFactor = 0.4f;

    public float crouchFactor = 0.5f;
    public bool isCrouching, isSliding, isWallrunning;
    float origScale;
    public float slideForce = 50f;
    public float slideTime = 2f;
    float slideTimer;
    Vector3 inputDir;
    public bool freeze = false;
    void Start()
    {
        slideTimer = slideTime;
        isCrouching = false;
        isSliding = false;
        origScale = transform.localScale.y;
        currSpeed = speed;
        currJumpCount = jumpCount;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale = 0.5f;
        } else if (Input.GetKeyUp(KeyCode.X))
        {
            Time.timeScale = 1f;
        }
        if(Input.GetKeyDown(KeyCode.R))
            GameManager.instance.RestartGame();
        if (freeze)
        {
            rb.linearVelocity = Vector3.zero;
            currSpeed = 0f;
        }
        // Debug.Log(slideTimer);
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        inputDir = transform.forward * ver + transform.right * hor;
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            rb.AddForce(inputDir.normalized * slideForce, ForceMode.Force);
            currSpeed = 0f;
        }
        else if(isCrouching) currSpeed = crouchFactor * speed;
        else if(isWallrunning) currSpeed = 0f;
        else currSpeed = speed;
        Debug.Log(currSpeed);
        // Debug.Log(hor);
        // Debug.Log(ver);
        rb.linearVelocity = inputDir.normalized * currSpeed + transform.up * rb.linearVelocity.y;
        LookAround();
        GroundCheck();
        Jump();
        if(inputDir.magnitude < 0.1f)
        {
            Crouch();
        }
        else
        {
            StartSlide();
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isCrouching = false;
            isSliding = false;
            transform.localScale = new Vector3(transform.localScale.x, origScale, transform.localScale.z);
        }
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
        if(Input.GetKeyDown(KeyCode.Space) && currJumpCount > 0 && !isWallrunning)
        {
            currJumpCount--;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForceAmount, ForceMode.Impulse);
        }
        else if(Input.GetKeyDown(KeyCode.Space) && isWallrunning)
        {
            Wallrunning wr = GetComponent<Wallrunning>();
            wr.WallJump();
        }
    }
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded && !isWallrunning)
        {
            isCrouching = true;
            transform.localScale = new Vector3(transform.localScale.x, scaleFactor*origScale, transform.localScale.z);
            rb.AddForce(5f * Vector3.down, ForceMode.Impulse);
        }
    }
    void StartSlide()
    {
        if (Input.GetKeyDown(KeyCode.Z) && slideTimer > 0f && isGrounded && !isWallrunning)
        {
            isSliding = true;
            transform.localScale = new Vector3(transform.localScale.x, scaleFactor*origScale, transform.localScale.z);
            rb.AddForce(5f * Vector3.down, ForceMode.Impulse);
        }
        if(slideTimer < 0f)
        {
            StopSlide();
        }
        
    }
    void StopSlide()
    {
        isSliding = false;
        transform.localScale = new Vector3(transform.localScale.x, origScale, transform.localScale.z);
        slideTimer = slideTime;
    }
}
