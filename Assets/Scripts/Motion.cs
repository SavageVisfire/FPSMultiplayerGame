using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private Rigidbody rig;
    void Start()
    {
        Camera.main.enabled = false;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float t_hmove = Input.GetAxis("Horizontal");
        float t_vmove = Input.GetAxis("Vertical");
        Vector3 t_direction = new Vector3(t_hmove,0,t_vmove);
        t_direction.Normalize();
        rig.velocity = transform.TransformDirection(t_direction)*speed*Time.fixeddeltaTime;
    }
}
