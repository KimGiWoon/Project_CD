using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : EnemyBaseAttack
{
    [Header("Ranged Attack Setting")]
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private Transform _firePoint;

    public override void AttackExcute(EnemyController controller)
    {
        if (controller == null) return;

        // 공격 기준 위치
        Vector2 center = _firePoint.position;

        // 법위 내의 콜라이더 탐색
        Collider2D[] attackTarget = Physics2D.OverlapCircleAll(center, controller.AttackRange, _targetLayer);

        // 공격 범위안에 있는 타겟 공격
        foreach (var hit in attackTarget)
        {
            // 공격할 수 있는 대상이면 공격
            if (hit.TryGetComponent<IDamagable>(out var damagable))
            {
                // 타겟 위치
                Vector2 targetPos = hit.transform.position;

                // 발사 방향
                Vector2 fireDir = (targetPos - center).normalized;

                // 풀에서 총알 꺼내오기
                EnemyBulletController bullet = BulletPoolManager.Instance.EnemyGetBullet();

                // 총알이 없으면 실행하지 않음
                if (bullet == null) return;

                // 총알의 위치와 회전 세팅
                bullet.transform.position = center;
                bullet.transform.rotation = Quaternion.FromToRotation(Vector2.right, fireDir);

                // 총알 초기화
                bullet.Init(fireDir, 10f, controller.AttackDamage);
            }
        }
    }
}
