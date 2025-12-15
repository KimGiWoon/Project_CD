using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("Room Info")]
    [SerializeField] private string _roomId;
    [SerializeField] private bool _enemySpawnOn = true;

    [Header("Player Detect")]
    [SerializeField] private string _playerTag = "Player";

    [Header("Enemy Spawn Setting")]
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private List<Transform> _enemySpawnList = new List<Transform>();

    [Header("Door Interaction Setting")]
    [SerializeField] private GameObject _doorTriggerBlock;

    private readonly List<EnemyController> _spawnEnemys = new List<EnemyController>();

    private bool _isRoomActivate;
    private bool _isRoomClear;
    private Collider2D _enterTrigger;

    private void Awake()
    {
        _enterTrigger = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // 방이 활성화 되어 있지 않거나 클리어하면 실행하지 않음
        if (!_isRoomActivate || _isRoomClear) return;

        // 방에 생성된 적이 모두 죽었는지 확인
        if (AllEnemyDeadCheck())
        {
            // 적이 모두 죽었으면 클리어
            RoomClear();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 몬스터 스폰이 꺼져있고 이미 룸에 진입했으며 플레이어가 아니면 실행하지 않음
        if (!_enemySpawnOn || _isRoomActivate || !collision.CompareTag("Player")) return;

        // 방 활성화
        ActivateRoom();
    }

    // 방 활성화, 전투 시작
    public void ActivateRoom()
    {
        // 이미 방이 활성화가 되어 있으면 실행하지 않음
        if (_isRoomActivate) return;

        _isRoomActivate = true;

        // 문 닫기
        if (_doorTriggerBlock != null)
        {
            _doorTriggerBlock.SetActive(true);
        }

        // 몬스터 생성
        SpawnEnemies();
    }

    // 몬스터 생성
    private void SpawnEnemies()
    {
        // 리스트에 몬스터가 있을 시 삭제
        _spawnEnemys.Clear();

        // 스폰 포인트를 순회
        foreach(var spawnPoint in _enemySpawnList)
        {
            if (spawnPoint == null || _enemyPrefab == null) continue;

            // 스폰 포인트에 몬스터 생성
            EnemyController enemy = Instantiate(_enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // 생성된 몬스터 리스트에 해당 몬스터 저장
            _spawnEnemys.Add(enemy);
        }
    }

    // 몬스터의 사망 확인
    private bool AllEnemyDeadCheck()
    {
        // 적의 상태 확인
        for(int i = 0; i < _spawnEnemys.Count; i++)
        {
            EnemyController enemy = _spawnEnemys[i];

            // 적이 살아있으면
            if (enemy != null && !enemy.IsDead) return false;
        }

        // 적이 모두 죽었으면
        return true;
    }

    // 방 클리어
    private void RoomClear()
    {
        _isRoomClear = true;

        // 문 열기
        if (_doorTriggerBlock != null)
        {
            _doorTriggerBlock.SetActive(false);
        }

        Debug.Log($"Room-{_roomId} Clear");
    }
}
