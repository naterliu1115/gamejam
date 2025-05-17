using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public float hp = 1;
    public float destroyDelay = 0.1f;
    public GameObject destoryEffect;
    public AudioClip destroySound;
    public AudioSource audioSource;

    private bool isDestroyed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroyed && hp <= 0)
        {
            isDestroyed = true;
            if (destoryEffect != null)
                Instantiate(destoryEffect, transform.position, Quaternion.identity);

            if (destroySound != null)
                AudioSource.PlayClipAtPoint(destroySound, transform.position);

            Destroy(gameObject, destroyDelay);
        }
        
    }
}
