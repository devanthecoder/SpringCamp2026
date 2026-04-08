using UnityEngine;

public class DummyGun : MonoBehaviour
{
    public GunScript Gun;
    public int currAmmo;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currAmmo = Gun.TotalAmmo;
    }
}
