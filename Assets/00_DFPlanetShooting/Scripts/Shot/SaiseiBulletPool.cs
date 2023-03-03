using UniRx.Toolkit;
using UnityEngine;

namespace J8N9.PlanetShooting
{
    public class SaiseiBulletPool : ObjectPool<SaiseiBulletObjects>
    {
        private readonly GameObject _bulltPrefab; // BulletRoot

        private readonly Transform _parenTransform;

        public SaiseiBulletPool(Transform parenTransform, GameObject bullet)
        {
            _parenTransform = parenTransform;
            _bulltPrefab = bullet;
        }

        // ショット生成
        protected override SaiseiBulletObjects CreateInstance()
        {
            var b = GameObject.Instantiate(_bulltPrefab).GetComponent<SaiseiBulletObjects>();
            b.transform.SetParent(_parenTransform); // BulletRootに格納
            return b;
        }
    }
}