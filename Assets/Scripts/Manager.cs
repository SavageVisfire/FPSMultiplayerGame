using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Manager : MonoBehaviour
{
    public string player_prefab;
    public Transform spawnPoint;
    private void Start(){
        Spawn();
    }
    public void Spawn(){
        if (PhotonNetwork.IsConnected) { 
            PhotonNetwork.Instantiate(player_prefab,spawnPoint.position,spawnPoint.rotation);
        }
    }
}
