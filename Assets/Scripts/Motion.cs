﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float sprintModifier;
    public Camera normalCam;
    public float jumpForce;
    private Rigidbody rig;
    private float baseFOV = 60f;
    private float sprintFOVMod = 1.25f;
    public LayerMask ground;
    public Transform groundDetector;
    void Start()
    {
        Camera.main.enabled = false;
        rig = GetComponent<Rigidbody>();
    }
    private void Update(){
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool jump = Input.GetKey(KeyCode.Space);
        bool isGrounded = Physics.Raycast(groundDetector.position,Vector3.down,0.1f,ground);
        bool isJumping = jump;
        if(isGrounded){
            isJumping = jump;
        }else{
            isJumping = false;
        }
        bool isSprinting;
        if(t_vmove > 0 && isJumping == false&& isGrounded == true){
            isSprinting = sprint;
        }else{
            isSprinting = false;
        }
        if(isJumping){
            rig.AddForce(Vector3.up * jumpForce);
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool jump = Input.GetKey(KeyCode.Space);
        bool isGrounded = Physics.Raycast(groundDetector.position,Vector3.down,0.1f,ground);
        bool isJumping = jump;
        if(isGrounded){
            isJumping = jump;
        }else{
            isJumping = false;
        }
        bool isSprinting;
        if(t_vmove > 0 && isJumping == false&& isGrounded == true){
            isSprinting = sprint;
        }else{
            isSprinting = false;
        }
        Vector3 t_direction = new Vector3(t_hmove,0,t_vmove);
        t_direction.Normalize();
        float t_adjustedSpeed = speed;
        if(isSprinting){
            t_adjustedSpeed = t_adjustedSpeed * sprintModifier;
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView,baseFOV*sprintFOVMod,Time.deltaTime*8);
        }else{
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView,baseFOV,Time.deltaTime*8);
        }
        Vector3 t_targetVelocity = transform.TransformDirection(t_direction)*t_adjustedSpeed*Time.fixedDeltaTime;
        t_targetVelocity.y = rig.velocity.y;
        rig.velocity = t_targetVelocity;
    }
}
