using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player // �÷��̾� Ŭ���� ����
{
    public float speed = 4f;
    public float jumpForce = 5.5f;
    public float hunger = 100f;

    // Hunger ������Ƽ 
    public float Hunger
    {
        get { return hunger; }
        set
        {
            hunger = value;
            Debug.Log("Hunger: " + hunger); // ���� ������ ���
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

    // �̺�Ʈ ��������Ʈ �̺�Ʈ ����
    public delegate void Action();
    public event Action OnSlow;
    public event Action OnDie;
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Animator animator;

    public bool isJump; // ������ �Ͽ��°�?
    public bool isGround; // ���� �浹�Ͽ��°�?
    public bool isWalk; // �ȴ°ǰ�? 
    public bool isDead; // �����ǰ�?

    public Player player = new Player(); // �÷��̾� ����

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // y�� �̵��� ����
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        // �̺�Ʈ 
        player.OnSlow += Slow;
        player.OnDie += Die;
    }

    void Start()
    {
        // ������ ���� �ڷ�ƾ
        StartCoroutine(DecreaseHunger());

        Debug.Log("Speed: " + player.speed); // ���� ���ǵ� ���
    }

    void Update()
    {
        // �״� ���
        if (isDead)
        {
            return;
        }

        // �÷��̾� ����
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }

        if (player.speed > 10f)
        {
            StartCoroutine(DecreaseSpeed());
        }
        if (player.speed < 4f) // �⺻������ ����
        {
            player.speed = 4f;
        }
        if (player.Hunger == 0f) // Hunger 0�̵Ǵ� ���
        {
            Die();
        }
    }

    IEnumerator DecreaseHunger()
    {
        while (player.Hunger > 0)
        {
            yield return new WaitForSeconds(1f); // 1�ʸ��� ����
            player.Hunger -= 1f; // 1�� ����

            if (player.Hunger <= 20) // ������� 20 ���Ϸ� �������� ��
            {
                Slow(); // �ӵ� ����
            }
        }
    }

    IEnumerator DecreaseSpeed()
    {
        while (player.speed > 10f)
        {
            yield return new WaitForSeconds(0.1f); // 0.1�ʸ��� ����
            player.speed -= 0.1f; // 0.1�� ����
            Debug.Log("SlowSpeed: " + player.speed); // ���� ���ǵ� ���
        }
        player.speed = 4f;
    }

    void FixedUpdate()
    {
        // �״� ��� �ľ�
        if (isDead)
        {
            return;
        }

        // �÷��̾� �̵�
        Move();
    }

    void Move()
    {
        if (!isJump)
        {
            float xInput = Input.GetAxis("Horizontal"); // ����
            float zInput = Input.GetAxis("Vertical"); // ����

            Vector3 movement = new Vector3(xInput, 0, zInput).normalized; // ����ȭ

            // ������ �ϰ� �ִ� ��쿡�� ȸ���� ����
            if (movement != Vector3.zero)
            {
                animator.SetBool("IsRun", true);

                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * player.speed);

                Vector3 newVelocity = transform.forward * player.speed; // �ӵ� ������ ���� transform.forward ���
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
            Debug.Log("FastSpeed: " + player.speed); // ���� ���ǵ� ���
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Trainer")) // �浹��
        {
            player.speed -= 1f;  // �ӵ� ����
            player.Hunger -= 5f; // ����� ����
        }
    }

    public void Slow()
    {
        animator.SetBool("IsWalk", true);

        Debug.Log("SlowSpeed: " + player.speed); // ���� ���ǵ� ���

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
















