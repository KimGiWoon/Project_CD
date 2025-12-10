using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "DataSO/Enemy_Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Enemy Data")]
    [SerializeField] private string _id;
    [SerializeField] private string _name;

    [Header("Enemy Stat Setting")]
    [SerializeField] private int _maxHp;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _traceRange;
    [SerializeField] private EnemyType _enemyType;

    public string Id => _id;
    public string Name => _name;
    public int MaxHp => _maxHp;
    public float MoveSpeed => _moveSpeed;
    public float AttackDamage => _attackDamage;
    public float AttackRange => _attackRange;
    public float TraceRange => _traceRange;
    public EnemyType EnemyType => _enemyType;

    // CSV 파싱 데이터 세팅
    public void ApplyCSVdata(string id, string name, int maxHp, float moveSpeed, float attackDamage, float attackRange, float traceRange, EnemyType type)
    {
        _id = id;
        _name = name;
        _maxHp = maxHp;
        _moveSpeed = moveSpeed;
        _attackRange = attackRange;
        _attackDamage = attackDamage;
        _traceRange = traceRange;
        _enemyType = type;
    }
}                   
