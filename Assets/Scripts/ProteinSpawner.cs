using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinSpawner : MonoBehaviour
{
    public GameObject[] proteinPrefabs; // 프로틴 프리팹들을 저장할 배열
    public int count = 6; //  총 생성 개수
    public float minT = 2.5f;
    public float maxT = 5.5f;

    private GameObject[] proteinItems;

    // Start is called before the first frame update
    void Start()
    {
        proteinItems = new GameObject[count];

        for (int i = 0; i < count; ++i)
        {
            GameObject proteinPrefab = GetRandomProteinPrefab();
            GameObject protein = Instantiate(proteinPrefab, Vector3.zero, Quaternion.identity);
            protein.SetActive(false); // 아이템 비활성화
            proteinItems[i] = protein;
        }

        StartCoroutine(SpawnProteins());
    }

    // 코루틴 정의
    IEnumerator SpawnProteins()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minT, maxT));
            SpawnProtein();
        }
    }

    void SpawnProtein()
    {
        for (int i = 0; i < count; ++i)
        {
            // 랜덤 선택
            GameObject selectedPrefab = GetRandomProteinPrefab();

            proteinItems[i].SetActive(true); // 생성되었던 아이템 비활성화

            proteinItems[i].transform.position = RandomPosition(); // 랜덤 위치에 위치
            proteinItems[i].SetActive(true); // 활성화
            proteinItems[i].transform.rotation = Quaternion.Euler(-90f, 0f, 0f); // 90도로 회전하여 생성
        }
    }

    // 랜덤 위치 계산
    Vector3 RandomPosition()
    {
        Vector3 randomSpawn = new Vector3(Random.Range(-15f, 15f), 0.2f, Random.Range(-15f, 15f)); // y는 0보다 약간 높이 위치
        return randomSpawn;
    }

    // 무작위로 뽑기 
    GameObject GetRandomProteinPrefab()
    {
        return proteinPrefabs[Random.Range(0, proteinPrefabs.Length)];
    }
}



