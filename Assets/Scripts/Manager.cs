using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Manager : MonoBehaviour
{
    public string player_prefab;
    public Transform[] spawnPoints;
    private void Start(){
        Spawn();
    }
    public void Spawn(){
        if (PhotonNetwork.IsConnected) {
            Transform t_spawn = spawnPoints[Random.Range(0,spawnPoints.Length)];
            PhotonNetwork.Instantiate(player_prefab,t_spawn.position,t_spawn.rotation);
        }
    }
}
