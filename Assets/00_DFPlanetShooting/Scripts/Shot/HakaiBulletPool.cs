using UniRx.Toolkit;
using UnityEngine;

namespace J8N9.PlanetShooting
{
    public class HakaiBulletPool : ObjectPool<HakaiBulletObjects>
    {
        private readonly GameObject _bulltPrefab;

        private readonly Transform _parenTransform; // BulletRoot

        public HakaiBulletPool(Transform parenTransform, GameObject bullet)
        {
            _parenTransform = parenTransform;
            _bulltPrefab = bullet;
        }

        protected override HakaiBulletObjects CreateInstance()
        {
            var b = GameObject.Instantiate(_bulltPrefab).GetComponent<HakaiBulletObjects>();
            b.transform.SetParent(_parenTransform); // BulletRootに格納
            return b;
        }
    }
}