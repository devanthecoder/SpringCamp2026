using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform player;
    NavMeshAgent agent;
    public float attackRange;
    public float lockDelay;
    bool lockOn;
    public GameObject bulletPrefab;
    public Transform shootPos;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lockOn = false;
    }

    void Update()
    {
        if(lockOn == true) 
            return;
        transform.LookAt(player);
        if(Vector3.Distance(transform.position, player.position) > agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
        } else
        {
            agent.isStopped = true;
            StartCoroutine(LockMode());
        }
    }
    IEnumerator LockMode()
    {
        lockOn = true;
        float timer = 0f;
        while(timer < lockDelay)
        {
            timer += Time.deltaTime;
            transform.LookAt(player);
            if(Vector3.Distance(transform.position, player.position) > agent.stoppingDistance + attackRange)
            {
                lockOn = false;
                yield break;
            }
            yield return null;
        }
        Shoot();
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        bullet.transform.LookAt(player);
        lockOn = false;
    }
}
