using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> itemPrefabs; // ������ ������ ����Ʈ
    public float spawnInterval = 5f; // ������ ���� ����
    public float itemTime = 7f; // ������ ���� �ð�
    public float delayTime = 6f; // �� ó�� ������ �ð�
    public float spawnRadius = 40f; // �������� ������ �ִ� �ݰ�

    void Start()
    {
        // �ڷ�ƾ 
        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            // ������ ����
            SpawnItem();

            // ���� ���� ��ٸ�
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(delayTime); // ���ϴ� �ð����� �����̸� �ְ� ����

        // ������ ���� �ڷ�ƾ
        StartCoroutine(SpawnItems());
    }

    void SpawnItem()
    {
        // ������ ������ ������ ����
        GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

        // ������ ��ġ ���
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // ������ ����
        GameObject item = Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity);

        // ���� �ð��� ������ ������ ����
        Destroy(item, itemTime);
    }

    // ������ ��ġ ���
    private Vector3 GetRandomSpawnPosition()
    {
        // ������ �� ���� �����ݰ� 40
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        randomPos.y = 0; // Y��ġ�� 0

        return randomPos;
    }

}
