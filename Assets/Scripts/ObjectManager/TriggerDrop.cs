using UnityEngine;

public class TriggerDrop : MonoBehaviour
{
    [Header("危險物品 Prefabs")]
    public GameObject[] dangerPrefabs;

    [Header("掉落設定")]
    public float dropInterval = 1f;         // 每幾秒掉一次
    public float dropHeight = 5f;
    public float horizontalRange = 3f;

    private bool playerInside = false;
    private float dropTimer = 0f;

    private void Update()
    {
        if (playerInside)
        {
            dropTimer += Time.deltaTime;
            if (dropTimer >= dropInterval)
            {
                DropDangerObject();
                dropTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            dropTimer = dropInterval; // 進來馬上掉一個
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            dropTimer = 0f;
        }
    }

    void DropDangerObject()
    {
        if (dangerPrefabs.Length == 0) return;

        GameObject prefab = dangerPrefabs[Random.Range(0, dangerPrefabs.Length)];

        float randomXOffset = Random.Range(-horizontalRange, horizontalRange);
        Vector3 spawnPosition = transform.position + new Vector3(randomXOffset, dropHeight, 0);

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
