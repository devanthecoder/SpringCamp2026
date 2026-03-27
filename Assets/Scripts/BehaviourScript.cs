using UnityEngine;

public class BehaviourScript : MonoBehaviour
{
    private bool canShoot;
    private int currAmmoCount;
    public GameObject[] Guns;
    // int currGunIndex = 0;
    private GunScript currGun;
    private Animator GunAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // currGun = Guns[0].GetComponent<DummyGun>().Gun;
        SwitchGun(0);
        canShoot = true;
        currAmmoCount = currGun.totalAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i <= Guns.Length; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int newIndex = i - 1;
                SwitchGun(newIndex);
            }
        }
        Debug.Log(currAmmoCount);
        if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && currGun.isAutomatic))
        {
            if(currAmmoCount > 0)
            {
                if (canShoot)
                {
                    canShoot = false;
                    Shoot();
                    Invoke("Wait", currGun.shootDelay);
                }
            }
            else Invoke("Reload", currGun.reloadTime);
        }
    }
    void Shoot()
    {
        GunAnim.SetTrigger("Recoil");
        currAmmoCount--;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            Rigidbody hitRb = hit.collider.GetComponent<Rigidbody>();
            if (hitRb != null)
            {
            Vector3 dir = Camera.main.transform.forward;
            hitRb.AddForceAtPosition(dir * currGun.gunForce, hit.point, ForceMode.Impulse);
            }

        }
    }
    void Reload()
    {
        currAmmoCount = currGun.totalAmmo;
    }
    void Wait()
    {
        canShoot = true;
    }
    void SwitchGun(int index)
    {
        for(int i = 0; i < Guns.Length; i++)
        {
            Guns[i].SetActive(i==index);
        }
        // currGunIndex = index;
        currGun = Guns[index].GetComponent<DummyGun>().Gun;
        GunAnim = Guns[index].GetComponent<DummyGun>().anim;
    }
}
