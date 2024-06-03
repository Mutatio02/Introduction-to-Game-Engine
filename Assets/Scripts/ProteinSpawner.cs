using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinSpawner : MonoBehaviour
{
    public GameObject[] proteinPrefabs; // ����ƾ �����յ��� ������ �迭
    public int count = 6; //  �� ���� ����
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
            protein.SetActive(false); // ������ ��Ȱ��ȭ
            proteinItems[i] = protein;
        }

        StartCoroutine(SpawnProteins());
    }

    // �ڷ�ƾ ����
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
            // ���� ����
            GameObject selectedPrefab = GetRandomProteinPrefab();

            proteinItems[i].SetActive(true); // �����Ǿ��� ������ ��Ȱ��ȭ

            proteinItems[i].transform.position = RandomPosition(); // ���� ��ġ�� ��ġ
            proteinItems[i].SetActive(true); // Ȱ��ȭ
            proteinItems[i].transform.rotation = Quaternion.Euler(-90f, 0f, 0f); // 90���� ȸ���Ͽ� ����
        }
    }

    // ���� ��ġ ���
    Vector3 RandomPosition()
    {
        Vector3 randomSpawn = new Vector3(Random.Range(-15f, 15f), 0.2f, Random.Range(-15f, 15f)); // y�� 0���� �ణ ���� ��ġ
        return randomSpawn;
    }

    // �������� �̱� 
    GameObject GetRandomProteinPrefab()
    {
        return proteinPrefabs[Random.Range(0, proteinPrefabs.Length)];
    }
}



