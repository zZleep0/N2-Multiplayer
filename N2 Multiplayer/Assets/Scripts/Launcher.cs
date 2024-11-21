using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public PhotonView playerPrefab;

    public InputField playerNickname;
    public GameObject nicknameInput;


    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
    }



    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomOrCreateRoom();
        //base.OnConnectedToMaster(); //Instanciado ao criar classe
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        //base.OnJoinedRoom(); //Instanciado ao criar classe
    }

    public void StartTheGame()
    {
        PhotonNetwork.NickName = playerNickname.text;
        PhotonNetwork.ConnectUsingSettings();

        nicknameInput.SetActive(true);
    }

}