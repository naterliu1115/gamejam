using UnityEngine;

public class Dropdown : MonoBehaviour
{
    publicb float fallSpeed = 3;
    public float destroyTime = 5;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject,destroyTime);
    }

    void Update()
    {
        transform.position = Vector2.down;
    }

}
