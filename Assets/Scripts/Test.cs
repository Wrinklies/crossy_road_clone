using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform player;
    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y; // sahnede kamera hedefini nereye koyarsan o Y sabit kalır
    }

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = new Vector3(
            player.position.x,
            fixedY,
            player.position.z
        );
    }
}