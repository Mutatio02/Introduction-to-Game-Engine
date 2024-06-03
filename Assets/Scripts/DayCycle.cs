using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light directionalLight; // Directional Light
    public float dayDuration = 100f; // �Ϸ� ���� 

    private float currentTime = 0f;

    void Update()
    {
        // �Ϸ� ���� ����� �ð� ���� ��� (0���� 1 ����)
        currentTime += Time.deltaTime;
        float timeRatio = currentTime / dayDuration;

        // 0���� 360�� ȸ��
        float angle = timeRatio * 360f;

        // ���Ϸ� ȸ�� ����Ͽ� ����Ʈ ȸ��
        Vector3 rotation = new Vector3(angle, 0, 0); // x���� �������θ� ȸ��
        directionalLight.transform.rotation = Quaternion.Euler(rotation);

        // �Ϸ簡 ������ �ð� �ʱ�ȭ
        if (currentTime >= dayDuration)
        {
            currentTime = 0f;
        }
    }
}
