using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light directionalLight; // Directional Light
    public float dayDuration = 100f; // 하루 길이 

    private float currentTime = 0f;

    void Update()
    {
        // 하루 동안 경과한 시간 비율 계산 (0에서 1 사이)
        currentTime += Time.deltaTime;
        float timeRatio = currentTime / dayDuration;

        // 0에서 360도 회전
        float angle = timeRatio * 360f;

        // 오일러 회전 사용하여 라이트 회전
        Vector3 rotation = new Vector3(angle, 0, 0); // x축을 기준으로만 회전
        directionalLight.transform.rotation = Quaternion.Euler(rotation);

        // 하루가 끝나면 시간 초기화
        if (currentTime >= dayDuration)
        {
            currentTime = 0f;
        }
    }
}
