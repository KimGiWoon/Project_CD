using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyBaseAttack
{
    [Header("Melee Attack Setting")]
    [SerializeField] private LayerMask _targetLayer;

    [Header("Attack Effect Setting")]
    [SerializeField] private GameObject _attackEffectPrefab;
    [SerializeField] private float _attackEffectDuration = 0.5f;

    // 이펙트 적용 확인
    private bool _applyEffect;

    public override void AttackExcute(EnemyController controller)
    {
        if (controller == null) return;

        // 공격 기준 위치
        Vector2 center = controller.transform.position;

        // 범위 내의 콜라이더 탐색
        Collider2D[] attackTarget = Physics2D.OverlapCircleAll(center, controller.AttackRange, _targetLayer);

        _applyEffect = false;

        // 공격 범위안에 있는 타겟 공격
        foreach (var hit in attackTarget)
        {
            // 공격할 수 있는 대상이면 공격
            if (hit.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(controller.AttackDamage);

                // 피격 이펙트는 한번만 생성
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
}
