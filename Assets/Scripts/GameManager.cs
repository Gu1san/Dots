using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Instance
    public static GameManager instance;

    [Header("Variaveis")]
    public int playerLife = 100;
    public int playerScore = 0;
    public float startTimeInMinutes = 0f; // Tempo configurável no Inspector
    private float elapsedTime; // Tempo restante em segundos
    public bool isRunning = false;
    private float nextBonusTime = 20f; // Tempo para o próximo bônus

    [Header("UI Referencias")]
    public TMP_Text timeText; // Arraste um Text da UI aqui para mostrar o tempo
    public TMP_Text scoreText; // Arraste um Text da UI aqui para mostrar os pontos do jogador
    public TMP_Text playerLifeText; // Arraste um Text da UI aqui para mostrar a vida do jogador
    public GameObject btnStart; // Botão de começar/recomeçar o jogo
    public GameObject defeatPanel; // painel de derrota


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        isRunning = false;
        defeatPanel.SetActive(true);
        btnStart.SetActive(true);
        elapsedTime = startTimeInMinutes * 60f;
        playerLifeText.text = "Life: " + playerLife;
        UpdateUI();
    }

    void Update()
    {
        if (isRunning == true)
        {
            elapsedTime += Time.deltaTime;

            // Verifica se passou de 20 segundos para adicionar pontos
            if (elapsedTime >= nextBonusTime)
            {
                playerScore += 10;
                nextBonusTime += 20f;
                Debug.Log("Bônus de 10 pontos! Score atual: " + playerScore);
            }

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + playerScore;
        }
    }

    public void AddPoints(int p)//Serve para acrecentar pontos
    {
        playerScore += p;
        //scoreText.text = "Pontos: " + playerScore;
    }

    public void AddDemage(int L)
    {
        playerLife -= L;
        if (playerLife <= 0)
        {
            isRunning = false;
            defeatPanel.SetActive(true);
            btnStart.SetActive(true);
        }
        playerLifeText.text = "Life: " + playerLife;
    }

    public void btnPlay()
    {
        elapsedTime = startTimeInMinutes * 60f;
        playerLife = 100;
        playerScore = 0;
        playerLifeText.text = "Life: " + playerLife;
        scoreText.text = "Score: " + playerScore;
        defeatPanel.SetActive(false);
        btnStart.SetActive(false);
        isRunning = true;
    }

}
