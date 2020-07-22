using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Weapon : MonoBehaviourPunCallbacks
{
    public Gun[] loadout;
    public Transform weaponParent;
    private GameObject currentWeapon;
    private int CurrentIndex;
    public GameObject bulletHolePrefab;
    public LayerMask canBeShot;
    private float currentCooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1)){
            photonView.RPC("Equip",RpcTarget.All,0);
        }
        if(currentWeapon != null){
            if(photonView.IsMine){
            Aim(Input.GetMouseButton(1));
            if(Input.GetMouseButton(0) && currentCooldown <=0){
            photonView.RPC("Shoot",RpcTarget.All);
            }
            if(currentCooldown > 0){
                currentCooldown -= Time.deltaTime;
            }
        }
        currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition,Vector3.zero,Time.deltaTime*4f);
        }
    }
    [PunRPC]
    void Equip(int p_ind)
    {
        if(currentWeapon != null){
        Destroy(currentWeapon);    
        }
        CurrentIndex = p_ind;
        GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab,weaponParent.position, weaponParent.rotation,weaponParent) as GameObject;
        t_newWeapon.transform.localPosition = Vector3.zero;
        t_newWeapon.transform.localEulerAngles = Vector3.zero;
        t_newWeapon.GetComponent<Sway>().isMine = photonView.IsMine;
        currentWeapon = t_newWeapon;
     }
     void Aim(bool p_isAiming){
        Transform t_Anchor = currentWeapon.transform.Find("Anchor");
        Transform t_state_ads = currentWeapon.transform.Find("States/ADS");
        Transform t_state_hip = currentWeapon.transform.Find("States/Hip");
        if(p_isAiming){
            t_Anchor.position = Vector3.Lerp(t_Anchor.position,t_state_ads.position,Time.deltaTime * loadout[CurrentIndex].aimSpeed);
            Transform t_spawn = transform.Find("Cameras/Normal Camera");
            Vector3 forward = t_spawn.TransformDirection(Vector3.forward) * 1000f;
            /*LineRenderer lineRenderer = GetComponent<LineRenderer>(); 
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0,t_spawn.forward * 2 + t_spawn.position);
            lineRenderer.SetPosition(1, t_spawn.forward * 20 + t_spawn.position);
            lineRenderer.startWidth = 0.1f;*/
            //lineRenderer.startColor = Color.green;
            //lineRenderer.endColor = Color.green;
        }else{
            t_Anchor.position = Vector3.Lerp(t_Anchor.position,t_state_hip.position,Time.deltaTime * loadout[CurrentIndex].aimSpeed);
        }
     }
     [PunRPC]
     void Shoot(){
         Transform t_spawn = transform.Find("Cameras/Normal Camera");
         Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;
         t_bloom += Random.Range(-loadout[CurrentIndex].bloom,loadout[CurrentIndex].bloom) * t_spawn.up;
         t_bloom += Random.Range(-loadout[CurrentIndex].bloom,loadout[CurrentIndex].bloom) * t_spawn.right;
         t_bloom-= t_spawn.position;
         t_bloom.Normalize();
         RaycastHit t_hit = new RaycastHit();
         if(Physics.Raycast(t_spawn.position,t_bloom,out t_hit,1000f,canBeShot)){
             GameObject t_newHole = Instantiate(bulletHolePrefab,t_hit.point+t_hit.normal *0.001f,Quaternion.identity) as GameObject;
             t_newHole.transform.LookAt(t_hit.point+t_hit.normal);
             Destroy(t_newHole,5f);
            if(photonView.IsMine){
                if(t_hit.collider.gameObject.layer ==11){
                    t_hit.collider.gameObject.GetPhotonView().RPC("TakeDamage",RpcTarget.All,loadout[CurrentIndex].damage);
                }
            }
         }
         currentWeapon.transform.Rotate(-loadout[CurrentIndex].recoil,0,0);
         currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[CurrentIndex].kickback;
         currentCooldown = loadout[CurrentIndex].fireRate;
     }
    [PunRPC]
    private void TakeDamage(int p_Damage){
        GetComponent<Motion>().takeDamage(p_Damage);
    }
}
