using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    private Vector2 input;

    [SerializeField] float stepInterval = 0.2f;

    private bool isCucuk = false;
    public void OnMove(InputValue value)
    {
        if(isReleased(value)) {
            move();
            transform.localScale = Vector3.one;
            isCucuk = false;
        }


        input = value.Get<Vector2>();
        
        if(input.magnitude != 1) {
            input = Vector2.zero;
        } else {
            if(!isCucuk) {
                Vector3 scale = transform.localScale;
                scale.y /= 2; 
                transform.localScale = scale;

                isCucuk = true;
            }
            
        }
    }

    bool isReleased(InputValue value) {
        return value.Get<Vector2>() == Vector2.zero;
    }
    void move()
    {
        Vector3 move = new Vector3(input.x, 0, input.y);
        // diagonal varsa normalize et (opsiyonel ama önerilir)
        if (move.magnitude > 1)
            move = move.normalized;
        transform.position += move;
    }

    void Update() {
        
    }
}