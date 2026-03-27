using UnityEngine;

[CreateAssetMenu(fileName = "GunScript", menuName = "Scriptable Objects/GunScript")]
public class GunScript : ScriptableObject
{
    public int totalAmmo; 
    public float gunForce;
    public float reloadTime;
    public float shootDelay;
    public bool isAutomatic;
    
    public Animator anim;
}
