using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Motion : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public float speed;
    public float sprintModifier;
    public Camera normalCam;
    public GameObject cameraParent;
    public float jumpForce;
    private Rigidbody rig;
    private float baseFOV = 60f;
    private float sprintFOVMod = 1.25f;
    public LayerMask ground;
    public Transform groundDetector;
    public int maxHealth;
    private int currentHealth;
    private Manager manager;
    private Transform UI_HealthBar;
    void Start()
    {
        currentHealth = maxHealth;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        cameraParent.SetActive(photonView.IsMine);
        if(Camera.main) Camera.main.enabled = false;
        if(!photonView.IsMine){
            gameObject.layer = 11;
        }
        rig = GetComponent<Rigidbody>();
        if(photonView.IsMine){
        UI_HealthBar=GameObject.Find("HUD/Health/Health").transform;
        refreshHealthBar();
        }
    }
    private void Update(){
        if(!photonView.IsMine) return;
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        if(Input.GetKeyDown(KeyCode.RightShift)){
            takeDamage(100);
        }
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
            rig.AddForce(Vector3.up * jumpForce* Time.deltaTime);
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!photonView.IsMine) return;
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
    public void takeDamage(int p_damage){
        if(photonView.IsMine){
        currentHealth -= p_damage;
        refreshHealthBar();
        if(currentHealth <= 0){
            manager.Spawn();
            PhotonNetwork.Destroy(gameObject);
        }
        }
    }
    private void refreshHealthBar(){
        float t_health_Ratio = (float)currentHealth/(float)maxHealth;
        UI_HealthBar.localScale = new Vector3(t_health_Ratio, 0.5f,1);
    }
}
