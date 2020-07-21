using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Gun[] loadout;
    public Transform weaponParent;
    private GameObject currentWeapon;
    private int CurrentIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Equip(0);
        }
        if(currentWeapon != null){
            Aim(Input.GetMouseButton(1));
        }
    }
    void Equip(int p_ind)
    {
        if(currentWeapon != null){
        Destroy(currentWeapon);    
        }
        CurrentIndex = p_ind;
        GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab,weaponParent.position, weaponParent.rotation,weaponParent) as GameObject;
        t_newWeapon.transform.localPosition = Vector3.zero;
        t_newWeapon.transform.localEulerAngles = Vector3.zero;
        currentWeapon = t_newWeapon;
     }
     void Aim(bool p_isAiming){
        Transform t_Anchor = currentWeapon.transform.Find("Anchor");
        Transform t_state_ads = currentWeapon.transform.Find("States/ADS");
        Transform t_state_hip = currentWeapon.transform.Find("States/Hip");
        if(p_isAiming){
            t_Anchor.position = Vector3.Lerp(t_Anchor.position,t_state_ads.position,Time.deltaTime * loadout[CurrentIndex].aimSpeed);
        }else{
            t_Anchor.position = Vector3.Lerp(t_Anchor.position,t_state_hip.position,Time.deltaTime * loadout[CurrentIndex].aimSpeed);
        }
     }
}
