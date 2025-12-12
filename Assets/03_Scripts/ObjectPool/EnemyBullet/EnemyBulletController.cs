using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [Header("Bullet Setting")]
    [SerializeField] private float _bulletLifeTime = 1f;

    [Header("Attack Effect Setting")]
    [SerializeField] private GameObject _attackEffectPrefab;
    [SerializeField] private float _attackEffectDuration = 0.5f;

    private bool _applyEffect;
    private Vector2 _shootDirection;
    private float _bulletSpeed;
    private int _bulletDamage;
    private float _timer;

    private BulletPoolManager _enemyBulletPool;

    // 반환 풀 지정
    public void SetPool(BulletPoolManager bulletPool)
    {
        _enemyBulletPool = bulletPool;
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
            _enemyBulletPool.EnemyReturnBullet(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _applyEffect = false;

        // 총알에 맞은 대상이 공격 받을 수 있는 대상이면
        if (collision.TryGetComponent<IDamagable>(out var damagable))
        {
            // 대상에 데미지 주기
            damagable.TakeDamage(_bulletDamage);

            // 피격 이펙트는 한번만 생성
            if (_attackEffectPrefab != null && !_applyEffect)
            {
                GameObject effect = Instantiate(_attackEffectPrefab, collision.transform.position, Quaternion.identity);
                Destroy(effect, _attackEffectDuration);

                _applyEffect = true;
            }
        }

        // 충돌 후 총알 반환
        _enemyBulletPool?.EnemyReturnBullet(this);
    }
}
