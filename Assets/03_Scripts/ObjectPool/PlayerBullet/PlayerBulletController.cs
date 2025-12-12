using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    [Header("Bullet Setting")]
    [SerializeField] private float _bulletLifeTime = 1f;

    private Vector2 _shootDirection;
    private float _bulletSpeed;
    private int _bulletDamage;
    private float _timer;

    private BulletPoolManager _playerBulletPool;

    // 반환 풀 지정
    public void SetPool(BulletPoolManager bulletPool)
    {
        _playerBulletPool = bulletPool;
    }

    // 총알 생성시 초기화
    public void Init(Vector2 direction, float speed, int damage)
    {
        _shootDirection = direction.normalized;
        _bulletSpeed = speed;
        _bulletDamage = damage;
        _timer = 0f;

        transform.up = _shootDirection;
    }

    private void Update()
    {
        // 공격 방향으로 총알 이동
        transform.position += (Vector3)(_shootDirection * _bulletSpeed * Time.deltaTime);

        _timer += Time.deltaTime;

        if( _timer >= _bulletLifeTime)
        {
            // 생성 시간 지나면 총알 반납
            _playerBulletPool.PlayerReturnBullet(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알에 맞은 대상이 공격 받을 수 있는 대상이면
        if (collision.TryGetComponent<IDamagable>(out var damagable))
        {
            // 대상에 데미지 주기
            damagable.TakeDamage(_bulletDamage);
        }

        // 충돌 후 총알 반환
        _playerBulletPool?.PlayerReturnBullet(this);
    }
}
