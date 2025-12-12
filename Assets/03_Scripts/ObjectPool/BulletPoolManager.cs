using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    // 싱글톤 설정
    public static BulletPoolManager Instance {  get; private set; }

    [Header("Player Bullet Pool Setting")]
    [SerializeField] private PlayerBulletController _playerBulletPrefab;
    [SerializeField] private int _playerInitialBulletCount;
    [SerializeField] protected bool _playerPoolExpandable;
    [SerializeField] protected Transform _playerBulletStorageBox;

    [Header("Enemy Bullet Pool Setting")]
    [SerializeField] private EnemyBulletController _enemyBulletPrefab;
    [SerializeField] private int _enemyInitialBulletCount;
    [SerializeField] protected bool _enemyPoolExpandable;
    [SerializeField] protected Transform _enemyBulletStorageBox;

    // 플레이어 총알 보관 큐
    private readonly Queue<PlayerBulletController> _playerBulletPool = new Queue<PlayerBulletController>();

    // 적 총알 보관 큐
    private readonly Queue<EnemyBulletController> _enemyBulletPool = new Queue<EnemyBulletController>();


    private void Awake()
    {
        _playerPoolExpandable = true;

        // BulletPoolManager가 만들어져 있고 자신이 아니라면 삭제
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 싱글톤 세팅
        Instance = this;

        // 초기 총알 생성
        InitialCreateBullet();
    }

    // 사용할 총알 미리 만들기
    private void InitialCreateBullet()
    {
        for (int i = 0; i < _playerInitialBulletCount; i++)
        {
            // 총알 만들어서 풀에 보관
            PlayerBulletController bullet = PlayerNewCreateBullet();
            PlayerReturnBullet(bullet);
        }

        for (int i = 0; i < _enemyInitialBulletCount; i++)
        {
            // 총알 만들어서 풀에 보관
            EnemyBulletController bullet = EnemyNewCreateBullet();
            EnemyReturnBullet(bullet);
        }
    }

    // 플레이어 새로운 총알 만들기
    private PlayerBulletController PlayerNewCreateBullet()
    {
        // 총알 만들고 비활성화 설정
        PlayerBulletController bullet = Instantiate(_playerBulletPrefab, _playerBulletStorageBox);
        bullet.gameObject.SetActive(false);

        // 반환 풀 전달
        bullet.SetPool(this);

        return bullet;
    }

    // 적 새로운 총알 만들기
    private EnemyBulletController EnemyNewCreateBullet()
    {
        // 총알 만들고 비활성화 설정
        EnemyBulletController bullet = Instantiate(_enemyBulletPrefab, _enemyBulletStorageBox);
        bullet.gameObject.SetActive(false);

        // 반환 풀 전달
        bullet.SetPool(this);

        return bullet;
    }

    // 플레이어 총알 꺼내오기
    public PlayerBulletController PlayerGetBullet()
    {
        // 풀에 보관한 총알이 없으면
        if (_playerBulletPool.Count == 0)
        {
            // 확장 가능 여부 확인
            if (_playerPoolExpandable)
            {
                // 총알 만들어서 반환
                return PlayerNewCreateBullet();
            }

            // 확장이 불가면 null 반환
            return null;
        }

        // 풀에서 총알 가져오기
        PlayerBulletController bullet = _playerBulletPool.Dequeue();
        // 총알 활성화
        bullet.gameObject.SetActive(true);
        // 총알 반환
        return bullet;
    }

    // 적 총알 꺼내오기
    public EnemyBulletController EnemyGetBullet()
    {
        // 풀에 보관한 총알이 없으면
        if (_enemyBulletPool.Count == 0)
        {
            // 확장 가능 여부 확인
            if (_enemyPoolExpandable)
            {
                // 총알 만들어서 반환
                return EnemyNewCreateBullet();
            }

            // 확장이 불가면 null 반환
            return null;
        }

        // 풀에서 총알 가져오기
        EnemyBulletController bullet = _enemyBulletPool.Dequeue();
        // 총알 활성화
        bullet.gameObject.SetActive(true);
        // 총알 반환
        return bullet;
    }

    // 플레이어 총알 반납
    public void PlayerReturnBullet(PlayerBulletController bullet)
    {
        // 총알 비활성화
        bullet.gameObject.SetActive(false);

        // 풀에 총알 반납
        _playerBulletPool.Enqueue(bullet);
    }

    // 적 총알 반납
    public void EnemyReturnBullet(EnemyBulletController bullet)
    {
        // 총알 비활성화
        bullet.gameObject.SetActive(false);

        // 풀에 총알 반납
        _enemyBulletPool.Enqueue(bullet);
    }
}
