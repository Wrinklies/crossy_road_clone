using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    public enum MoveDirection
    {
        None,
        Right,
        Left,
        Forward,
        Backward
    }

    private Vector2 input;
    private MoveDirection currentDir = MoveDirection.None;

    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float hopHeight = 0.5f;

    [SerializeField] private float squeezeDuration = 0.3f;
    [SerializeField] private float bufferTime = 0.3f;

    [SerializeField] private Vector2 squeezeScale = new Vector2(1.2f, 0.5f);

    private bool isMoving = false;
    private Coroutine squeezeCoroutine;

    private bool hasBufferedMove = false;
    private Vector2 bufferedInput;
    private float bufferedTime;

    void OnForward(InputValue value)
    {
        if (value.isPressed)
            Press(MoveDirection.Forward, Vector2.up);
        else
            Release(MoveDirection.Forward);
    }

    void OnBackward(InputValue value)
    {
        if (value.isPressed)
            Press(MoveDirection.Backward, Vector2.down);
        else
            Release(MoveDirection.Backward);
    }

    void OnLeft(InputValue value)
    {
        if (value.isPressed)
            Press(MoveDirection.Left, Vector2.left);
        else
            Release(MoveDirection.Left);
    }

    void OnRight(InputValue value)
    {
        if (value.isPressed)
            Press(MoveDirection.Right, Vector2.right);
        else
            Release(MoveDirection.Right);
    }

    void Press(MoveDirection direction, Vector2 directionInput)
    {
        if (currentDir != MoveDirection.None)
            return;

        currentDir = direction;
        input = directionInput;

        StartSqueeze();
    }

    void Release(MoveDirection direction)
    {
        if (currentDir != direction)
            return;

        currentDir = MoveDirection.None;

        StopSqueeze();

        if (isMoving)
        {
            // Store the move so it fires as soon as the current hop ends.
            bufferedInput = input;
            bufferedTime = Time.time;
            hasBufferedMove = true;
            return;
        }

        move();
    }

    void move()
    {
        Vector3 direction = new Vector3(input.x, 0, input.y);

        StartCoroutine(MoveTo(direction));
    }

    void StartSqueeze()
    {
        if (squeezeCoroutine != null)
            StopCoroutine(squeezeCoroutine);

        squeezeCoroutine = StartCoroutine(Squeeze());
    }

    void StopSqueeze()
    {
        if (squeezeCoroutine != null)
            StopCoroutine(squeezeCoroutine);

        squeezeCoroutine = StartCoroutine(Unsqueeze());
    }

    IEnumerator Squeeze()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(
            squeezeScale.x,
            squeezeScale.y,
            startScale.z
        );

        float elapsed = 0f;

        while (elapsed < squeezeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / squeezeDuration;

            transform.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                t
            );

            yield return null;
        }

        transform.localScale = targetScale;
    }

    IEnumerator Unsqueeze()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.one;

        float elapsed = 0f;

        while (elapsed < squeezeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / squeezeDuration;

            transform.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                t
            );

            yield return null;
        }

        transform.localScale = targetScale;
    }

    IEnumerator MoveTo(Vector3 direction)
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 targetPosition = startPosition + direction;
        Quaternion targetRotation = getTargetRotation(direction);

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / moveDuration;

            Vector3 position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            Quaternion rotation = Quaternion.Lerp(
                startRotation,
                targetRotation,
                t
            );

            position.y += Mathf.Sin(t * Mathf.PI) * hopHeight;

            transform.position = position;
            transform.rotation = rotation;
            yield return null;
        }
        transform.position = targetPosition;
        transform.rotation = targetRotation;

        isMoving = false;

        if (hasBufferedMove && (Time.time - bufferedTime <= bufferTime))
        {
            input = bufferedInput;
            move();
        }

        hasBufferedMove = false;
    }

    private Quaternion getTargetRotation(Vector3 direction)
    {
        if (direction == Vector3.forward)
        {
            Debug.Log("FORWARD");
            return Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector3.back)
        {
            Debug.Log("BACKWARD");
            return Quaternion.Euler(0, 180, 0);
        }
        else if (direction == Vector3.right)
        {
            Debug.Log("RIGHT");
            return Quaternion.Euler(0, 90, 0);
        }
        else
        {
            Debug.Log("LEFT");
            return Quaternion.Euler(0, -90, 0);
        }
    }

    void Update()
    {
    }
}