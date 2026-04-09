using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed;
    public float damage;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.linearVelocity = transform.forward * speed;
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.GetComponent<HealthBar>() != null)
        {
            collision.transform.GetComponent<HealthBar>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
