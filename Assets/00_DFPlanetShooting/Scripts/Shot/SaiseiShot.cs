using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace J8N9.PlanetShooting
{
    public class SaiseiShot : MonoBehaviour
    {
        public GameObject Bullet;

        [SerializeField]
        private Transform _parenTransform; // ShotPrefab格納

        [SerializeField]
        private Transform _muzzle; // 発射口

        [SerializeField]
        private float shotInterval = 500;

        private SaiseiBulletPool _bulletPool;

        public static Subject<SaiseiBulletObjects> ReturnPool = new Subject<SaiseiBulletObjects>();

        void Awake()
        {
            CreateBulletPool(_parenTransform, Bullet);
        }

        private void Start()
        {
            InputShot();
        }

        // ショットを発射
        private void InputShot()
        {
            if (GameManager.Instance.isController)
            {
                this.UpdateAsObservable()
                .Where(x =>
                (Input.GetButtonDown("SaiseiShot"))
                )
                .Subscribe(x => PlayerShot());
            }
            else
            {
                this.UpdateAsObservable()
                .Where(x =>
                (Input.GetKeyDown(KeyCode.Space)) // コントローラー未使用ならSpaceキーで発射
                )
                .ThrottleFirst(TimeSpan.FromMilliseconds(shotInterval)) // 発射間隔
                .Subscribe(x => PlayerShot());
            }
        }

        //　BulletPool生成
        public void CreateBulletPool(Transform parenTransform, GameObject bullet)
        {
            _bulletPool = new SaiseiBulletPool(parenTransform, bullet);
            this.OnDestroyAsObservable().Subscribe(_ => _bulletPool.Dispose());
            ReturnPool.Subscribe(_obj => _bulletPool.Return(_obj)).AddTo(this.gameObject);
        }

        // ショットの初期位置
        public void PlayerShot()
        {
            if (GameManager.Instance.CurrentScene != GameManager.Scene.Play)
                return;

            var b = _bulletPool.Rent();
            SoundManager.Instance.PlaySe(SoundManager.Instance.SeAudioSource[4]);
            b.transform.position = _muzzle.position;
            //b.GetComponent<Rigidbody2D>().AddForce(transform.right * 1600 / Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }
}