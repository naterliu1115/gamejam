using UnityEngine;

public class Dropdown : MonoBehaviour
{
    public float fallSpeed = 3;
    public float destroyTime = 5;
    

    void Start()
    {
        
        Destroy(gameObject,destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

}
