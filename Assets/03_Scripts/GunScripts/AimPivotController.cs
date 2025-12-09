using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPivotController : MonoBehaviour
{
    [Header("Aim Setting")]
    [SerializeField] private Camera _camera;
    [SerializeField] private SpriteRenderer _bodyRenderer;
    [SerializeField] private SpriteRenderer _gunRenderer;

    private float aimAngle;

    private void Awake()
    {
        if (_camera == null) _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera == null) return;

        // 마우스 좌표
        Vector3 mouseScreenPosition = Input.mousePosition;

        // 월드 좌표 변환
        Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = transform.position.z;

        // 피벗 중심점과 마우스 방향 확인
        Vector2 aimDir = (mouseWorldPosition - transform.position).normalized;
        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        // 에임 중심점 회전
        transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        // 스프라이트 플립 체인지
        GunFlipChange();
    }

    // 플립 체인지
    private void GunFlipChange()
    {
        bool flipLeft = aimAngle > 90f || aimAngle < -90f;

        // 플레이어 스프라이트 플립 좌우 반전
        _bodyRenderer.flipX = flipLeft;
        // 총 스프라이트 플립 좌우 반전
        _gunRenderer.flipY = flipLeft;
    }
}
