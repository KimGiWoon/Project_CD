using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunController : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponDataSO _weaponData;

    [Header("Bullet Setting")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;

    [Header("Camera Shake Setting")]
    [SerializeField] private CameraShakeController _cameraShakeController;

    [Header("Muzzle Flash Setting")]
    [SerializeField] private GameObject _muzzleFlashPrefab;
    [SerializeField] private float _flashDuration = 0.05f;

    // 무기의 공격 딜레이 시간
    public float AttackDelay => _weaponData.AttackDuration;

    private void Awake()
    {
        if (_firePoint == null)
        {
            Debug.Log("GunController의 FirePoint가 참조되어 있지 않습니다.");
        }

        // 카메라 컨트롤러가 참조가 안되어 있으면 찾아서 넣기
        if (_cameraShakeController == null)
        {
            _cameraShakeController = FindObjectOfType<CameraShakeController>();
        }
    }

    //private void Update()
    //{
    //    GunAimTowardMouse();
    //}

    //// 총 마우스 방향 회전
    //private void GunAimTowardMouse()
    //{
    //    if (Camera.main == null) return;

    //    // 마우스 방향 계산
    //    Vector2 mouseDir = GetMouseDirection();

    //    // 마우스 방향으로 각도 변환
    //    float gunAngle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

    //    // Z축 회전
    //    transform.rotation = Quaternion.Euler(0f, 0f, gunAngle);
    //}

    // 총알 발사 입력
    public void BulletFireInput()
    {
        // 총알 프리팹, 무기 데이터가 없거나 발사 포인트가 없으면 실행하지 않음
        if(_bulletPrefab == null || _firePoint == null || _weaponData == null) return;

        // 발사 방향 계산
        Vector2 fireDir = GetMouseDirection();

        // 총알 생성
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

        // 총알 공격력 초기화
        if (bullet.TryGetComponent<BulletController>(out var bulletDamage))
        {
            bulletDamage.Init(fireDir, _weaponData.BulletSpeed, _weaponData.BulletDamage);
        }

        // 총구의 플래시 처리
        GunMuzzleFlash(fireDir);

        // 카메라 흔들림 처리
        if (_cameraShakeController != null)
        {
            // 카메라 흔들림 설정
            _cameraShakeController.CameraShake(_weaponData.CamShakeDuration, _weaponData.CamShakeMagnitude);
        }
    }

    // 마우스 위치 전달
    private Vector2 GetMouseDirection()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = _firePoint.position.z;

        // 발사 방향 계산 후 전달
        return (mouseWorldPosition - _firePoint.position).normalized;
    }

    // 발사에 대한 총구의 플래시
    private void GunMuzzleFlash(Vector2 dir)
    {
        if (_muzzleFlashPrefab == null || _firePoint == null) return;

        // 플래시 생성 위치를 총구 방향으로 설정
        Quaternion flashRotation = Quaternion.FromToRotation(Vector2.right, dir);
        GameObject flash = Instantiate(_muzzleFlashPrefab, _firePoint.position, flashRotation);

        // 일정 시간 뒤 파괴
        Destroy(flash, _flashDuration);
    }
}
