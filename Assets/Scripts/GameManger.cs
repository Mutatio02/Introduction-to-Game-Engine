using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;

    public bool isOver; // ���� ����

    public Text hungerText; // Hunger ���� ǥ��
    public Text speedText; // Speed ���� ǥ��
    public GameObject gameOverText; // ���� ���� �ؽ�Ʈ

    public AudioSource audioSource; // ����

    private PlayerController playerController; 
    private TrainerController trainerController;

    void Awake()
    {
        if (instance != null) // �ڱ� �ڽ��� �ƴ϶��
        {
            Destroy(gameObject); //�ڱ� �ڽ��� �ı�
        }
        else
        {
            instance = this; //�ڱ� �ڽ�
        }
    }

    void Start()
    {
        isOver = false; 
        gameOverText.SetActive(false); // �ȳ����� ��Ȱ��ȭ

        playerController = FindObjectOfType<PlayerController>(); // ��ũ��Ʈ ��������
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
                hungerText.text = "Hunger: " + ((int)playerController.player.Hunger).ToString(); //����ȯ
                speedText.text = "Speed: " + playerController.player.speed.ToString("F1"); //�Ҽ��� ù° �ڸ�

                // �÷��̾ ������ 
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
        audioSource.gameObject.SetActive(false); // ��Ȱ��ȭ    
        Debug.Log("BGM stopped"); // Ȯ�� 
    }
}

