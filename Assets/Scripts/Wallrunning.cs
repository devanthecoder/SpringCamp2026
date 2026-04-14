using UnityEngine;

public class Wallrunning : MonoBehaviour
{
    // Forces
    public float forwardForce;
    public float stickForce;
    public float upForce;
    public float normalForce;
    // Detection
    bool leftWall, rightWall;
    RaycastHit leftWallHit, rightWallHit;
    public LayerMask wallLayer, groundLayer;
    public float rayCastDist;
    public float minJumpHeight;
    bool canWallRun = true;
    public float wallRunResetTime;
    // Input
    float horizontal, vertical;
    // References
    Rigidbody rb;
    Movement pm;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Movement>();
    }
    void Update()
    {
        CheckWall();

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if(!pm.isCrouching && !pm.isSliding && AboveGround() && (leftWall || rightWall) && vertical > 0f && canWallRun)
        {
            if(!pm.isWallrunning)
            {
                StartWallRun();
            }
        } else
        {
            if(pm.isWallrunning)
            {
                StopWallRun();
            }
        }
    }
    void FixedUpdate()
    {
        if(pm.isWallrunning)
        {
            WallRunMovement();
        }
    }
    void CheckWall()
    {
        leftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, rayCastDist, wallLayer);
        rightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, rayCastDist, wallLayer);
    }
    bool AboveGround()
    {
        bool notEnoughAbove = Physics.Raycast(transform.position, -transform.up, minJumpHeight, groundLayer);
        return !notEnoughAbove;
    }
    void StartWallRun()
    {
        rb.useGravity = false;
        pm.isWallrunning = true;
    }
    void StopWallRun()
    {
        rb.useGravity = true;
        pm.isWallrunning = false;
        canWallRun = false;
        Invoke(nameof(ResetWallRun), wallRunResetTime);
    }
    void WallRunMovement()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Vector3 wallNormal = rightWall ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallUp = transform.up;
        Vector3 wallForward = Vector3.Cross(wallNormal, wallUp);

        if(Vector3.Dot(wallForward, transform.forward) < 0f)
        {
            wallForward = -wallForward;
        }

        rb.AddForce(wallForward * forwardForce, ForceMode.Force);

        if(!((leftWall && horizontal > 0) || (rightWall && horizontal < 0)))
            rb.AddForce(-wallNormal * stickForce, ForceMode.Force);
    }
    public void WallJump()
    {
        StopWallRun();
        Vector3 wallNormal = rightWall ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallUp = transform.up;
        Vector3 JumpForce = wallNormal * normalForce + wallUp * upForce;

        rb.AddForce(JumpForce, ForceMode.Impulse);
    }
    void ResetWallRun()
    {
        canWallRun = true;
    }
}
