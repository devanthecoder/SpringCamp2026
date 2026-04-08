using UnityEngine;

[CreateAssetMenu(fileName = "GunScript", menuName = "Scriptable Objects/GunScript")]
public class GunScript : ScriptableObject
{
    public int TotalAmmo;
    public float ShootDelay;
    public float ReloadTime;
    public float GunForce;
    public bool isAutomatic;
}
