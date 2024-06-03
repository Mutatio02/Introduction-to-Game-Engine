using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;

    public bool isOver; // 게임 종료

    public Text hungerText; // Hunger 값을 표시
    public Text speedText; // Speed 값을 표시
    public GameObject gameOverText; // 게임 종료 텍스트

    public AudioSource audioSource; // 음향

    private PlayerController playerController; 
    private TrainerController trainerController;

    void Awake()
    {
        if (instance != null) // 자기 자신이 아니라면
        {
            Destroy(gameObject); //자기 자신을 파괴
        }
        else
        {
            instance = this; //자기 자신
        }
    }

    void Start()
    {
        isOver = false; 
        gameOverText.SetActive(false); // 안내문구 비활성화

        playerController = FindObjectOfType<PlayerController>(); // 스크립트 가져오기
        trainerController = FindObjectOfType<TrainerController>();

        if (trainerController != null)
        {
            Debug.Log("TrainerController is found.");
        }
        else
        {
            Debug.Log("TrainerController is not found.");
        }
    }

    void Update()
    {
        if (isOver)
        {
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                Debug.Log("Esc execute");
            }
        }
        else
        {
            if (playerController != null)
            {
                hungerText.text = "Hunger: " + ((int)playerController.player.Hunger).ToString(); //형변환
                speedText.text = "Speed: " + playerController.player.speed.ToString("F1"); //소수점 첫째 자리

                // 플레이어가 죽으면 
                if (playerController.isDead)
                {
                    
                    if (trainerController != null)
                    {
                        trainerController.StopRunning();
                    }
                }
            }
        }
    }

    public void EndGame()
    {
        isOver = true;
        gameOverText.SetActive(true);
        audioSource.Stop();
        audioSource.gameObject.SetActive(false); // 비활성화    
        Debug.Log("BGM stopped"); // 확인 
    }
}

