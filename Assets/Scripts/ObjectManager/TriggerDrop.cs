using UnityEngine;

public class TriggerDrop : MonoBehaviour
{
    [Header("危險物品 Prefabs")]
    public GameObject[] dangerPrefabs;

    [Header("掉落設定")]
    public int dropCount = 3;
    public float dropHeight = 5f;
    public float horizontalRange = 3f;

    [Header("只觸發一次？")]
    public bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && triggerOnce) return;

        if (other.CompareTag("Player")) // 玩家進入
        {
            hasTriggered = true;
            DropDangerObjects();
        }
    }

    void DropDangerObjects()
    {
        for (int i = 0; i < dropCount; i++)
        {
            if (dangerPrefabs.Length == 0) return;

            GameObject prefab = dangerPrefabs[Random.Range(0, dangerPrefabs.Length)];

            float randomXOffset = Random.Range(-horizontalRange, horizontalRange);
            Vector3 spawnPosition = transform.position + new Vector3(randomXOffset, dropHeight, 0);

            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}
