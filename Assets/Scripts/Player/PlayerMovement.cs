using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Player player;

    private Vector2 velocity;

    private float inputAxis;
    private float verticalAxis;

    private bool runInput;

    private float defaultGravityScale;

    public float moveSpeed = 8f;
    public float runSpeed = 12f;
    public float runJumpMultiplier = 1.25f;

    [Header("Underwater")]
    public bool underwater;
    public string waterZoneTag = "WaterZone";

    public float underwaterSpeed = 4f;
    public float swimForce = 5f;
    public float swimAcceleration = 8f;
    public float underwaterGravity = 0.25f;

    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);

    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }

    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;

    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

    public bool falling => velocity.y < 0f && !grounded;

    private void Awake()
    {
        camera = Camera.main;

        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        player = GetComponent<Player>();

        defaultGravityScale = rigidbody.gravityScale;
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = false;

        collider.enabled = true;

        velocity = Vector2.zero;

        jumping = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;

        collider.enabled = false;

        velocity = Vector2.zero;

        jumping = false;
    }

    private void Update()
    {
        // INPUTS (WSAD / Flechas)
        inputAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        runInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        grounded = rigidbody.Raycast(Vector2.down);

        // MOVIMIENTO HORIZONTAL
        HorizontalMovement();

        // COMPORTAMIENTO BAJO EL AGUA
        if (underwater)
        {
            UnderwaterMovement();

            if (Input.GetButtonDown("Jump"))
            {
                Swim();
            }
        }
        // MOVIMIENTO NORMAL EN TIERRA
        else if (grounded)
        {
            GroundedMovement();
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;

        position += velocity * Time.fixedDeltaTime;

        // Limitar cámara
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        position.x = Mathf.Clamp(
            position.x,
            leftEdge.x + 0.5f,
            rightEdge.x - 0.5f
        );

        rigidbody.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        float targetSpeed = underwater ? underwaterSpeed : (runInput ? runSpeed : moveSpeed);

        // CORRECCIÓN: Si está bajo el agua, usamos 'swimAcceleration' para vencer la fricción fluida.
        float acceleration = underwater ? swimAcceleration : targetSpeed;

        velocity.x = Mathf.MoveTowards(
            velocity.x,
            inputAxis * targetSpeed,
            acceleration * Time.deltaTime
        );

        // Pared
        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        // Girar sprite
        if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void UnderwaterMovement()
    {
        // Movimiento vertical subacuático (W/S)
        velocity.y += verticalAxis * swimAcceleration * Time.deltaTime;

        // Limitar la velocidad máxima permitida en el agua
        velocity.x = Mathf.Clamp(velocity.x, -underwaterSpeed, underwaterSpeed);
        velocity.y = Mathf.Clamp(velocity.y, -underwaterSpeed, underwaterSpeed);

        // CORRECCIÓN: Resistencia del agua controlada. 
        // Solo reduce la velocidad de forma drástica si dejas de presionar las teclas correspondientes.
        if (Mathf.Abs(inputAxis) < 0.01f)
        {
            velocity.x *= 0.9f; 
        }
        
        if (Mathf.Abs(verticalAxis) < 0.01f)
        {
            velocity.y *= 0.9f;
        }
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);

        jumping = velocity.y > 0f;

        if (Input.GetButtonDown("Jump"))
        {
            float jumpMultiplier = runInput ? runJumpMultiplier : 1f;

            velocity.y = jumpForce * jumpMultiplier;

            jumping = true;

            // Sonidos
            if (player.big)
            {
                player.soundController.PlaySaltoGrandeSound();
            }
            else
            {
                player.soundController.PlaySaltoPequenoSound();
            }
        }
    }

    private void ApplyGravity()
    {
        // Gravedad submarina
        if (underwater)
        {
            velocity.y += gravity * underwaterGravity * Time.deltaTime;

            velocity.y = Mathf.Clamp(
                velocity.y,
                -underwaterSpeed,
                underwaterSpeed
            );

            return;
        }

        // Gravedad normal
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");

        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;

        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void Swim()
    {
        velocity.y = swimForce;

        jumping = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            collision.CompareTag(waterZoneTag) ||
            collision.gameObject.layer == LayerMask.NameToLayer(waterZoneTag)
        )
        {
            underwater = true;
            rigidbody.gravityScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (
            collision.CompareTag(waterZoneTag) ||
            collision.gameObject.layer == LayerMask.NameToLayer(waterZoneTag)
        )
        {
            underwater = false;
            rigidbody.gravityScale = defaultGravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Rebote enemigo
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;

                jumping = true;

                if (player.big)
                {
                    player.soundController.PlaySaltoGrandeSound();
                }
                else
                {
                    player.soundController.PlaySaltoPequenoSound();
                }
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            // Golpe cabeza
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }
}