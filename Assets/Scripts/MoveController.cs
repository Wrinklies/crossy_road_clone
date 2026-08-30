using System.Net.NetworkInformation;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    public enum MoveDirection {
        None,
        Right,
        Left,
        Forward,
        Backward
    }
    private Vector2 input;
    private MoveDirection currentDir = MoveDirection.None;
    void OnForward(InputValue value) {
        if(currentDir == MoveDirection.None) {
            currentDir = MoveDirection.Forward;
            input = Vector2.up;
            doSqueeze();
        }
        if(currentDir == MoveDirection.Forward && !value.isPressed) {
            move();
            currentDir = MoveDirection.None;
            undoSqueeze();
        }
    }
    void OnBackward(InputValue value) {
        if(currentDir == MoveDirection.None) {
            currentDir = MoveDirection.Backward;
            input = Vector2.down;
            doSqueeze();
        }
        if(currentDir == MoveDirection.Backward && !value.isPressed) {
            move();
            currentDir = MoveDirection.None;
            undoSqueeze();
        }
    }
    void OnLeft(InputValue value) {
        if(currentDir == MoveDirection.None) {
            currentDir = MoveDirection.Left;
            input = Vector2.left;
            doSqueeze();
        }
        if(currentDir == MoveDirection.Left && !value.isPressed) {
            move();
            currentDir = MoveDirection.None;
            undoSqueeze();
        }
    }
    void OnRight(InputValue value) {
        if(currentDir == MoveDirection.None) {
            currentDir = MoveDirection.Right;
            input = Vector2.right;
            doSqueeze();
        }
        if(currentDir == MoveDirection.Right && !value.isPressed) {
            move();
            currentDir = MoveDirection.None;
            undoSqueeze();
        }
    }
    void move()
    {
        Vector3 move = new Vector3(input.x, 0, input.y);
        // diagonal varsa normalize et (opsiyonel ama önerilir)
        if (move.magnitude > 1)
            move = move.normalized;
        transform.position += move;
    }
    void doSqueeze() {
        Vector3 scale = transform.localScale;
        scale.y /= 2; 
        transform.localScale = scale;
    }

    void undoSqueeze() {
        transform.localScale = Vector3.one;
    }
    void Update() {
        
    }
}