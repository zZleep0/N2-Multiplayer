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
    [SerializeField] private List<Transform> players = new List<Transform>();

    [SerializeField] private float changeTargetCooldown = 5f; // Tempo de espera antes de trocar o alvo
    private bool canChangeTarget = true; // Controla se o inimigo pode trocar de alvo

    public float tempoCD = 2f;


    // Start is called before the first frame update
    void Start()
    {
        if (agent == null)
        {
            if (!TryGetComponent(out agent))
            {
                Debug.LogWarning(name + " precisa colocar um NavMesh Agent");
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

        tempoCD -= Time.deltaTime;

        // Se o cooldown estiver ativo, n�o chama a fun��o
        if (canChangeTarget && tempoCD <= 0)
        {

            Debug.Log("ta tentando encontrar player mais proximo");

            // Encontrar o jogador mais pr�ximo
            FindClosestPlayer();

            tempoCD = 2f;
        }

        if (target != null)
        {
            MoveToTarget();
        }
    }

    public void FindPlayers()
    {
        // Limpar a lista de jogadores a cada chamada
        players.Clear();

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObject in playerObjects)
        {
            // Verificar se o objeto tem PhotonView e se � v�lido
            PhotonView photonView = playerObject.GetComponent<PhotonView>();
            if (photonView != null && photonView.Owner != null)
            {
                // Adicionar apenas jogadores conectados via Photon
                players.Add(playerObject.transform);
            }
        }

        Debug.Log("Encontrados " + players.Count + " jogadores.");
    }

    // Encontra o jogador mais pr�ximo
    private void FindClosestPlayer()
    {
        if (!canChangeTarget)
        {
            Debug.Log("Aguardando cooldown para trocar de alvo.");
            return;
        }

        if (players == null || players.Count == 0)
        {
            Debug.LogWarning("Nenhum jogador dispon�vel na lista para encontrar o mais pr�ximo.");
            target = null;
            return;
        }

        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            if (player == null)
            {
                Debug.LogWarning("Um dos jogadores na lista est� nulo.");
                continue; // Pula itera��es com jogadores inv�lidos
            }

            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            if (target == null || target != closestPlayer)
            {
                target = closestPlayer;
                Debug.Log("Novo alvo selecionado: " + target.name);
                StartCoroutine(ChangeTargetCooldown());
            }
        }
        else
        {
            Debug.LogWarning("Nenhum jogador v�lido foi encontrado como mais pr�ximo.");
            target = null;
        }
    }

    private IEnumerator ChangeTargetCooldown()
    {
        canChangeTarget = false; // Bloqueia a troca de alvo
        yield return new WaitForSeconds(changeTargetCooldown); // Espera pelo tempo definido
        canChangeTarget = true; // Permite trocar de alvo novamente
        Debug.Log("Cooldown finalizado. Pode trocar de alvo novamente.");
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            agent.isStopped = false;
        }
    }
}

