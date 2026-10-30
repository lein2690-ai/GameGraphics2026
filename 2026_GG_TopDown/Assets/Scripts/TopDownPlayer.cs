using UnityEngine;
using UnityEngine.InputSystem;
public class TopDownPlayer : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float frameTime = 0.15f;
    public Sprite[] downSprites;
    public Sprite[] upSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;
    private Vector2 moveInput;
    private Vector2 facingDir = Vector2.down;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float frameTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput != Vector2.zero) { facingDir = moveInput; }
    }
    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        if (facingDir.y > 0) { currentSprites = upSprites; }
        else if (facingDir.y < 0) { currentSprites = downSprites; }
        else if (facingDir.x < 0) { currentSprites = leftSprites; }
        else { currentSprites = rightSprites; }
        if (moveInput != Vector2.zero)
        {
            frameTimer -= Time.deltaTime;
            if (frameTimer <= 0f)
            {
                frameTimer = frameTime;
                frameIndex += 1;
                if (frameIndex >= currentSprites.Length) { frameIndex = 0; }
            }
        }
        else { frameIndex = 0; }
        sr.sprite = currentSprites[frameIndex];
    }
}