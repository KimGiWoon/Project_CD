using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    private Vector3 _originalPosition;
    private Coroutine _shakeRoutine;

    private void Awake()
    {
        // 현재 위치를 초기 위치로 설정
        _originalPosition = transform.localPosition;
    }

    // 카메라 쉐이크 처리
    public void CameraShake(float duration, float magnitude)
    {
        float camDuration = duration;
        float camMagnitude = magnitude;

        // 기존에 실행하던 코루틴이 있으면 정지
        if (_shakeRoutine != null)
        {
            StopCoroutine( _shakeRoutine );
            transform.localPosition = _originalPosition;
        }

        // 쉐이크 루틴 시작
        _shakeRoutine = StartCoroutine(CamShakeCoroutine(camDuration, camMagnitude));
    }

    // 카메라 쉐이크 루틴
    private IEnumerator CamShakeCoroutine(float duration, float magnitude)
    {
        float shakeTime = 0f;

        if (shakeTime < duration)
        {
            shakeTime += Time.deltaTime;

            // 원점 주변의 랜덤 흔들림 방향 설정
            float shakeDirX = Random.Range(-1f, 1f) * magnitude;
            float shakeDirY = Random.Range(-1f, 1f) * magnitude;
            Vector3 shakeDir = new Vector3(shakeDirX, shakeDirY, 0f);

            // 카메라 쉐이크 적용
            transform.localPosition = _originalPosition + shakeDir;

            yield return null;
        }

        // 쉐이크 후 원점 위치 복귀
        transform.localPosition = _originalPosition;
        _shakeRoutine = null;
    }
}
