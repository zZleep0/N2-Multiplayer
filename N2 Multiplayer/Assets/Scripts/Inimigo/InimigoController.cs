using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class InimigoController : MonoBehaviourPunCallbacks
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;

    //Distancia para ele eliminar o alvo
    [SerializeField] private float killDistance = 3f;
    //Quanto o npc tem que girar para atingir o alvo para elimina-lo
    [SerializeField] private float lookThreshold = 0.8f;

    // Lista para armazenar todos os jogadores
    private List<Transform> players = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        if (agent == null)
        {
            if (!TryGetComponent(out agent))
            {
                Debug.LogWarning(name + "precisa colocar um navmesh agent");
            }
        }


    }

    // Update is called once per frame
    void Update()
    {


        if (players.Count == 0)
        {
            Debug.Log("Nenhum jogador encontrado.");
            return;
        }

        // Encontrar o jogador mais próximo
        FindClosestPlayer();

        //if (target == null)
        //{
        //    Debug.Log("inimigo achou player");
        //    target = GameObject.Find("Player").GetComponent<Transform>();
        //}

        if (target != null)
        {
            MoveToTarget();

            //if (Vector3.Dot(transform.forward.normalized, target.position.normalized) <= lookThreshold && Vector3.Distance(transform.position, target.position) <= killDistance)
            //{
            //    KillTarget();

            //    return;
            //}
        }

        
    }

    public void FindPlayers()
    {
        // Limpar a lista de jogadores a cada chamada
        players.Clear();

        // Iterar sobre os jogadores conectados
        foreach (var player in PhotonNetwork.PlayerList)
        {
            // Encontrar o GameObject do jogador pela sua ID (ou NickName, dependendo do que está sendo usado na cena)
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // Ou usar uma outra forma de identificar o jogador na cena

            

            // Verifique se o jogador está instanciado na cena
            if (playerObject != null)
            {
                // Adiciona o transform do jogador na lista
                players.Add(playerObject.transform);
            }
        }

        Debug.Log("Encontrados " + players.Count + " jogadores.");
    }

    // Encontra o jogador mais próximo
    private void FindClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        target = closestPlayer;
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
        agent.isStopped = false;
    }

    private void KillTarget()
    {
        Debug.Log("morreu");
    }
}
