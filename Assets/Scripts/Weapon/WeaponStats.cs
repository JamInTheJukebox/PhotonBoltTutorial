using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Equipment/Weapon")]
public class WeaponStats : ScriptableObject
{
    [Space(40)]
    public string weaponName;
    public float reloadTime = 1f;
    public int magazine = 30;
    public int totalMagazine = 120;
    public int ammoPerShot = 1;
    public int multiShot = 1;
    [Space(10)]
    public int rpm = 80;
    public int dmg = 25;
    public int maxRange = 30;
    public float precision = 0.3f;
    [Space(10)]
    public GameObject trail;
    public GameObject decal;
    public GameObject impact;
}