﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sway : MonoBehaviour
{
    public float intensity;
    public float smooth;
    public bool isMine;
    private Quaternion origin_Rotation;
    // Start is called before the first frame update
    void Start()
    {
        origin_Rotation = transform.localRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSway();
    }
    private void UpdateSway(){
        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");
        if(!isMine){
            t_x_mouse = 0;
            t_y_mouse = 0;
        }
        Quaternion t_x_adj = Quaternion.AngleAxis(-1*intensity*t_x_mouse,Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(intensity*t_y_mouse,Vector3.right);
        Quaternion target_Rotation = origin_Rotation * t_y_adj*t_x_adj;
        transform.localRotation = Quaternion.Lerp(transform.localRotation,target_Rotation,Time.deltaTime * smooth);
    }
}
