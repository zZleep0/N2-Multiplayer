using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int pontos = 0;
    public Text txtPontos;

    public bool vitoria = false;
    public bool derrota = false;

    

    public Text txtTempo;
    public float tempo;
    public bool startTempo = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txtPontos.text = pontos + "/10";

        //if (tempo < 120 && vitoria == false && startTempo)
        //{
        //    tempo += Time.deltaTime;
        //}
        //else if (tempo  > 120)
        //{
        //    tempo = 120;
        //    derrota = true;
        //}
        
        //txtTempo.text = tempo.ToString();

        if (pontos >= 10)
        {
            vitoria = true;
        }

        if (derrota)
        {
            
        }
        if (vitoria)
        {
            SceneManager.LoadSceneAsync("Vitoria");
        }
    }
}
