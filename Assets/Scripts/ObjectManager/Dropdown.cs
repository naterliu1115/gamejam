using UnityEngine;

public class Dropdown : MonoBehaviour
{
    
    public float destroyTime = 5;
    
    void Start()
    {
        
        Destroy(gameObject,destroyTime);
    }

}
