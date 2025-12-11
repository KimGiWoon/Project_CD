using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    // 최대 체력
    [SerializeField] private int _maxHp = 6;
    public int MaxHp => _maxHp;

    // 현재 체력
    [SerializeField] private int _currentHp;
    public int CurrentHp 
    { 
        get => _currentHp; 
        set => _currentHp = Mathf.Clamp(value, 0, _maxHp); 
    } 

    // 이동 속도
    [SerializeField] private float _moveSpeed = 5f;
    public float MoveSpeed => _moveSpeed;

    // 죽음 여부
    public bool IsDead => CurrentHp <= 0;

    // 초기화
    public void Init()
    {
        CurrentHp = _maxHp;
    }
}
