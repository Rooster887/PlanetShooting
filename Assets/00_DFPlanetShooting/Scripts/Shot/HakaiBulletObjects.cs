using UnityEngine;
using System.Collections;

namespace J8N9.PlanetShooting
{
    public class HakaiBulletObjects : MonoBehaviour
    {
        [SerializeField]
        private GameObject _explosion;

        private void OnEnable()
        {
            Invoke("destroy", 1);
        }

        private void destroy()
        {
            HakaiShot.ReturnPool.OnNext(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            StartCoroutine(InactiveBullet());
        }

        // 弾が消えるインターバル
        IEnumerator InactiveBullet()
        {
            yield return new WaitForSeconds(0.04f);
            gameObject.SetActive(false);
            yield break;
        }
    }
}