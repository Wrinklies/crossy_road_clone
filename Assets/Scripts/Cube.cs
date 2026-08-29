using UnityEngine;
using UnityEngine.InputSystem;

public class Cube : MonoBehaviour
{
    [SerializeField] private float repeatDelay = 0.15f;
    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float hopHeight = 0.5f;

    private PlayerInputActions input;

    private float baseY;
    private float holdTimer;
    private float moveTimer;

    private bool isMoving;

    private Vector3 lastDirection;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        baseY = transform.position.y;
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        if (isMoving)
        {
            UpdateMovement();
            return;
        }

        HandleInput();
    }

    private void HandleInput()
    {
        Vector2 inputDirection = input.Player.Move.ReadValue<Vector2>();

        if (inputDirection == Vector2.zero)
        {
            holdTimer = 0f;
            lastDirection = Vector3.zero;
            return;
        }

        Vector3 direction = ToWorldDirection(inputDirection);

        // Move immediately when the direction changes.
        if (direction != lastDirection)
        {
            lastDirection = direction;
            holdTimer = 0f;

            StartMovement(direction);
            return;
        }

        // Move repeatedly while holding the key.
        holdTimer += Time.deltaTime;

        if (holdTimer >= repeatDelay)
        {
            holdTimer = 0f;
            StartMovement(direction);
        }
    }

    private void StartMovement(Vector3 direction)
    {
        isMoving = true;

        moveTimer = 0f;
        startPosition = transform.position;
        targetPosition = startPosition + direction;
    }

    private void UpdateMovement()
    {
        moveTimer += Time.deltaTime;

        float t = moveTimer / moveDuration;
        t = Mathf.Clamp01(t);

        float height = Mathf.Sin(t * Mathf.PI) * hopHeight;

        transform.position =
            Vector3.Lerp(startPosition, targetPosition, t)
            + Vector3.up * height;

        if (t >= 1f)
        {
            transform.position = new Vector3(
                targetPosition.x,
                baseY,
                targetPosition.z
            );

            isMoving = false;
        }
    }

    private Vector3 ToWorldDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            return input.x > 0 ? Vector3.right : Vector3.left;

        return input.y > 0 ? Vector3.forward : Vector3.back;
    }
}