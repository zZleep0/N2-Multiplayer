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

    private LevelManager levelManager;
    private InimigoController inimigoController;


    // Start is called before the first frame update
    void Start()
    {
        inimigoController = GameObject.Find("Inimigo").GetComponent<InimigoController>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
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

        // Instanciar o jogador
        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity).gameObject;
        // Definir o nome do objeto do jogador como o nickname
        playerObject.name = "Player " + PhotonNetwork.NickName;


        levelManager.startTempo = true;

        // Agora vamos esperar um momento para garantir que todos os jogadores foram instanciados antes de buscar os jogadores
        StartCoroutine(FindPlayersWithDelay());
    }

    private IEnumerator FindPlayersWithDelay()
    {
        // Espera um segundo para garantir que todos os jogadores tenham sido instanciados
        yield return new WaitForSeconds(3f);

        // Agora podemos chamar o método de encontrar os jogadores
        inimigoController.FindPlayers();

    }

    public void StartTheGame()
    {
        PhotonNetwork.NickName = playerNickname.text;
        PhotonNetwork.ConnectUsingSettings();

        nicknameInput.SetActive(true);
    }

}