using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player // 플레이어 클래스 정의
{
    public float speed = 4f;
    public float jumpForce = 5.5f;
    public float hunger = 100f;

    // Hunger 프로퍼티 
    public float Hunger
    {
        get { return hunger; }
        set
        {
            hunger = value;
            Debug.Log("Hunger: " + hunger); // 현재 공복감 출력
            if (hunger >= 100 && hunger < 150)
            {
                OnSlow?.Invoke();
            }
            else if (hunger <= 0)
            {
                hunger = 0;
                OnDie?.Invoke();
            }
        }
    }

    // 이벤트 델리게이트 이벤트 정의
    public delegate void Action();
    public event Action OnSlow;
    public event Action OnDie;
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Animator animator;

    public bool isJump; // 점프를 하였는가?
    public bool isGround; // 땅과 충돌하였는가?
    public bool isWalk; // 걷는건가? 
    public bool isDead; // 죽은건가?

    public Player player = new Player(); // 플레이어 생성

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // y축 이동을 제한
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        // 이벤트 
        player.OnSlow += Slow;
        player.OnDie += Die;
    }

    void Start()
    {
        // 공복감 감소 코루틴
        StartCoroutine(DecreaseHunger());

        Debug.Log("Speed: " + player.speed); // 현재 스피드 출력
    }

    void Update()
    {
        // 죽는 경우
        if (isDead)
        {
            return;
        }

        // 플레이어 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }

        if (player.speed > 10f)
        {
            StartCoroutine(DecreaseSpeed());
        }
        if (player.speed < 4f) // 기본값으로 변경
        {
            player.speed = 4f;
        }
        if (player.Hunger == 0f) // Hunger 0이되는 경우
        {
            Die();
        }
    }

    IEnumerator DecreaseHunger()
    {
        while (player.Hunger > 0)
        {
            yield return new WaitForSeconds(1f); // 1초마다 감소
            player.Hunger -= 1f; // 1씩 감소

            if (player.Hunger <= 20) // 배고픔이 20 이하로 떨어졌을 때
            {
                Slow(); // 속도 감소
            }
        }
    }

    IEnumerator DecreaseSpeed()
    {
        while (player.speed > 10f)
        {
            yield return new WaitForSeconds(0.1f); // 0.1초마다 감소
            player.speed -= 0.1f; // 0.1씩 감소
            Debug.Log("SlowSpeed: " + player.speed); // 현재 스피드 출력
        }
        player.speed = 4f;
    }

    void FixedUpdate()
    {
        // 죽는 경우 파악
        if (isDead)
        {
            return;
        }

        // 플레이어 이동
        Move();
    }

    void Move()
    {
        if (!isJump)
        {
            float xInput = Input.GetAxis("Horizontal"); // 수평
            float zInput = Input.GetAxis("Vertical"); // 수직

            Vector3 movement = new Vector3(xInput, 0, zInput).normalized; // 정규화

            // 조종을 하고 있는 경우에만 회전을 적용
            if (movement != Vector3.zero)
            {
                animator.SetBool("IsRun", true);

                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * player.speed);

                Vector3 newVelocity = transform.forward * player.speed; // 속도 변경을 위해 transform.forward 사용
                playerRigidbody.velocity = newVelocity;

                if (isJump)
                {
                    newVelocity *= 0.5f;
                }
            }
            else
            {
                playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
                animator.SetBool("IsRun", false);
            }
        }
    }

    void Jump()
    {
        animator.SetTrigger("IsJump");
        isJump = true;
        isGround = false;

        playerRigidbody.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            isJump = false;
            animator.SetBool("IsGround", true);
        }
        if (collision.gameObject.CompareTag("P_item"))
        {
            player.speed += 1.5f;
            player.Hunger += 2f;
            Debug.Log("FastSpeed: " + player.speed); // 현재 스피드 출력
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Trainer")) // 충돌시
        {
            player.speed -= 1f;  // 속도 감소
            player.Hunger -= 5f; // 허기짐 감소
        }
    }

    public void Slow()
    {
        animator.SetBool("IsWalk", true);

        Debug.Log("SlowSpeed: " + player.speed); // 현재 스피드 출력

        isGround = true;
    }

    public void Die()
    {
        isJump = false;
        animator.SetTrigger("IsDie");
        isDead = true;
        playerRigidbody.velocity = Vector3.zero;
        GameManger.instance.EndGame();
    }
}
















