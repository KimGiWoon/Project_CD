using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyBaseAttack
{
    [Header("Melee Attack Setting")]
    [SerializeField] private float _attackRadius = 0.7f;
    [SerializeField] private LayerMask _targetLayer;

    [Header("Attack Effect Setting")]
    [SerializeField] private GameObject _attackEffectPrefab;
    [SerializeField] private float _attackEffectDuration = 0.5f;

    // 공격 반경 프로퍼티
    public float AttackRadius => _attackRadius;

    // 이펙트 적용 확인
    private bool _applyEffect;

    public override void AttackExcute(EnemyController controller)
    {
        if (controller == null) return;

        // 공격 기준 위치
        Vector2 center = controller.transform.position;

        // 범위 내의 콜라이더 탐색
        Collider2D[] attackTarget = Physics2D.OverlapCircleAll(center, _attackRadius, _targetLayer);

        // 공격 범위안에 타겟이 없으면 공격하지 않음
        if (attackTarget == null || attackTarget.Length == 0)
        {
            return;
        }

        _applyEffect = false;

        // 공격 범위안에 있는 타겟 공격
        foreach (var hit in attackTarget)
        {
            // 공격할 수 있는 대상이면 공격
            if (hit.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(controller.AttackDamage);

                // 피격 이펙트가 있으면 한번만 생성
                if (_attackEffectPrefab != null && !_applyEffect)
                {
                    GameObject effect = Instantiate(_attackEffectPrefab, hit.transform.position, Quaternion.identity);
                    Destroy(effect, _attackEffectDuration);

                    _applyEffect = true;
                }
            }
        }

        // TODO: 애니메이션/사운드 연동
    }

    // 에디터에서 공격 범위 확인용
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}
