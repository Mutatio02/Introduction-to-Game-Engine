using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> itemPrefabs; // 아이템 프리팹 리스트
    public float spawnInterval = 5f; // 아이템 생성 간격
    public float itemTime = 7f; // 아이템 생존 시간
    public float delayTime = 6f; // 맨 처음 딜레이 시간
    public float spawnRadius = 40f; // 아이템이 생성될 최대 반경

    void Start()
    {
        // 코루틴 
        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            // 아이템 생성
            SpawnItem();

            // 일정 간격 기다림
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(delayTime); // 원하는 시간동안 딜레이를 주고 시작

        // 아이템 생성 코루틴
        StartCoroutine(SpawnItems());
    }

    void SpawnItem()
    {
        // 랜덤한 아이템 프리팹 선택
        GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

        // 랜덤한 위치 계산
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // 아이템 생성
        GameObject item = Instantiate(randomItemPrefab, spawnPosition, Quaternion.identity);

        // 일정 시간이 지나면 아이템 삭제
        Destroy(item, itemTime);
    }

    // 랜덤한 위치 계산
    private Vector3 GetRandomSpawnPosition()
    {
        // 임의의 구 부터 생성반경 40
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        randomPos.y = 0; // Y위치는 0

        return randomPos;
    }

}
