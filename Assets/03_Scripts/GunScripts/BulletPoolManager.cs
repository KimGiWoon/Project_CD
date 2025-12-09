using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    // 싱글톤 설정
    public static BulletPoolManager Instance {  get; private set; }

    [Header("Bullet Pool Setting")]
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private int _InitialBulletCount;
    [SerializeField] protected bool _poolExpandable;
    [SerializeField] protected Transform _bulletStorageBox;

    // 총알 보관 큐
    private readonly Queue<BulletController> _bulletPool = new Queue<BulletController>();

    private void Awake()
    {
        _poolExpandable = true;

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
        for (int i = 0; i < _InitialBulletCount; i++)
        {
            // 총알 만들어서 풀에 보관
            BulletController bullet = NewCreateBullet();
            ReturnBullet(bullet);
        }
    }

    // 새로운 총알 만들기
    private BulletController NewCreateBullet()
    {
        // 총알 만들고 비활성화 설정
        BulletController bullet = Instantiate(_bulletPrefab, _bulletStorageBox);
        bullet.gameObject.SetActive(false);

        // 반환 풀 전달
        bullet.SetPool(this);

        return bullet;
    }

    // 총알 꺼내오기
    public BulletController GetBullet()
    {
        // 풀에 보관한 총알이 없으면
        if (_bulletPool.Count == 0)
        {
            // 확장 가능 여부 확인
            if (_poolExpandable)
            {
                // 총알 만들어서 반환
                return NewCreateBullet();
            }

            // 확장이 불가면 null 반환
            return null;
        }

        // 풀에서 총알 가져오기
        BulletController bullet = _bulletPool.Dequeue();
        // 총알 활성화
        bullet.gameObject.SetActive(true);
        // 총알 반환
        return bullet;
    }

    // 총알 반납
    public void ReturnBullet(BulletController bullet)
    {
        // 총알 비활성화
        bullet.gameObject.SetActive(false);

        // 풀에 총알 반납
        _bulletPool.Enqueue(bullet);
    }
}
