using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;   // using 줄에 추가

using TMPro;   // 기존 using 두 줄 아래에 추가

public class PlayerController : MonoBehaviour
{
    public string playerName = "Player";
    public int hp = 100;
    public float moveSpeed = 5f;
    public float jumpPower = 8f;
    public Vector3 startPosition;
    public Transform visual;
    public float fallLimit = -10f;
    public int lives = 3;
    public Vector2 airScale = new Vector2(0.8f, 1.2f);
    private float facing = 1f;
    private bool isGrounded = false;
    private bool isGameOver = false;
    public TMP_Text livesText;
    public GameObject gameOverPanel;
    public string nextSceneName = "Stage2";


    private Vector2 moveInput;

    private Rigidbody2D rb;
    void Start()
    {
        if (hp >= 70)
        {
            Debug.Log("건강");
        }
        else if (hp >= 30)
        {
            Debug.Log("주의");
        }
        else
        {
            Debug.Log("위험");
        }

        rb = GetComponent<Rigidbody2D>();
        transform.position = startPosition;
        livesText.text = "LIFE " + lives;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
          //Debug.Log("착지");
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
          //Debug.Log("공중");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Update의 나머지는 그대로
        if (other.CompareTag("Goal"))
        {
            SceneManager.LoadScene(nextSceneName);

        }
        else if (other.CompareTag("Trap"))
        {
            LoseLife();
        }
    }
    void LoseLife()
        {
            lives -= 1;
            transform.position = startPosition;
            rb.linearVelocity = Vector2.zero;
        livesText.text = "LIFE " + lives;
        if (lives <= 0)
        {
            isGameOver = true;
            gameOverPanel.SetActive(true);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput.x > 0) { facing = 1f; }
        else if (moveInput.x < 0) { facing = -1f; }

        Debug.Log("입력" + moveInput);
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded && !isGameOver)
        {
            Debug.Log("점프!");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
    }


    void Update()
    {
        if (!isGameOver)
        {
            transform.Translate(Vector3.right * moveInput.x
                    * moveSpeed * Time.deltaTime);

            // 아래 낙사 if 블록은 그대로
            if (transform.position.y < fallLimit)
            {
                LoseLife();
            }
            //Debug.Log("Update");
        }
        // 현재 인스펙터에 설정된 visual 오브젝트의 고유 Y, Z 크기값을 가져옵니다.
        float currentScaleY = transform.Find("Visual") != null ? transform.Find("Visual").localScale.y : 1f;
        float currentScaleZ = transform.Find("Visual") != null ? transform.Find("Visual").localScale.z : 1f;

        // 만약 수동으로 크기를 정하고 싶다면 아래의 1f 자리에 원하는 크기 배율(예: 3f 등)을 직접 적으셔도 됩니다.
        float myCharacterSize = 7.4131f;

        if (isGrounded)
        {
            visual.localScale = new Vector3(facing * myCharacterSize, myCharacterSize, currentScaleZ);
        }
        else
        {
            visual.localScale = new Vector3(facing * airScale.x * myCharacterSize, airScale.y * myCharacterSize, currentScaleZ);
        }
    }
    }