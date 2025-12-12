using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : EnemyBaseAttack
{
    [Header("Ranged Attack Setting")]
    [SerializeField] private float _attackRadius = 2f;
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

        // 법위 내의 콜라이더 탐색
        Collider2D[] attackTarget = Physics2D.OverlapCircleAll(center, AttackRadius, _targetLayer);
    }
}
