using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public DummyGun[] DummyGuns;
    public Text AmmoDisplay;
    private int currIndex = 0;
    private GunScript Gun;
    private bool canShoot;
    private int currAmmo;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canShoot = true;
        Gun = DummyGuns[currIndex].Gun;
        currAmmo = Gun.TotalAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchGun(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchGun(1);
        }
        if ((Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && Gun.isAutomatic)) && canShoot)
        {
            if (currAmmo > 0)
            {
                Debug.Log(currAmmo);   
                canShoot = false;
                Shoot();  
                Debug.Log("Shot");
                Invoke("Wait", Gun.ShootDelay); 
            }
            else
            {
                Invoke("Reload", Gun.ReloadTime);
            }
        }
        AmmoDisplay.text = currAmmo.ToString();
    }
    void Shoot()
    {
        DummyGuns[currIndex].anim.SetTrigger("Recoil");
        currAmmo--;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Successful Hit");
                rb.AddForceAtPosition(Gun.GunForce * Camera.main.transform.forward, hit.point, ForceMode.Impulse);   
            }
        }
    }
    void Wait()
    {
        canShoot = true;
    }
    void Reload()
    {
        currAmmo = Gun.TotalAmmo;
    }
    void SwitchGun(int index)
    {
        DummyGuns[currIndex].currAmmo = currAmmo;
        for(int i = 0; i < DummyGuns.Length; i++)
        {
            bool setActive = i==index;
            DummyGuns[i].gameObject.SetActive(setActive);
        }
        currIndex = index;
        currAmmo = DummyGuns[currIndex].currAmmo;
        Gun = DummyGuns[currIndex].Gun;
    }
}
