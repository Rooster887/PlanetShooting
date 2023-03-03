using UnityEngine;

namespace J8N9.PlanetShooting
{
    public class EffectManager : SingletonMonoBehaviour<EffectManager>
    {
        [SerializeField]
        private GameObject _explosion;

        public GameObject Explosion => _explosion;

        [SerializeField]
        private float _explosionInterval;

        public float ExplosionInterval => _explosionInterval;

        public static bool IsExplosion = false;

        [SerializeField]
        private GameObject _repair;

        public GameObject Repair => _repair;

        [SerializeField]
        private float _repairInterval;

        public float RepairInterval => _repairInterval;

        public static bool IsRepair = false;
    }
}