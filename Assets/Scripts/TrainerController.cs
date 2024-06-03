using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrainerController : MonoBehaviour
{
    public float speed = 3f; // 기본 속도
    public GameObject player; // 플레이어 오브젝트

    private NavMeshAgent navAgent;
    private Animator t_animator;
    private bool isRunning = false; // 현재 상태를 저장하는 변수

    // Awake는 상관없이 실행 (컴포넌트, collider 등)
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        t_animator = GetComponent<Animator>();

        // NavMeshAgent 속도 설정
        navAgent.speed = speed;
    }

    void Start()
    {
        // 초기 애니메이션 상태를 Idle로 설정
        t_animator.SetBool("IsRun", false);

        // 일정 시간 후 Run 애니메이션으로 전환
        Invoke("StartRunning", 3f); // 3초 후에 달리기 시작
    }

    void Update()
    {
        if (player != null && isRunning)
        {
            // 플레이어 위치로 NavMeshAgent를 이동
            navAgent.SetDestination(player.transform.position);
        }
    }

    void StartRunning()
    {
        // Run 애니메이션으로 전환
        t_animator.SetBool("IsRun", true);
        isRunning = true; // 현재 상태를 갱신
        Debug.Log("Start Running"); // 디버그 메시지 추가
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 태그가 Player인 것과 충돌 시
        {
            Debug.Log("충돌"); // 충돌 확인
        }
        if (collision.gameObject.CompareTag("P_item")) // 태그가 P_item인 것과 충돌 시
        {
            Destroy(collision.gameObject); // 플레이어 아이템 파괴
        }
    }
    public void StopRunning()
    {
        Debug.Log("StopAnimation method is called.");
        // 애니메이션을 멈추고 상태를 변경
        t_animator.SetBool("IsRun", false);
        isRunning = false;
    }
}






