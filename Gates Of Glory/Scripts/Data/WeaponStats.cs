using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapon Stats")]
public class WeaponStats : ScriptableObject 
{

    [Header("Weapon Stats")]
    public Stat myDamage;
    public Stat myForce;
    public Stat myFireRate;
    [Space(10)]
    public int autoFireLevelRequirement;
}
