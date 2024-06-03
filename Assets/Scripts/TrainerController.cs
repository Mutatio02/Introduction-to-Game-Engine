using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrainerController : MonoBehaviour
{
    public float speed = 3f; // �⺻ �ӵ�
    public GameObject player; // �÷��̾� ������Ʈ

    private NavMeshAgent navAgent;
    private Animator t_animator;
    private bool isRunning = false; // ���� ���¸� �����ϴ� ����

    // Awake�� ������� ���� (������Ʈ, collider ��)
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        t_animator = GetComponent<Animator>();

        // NavMeshAgent �ӵ� ����
        navAgent.speed = speed;
    }

    void Start()
    {
        // �ʱ� �ִϸ��̼� ���¸� Idle�� ����
        t_animator.SetBool("IsRun", false);

        // ���� �ð� �� Run �ִϸ��̼����� ��ȯ
        Invoke("StartRunning", 3f); // 3�� �Ŀ� �޸��� ����
    }

    void Update()
    {
        if (player != null && isRunning)
        {
            // �÷��̾� ��ġ�� NavMeshAgent�� �̵�
            navAgent.SetDestination(player.transform.position);
        }
    }

    void StartRunning()
    {
        // Run �ִϸ��̼����� ��ȯ
        t_animator.SetBool("IsRun", true);
        isRunning = true; // ���� ���¸� ����
        Debug.Log("Start Running"); // ����� �޽��� �߰�
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �±װ� Player�� �Ͱ� �浹 ��
        {
            Debug.Log("�浹"); // �浹 Ȯ��
        }
        if (collision.gameObject.CompareTag("P_item")) // �±װ� P_item�� �Ͱ� �浹 ��
        {
            Destroy(collision.gameObject); // �÷��̾� ������ �ı�
        }
    }
    public void StopRunning()
    {
        Debug.Log("StopAnimation method is called.");
        // �ִϸ��̼��� ���߰� ���¸� ����
        t_animator.SetBool("IsRun", false);
        isRunning = false;
    }
}






