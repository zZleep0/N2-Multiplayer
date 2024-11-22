using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ShowPlayerNickname : MonoBehaviourPunCallbacks
{
    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        //Definir o nome do jogador
        GetComponent<Text>().text = photonView.Owner.NickName;

        //Procurando a camera do jogador
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCamera != null && !photonView.IsMine)
        {
            //Faz o nome olhar para a camera, com rotacao adequada
            transform.LookAt(playerCamera.transform.position);

            //Mantem a rotacao correta, apenas para nao inverter o nome
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.x = 0; //Previne a rotacao em x
            eulerRotation.y = 0; //Previne a rotacao em y
            transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }
}
