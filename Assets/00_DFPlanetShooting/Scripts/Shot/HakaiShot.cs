using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace J8N9.PlanetShooting
{
    public class HakaiShot : MonoBehaviour
    {
        public GameObject Bullet;

        [SerializeField]
        private Transform _parenTransform; // ShotPrefab格納

        [SerializeField]
        private Transform _muzzle; // 発射口

        private HakaiBulletPool _bulletPool;

        public static Subject<HakaiBulletObjects> ReturnPool = new Subject<HakaiBulletObjects>();

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
                (Input.GetButtonDown("HakaiShot"))
                )
                .Subscribe(x => PlayerShot());
            }
            else
            {
                this.UpdateAsObservable()
                .Where(x =>
                (Input.GetKeyDown(KeyCode.Space)) // コントローラー未使用ならSpaceキーで発射
                )
                .Subscribe(x => PlayerShot());
            }
        }

        //　BulletPool生成
        public void CreateBulletPool(Transform parenTransform, GameObject bullet)
        {
            _bulletPool = new HakaiBulletPool(parenTransform, bullet);
            this.OnDestroyAsObservable().Subscribe(_ => _bulletPool.Dispose());
            ReturnPool.Subscribe(_obj => _bulletPool.Return(_obj)).AddTo(this.gameObject);
        }

        // ショットの初期位置
        public void PlayerShot()
        {
            if (GameManager.Instance.CurrentScene != GameManager.Scene.Play)
                return;

            var b = _bulletPool.Rent();
            SoundManager.Instance.PlaySe(SoundManager.Instance.SeAudioSource[2]);
            b.transform.position = _muzzle.position;
            b.transform.rotation = transform.rotation;
            //b.GetComponent<Rigidbody2D>().velocity = -transform.right * 2500 * Time.fixedDeltaTime;
        }
    }
}