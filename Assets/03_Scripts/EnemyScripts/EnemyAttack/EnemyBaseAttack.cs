using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseAttack : MonoBehaviour
{
    // 공격 실행
    public abstract void AttackExcute(EnemyController controller);
}
