using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "DataSO/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Weapon Data")]
    [SerializeField] private string _id;
    [SerializeField] private string _name;

    [Header("Weapon Option")]
    [SerializeField] private int _bulletDamage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _camShakeDuration;
    [SerializeField] private float _camShakeMagnitude;


    public string Id => _id;
    public string Name => _name;
    public int BulletDamage => _bulletDamage;
    public float BulletSpeed => _bulletSpeed;
    public float AttackDuration => _attackDuration;
    public float CamShakeDuration => _camShakeDuration;
    public float CamShakeMagnitude => _camShakeMagnitude;
}
