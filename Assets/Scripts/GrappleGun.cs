using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    SpringJoint joint;
    LineRenderer lr;
    public Transform GunTip;
    bool isGrappling;
    Vector3 grapplePoint, dir;
    Rigidbody rb;
    public float maxRange = 50f;
    public float Stiffness = 10f;
    public float grappleDelay = 0.2f;
    Movement pm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        joint = GetComponent<SpringJoint>();
        lr.enabled = false;
        isGrappling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            StartGrapple();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            StopGrapple();
        }
    }
    void LateUpdate()
    {
        if (isGrappling)
        {
            lr.SetPosition(0, GunTip.position);
            lr.SetPosition(1, grapplePoint);
        }
    }
    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(GunTip.position, Camera.main.transform.forward, out hit, maxRange))
        {
            pm.freeze = true;
            lr.enabled = true;
            grapplePoint = hit.point;
            isGrappling = true;
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;
            joint.spring = Stiffness;
            Invoke("StopFreeze", grappleDelay);
        }
    }
    void StopFreeze()
    {
        pm.freeze = false;
    }
    void StopGrapple()
    {
        joint.spring = 0;
        pm.freeze = false;    // joint.spring = 0f;
        lr.enabled = false;
        isGrappling = false;
    }
}
