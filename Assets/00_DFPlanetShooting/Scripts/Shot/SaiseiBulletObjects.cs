using UnityEngine;

namespace J8N9.PlanetShooting
{
    public class SaiseiBulletObjects : MonoBehaviour
    {
        private void OnEnable()
        {
            Invoke("destroy", 1);
        }
        private void destroy()
        {
            SaiseiShot.ReturnPool.OnNext(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            gameObject.SetActive(false);
        }
    }
}