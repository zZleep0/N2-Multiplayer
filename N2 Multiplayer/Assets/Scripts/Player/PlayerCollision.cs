using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviourPunCallbacks
{
    public LayerMask inimigoL;
    public LayerMask objetivoL;
    public float capsuleRadius = 1f; // Raio da cápsula

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

        DetectCollision(inimigoL, "Inimigo");

        DetectCollision(objetivoL, "Objetivo");

    }

    private void DetectCollision(LayerMask layer, string tipo)
    {
        Vector3 capsuleStart = transform.position + Vector3.up * 0.5f;
        Vector3 capsuleEnd = transform.position + Vector3.up * 2f;

        // Detecta colisores no LayerMask especificado
        Collider[] hitColliders = Physics.OverlapCapsule(capsuleStart, capsuleEnd, capsuleRadius, layer);

        // Itera sobre os objetos detectados
        foreach (Collider hitCollider in hitColliders)
        {
            if (tipo == "Objetivo")
            {
                levelManager.pontos++;
                Debug.Log(levelManager.pontos);
                Destroy(hitCollider.gameObject);
            }
            Debug.Log($"Colidiu com {hitCollider.gameObject.name} no layer {tipo} !");
        }

        foreach (Collider hitCollider in hitColliders)
        {
            if (tipo == "Inimigo")
            {
                
                Debug.Log("pego pela muie");
                
            }
            Debug.Log($"Colidiu com {hitCollider.gameObject.name} no layer {tipo} !");
        }
    }
}
