﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Gun",menuName="Gun")]
public class Gun : ScriptableObject
{
    public string Name;
    public GameObject prefab;
    public float aimSpeed;
}