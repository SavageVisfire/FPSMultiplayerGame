using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Gun",menuName="Gun")]
public class Gun : ScriptableObject
{
    public string Name;
    public GameObject prefab;
    public float aimSpeed;
    public float recoil;
    public float bloom;
    public float kickback;
    public float fireRate;
    public int damage;
}