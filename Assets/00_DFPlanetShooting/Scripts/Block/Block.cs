using System.Collections;
using UnityEngine;

namespace J8N9.PlanetShooting
{
    public class Block : MonoBehaviour
    {
        [SerializeField]
        private GameObject _defaultBlock;

        [SerializeField]
        private GameObject _saiseiBlock;

        private GameObject saiseiBlock = null;

        private CapsuleCollider2D capCol;

        private void Awake()
        {
            capCol = this.GetComponent<CapsuleCollider2D>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // プレイシーン以外では実行しない
            if (GameManager.Instance.CurrentScene != GameManager.Scene.Play)
                return;

            // HakaiShotが当たった時
            if (LayerMask.LayerToName(other.gameObject.layer) == "Hakai")
            {
                //StartFlicker();
                ScoreManager.blockCount += 1;
                SoundManager.Instance.PlaySe(SoundManager.Instance.SeAudioSource[0]);
                if (!EffectManager.IsExplosion)
                    ExplosionGenerated(other);

                //　ブロックパターンが初期状態
                if (_defaultBlock != null)
                {
                    _defaultBlock.AddComponent<Rigidbody2D>();
                    capCol.enabled = false;
                    Destroy(_defaultBlock, 2.0f);
                    StartCoroutine(ChangeBreakAtBlank());
                }

                // ブロックパターンがSaisei時
                if (saiseiBlock != null)
                {
                    saiseiBlock.AddComponent<Rigidbody2D>();
                    capCol.enabled = false;
                    Destroy(this.saiseiBlock, 2.0f);
                    StartCoroutine(ChangeBreakAtBlank());
                }
            }

            // SaiseiShotが当たった時
            if (this.gameObject.layer == 9 && LayerMask.LayerToName(other.gameObject.layer) == "Saisei")
            {
                if (!EffectManager.IsRepair)
                {
                    RepairGenerated(other);
                }
                ScoreManager.blockCount -= 1;
                GameObjectExtension.SetLayer(this.gameObject, 8);
                capCol.enabled = false;
                GameObject obj = Instantiate(_saiseiBlock);
                obj.transform.parent = this.gameObject.transform;
                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                this.saiseiBlock = obj;
                StartCoroutine(ChangeSaiseiBlock());
            }
        }

        // ブロックレイヤーをBlankに変更
        private IEnumerator ChangeBreakAtBlank()
        {
            // Breakレイヤーに切り替え
            GameObjectExtension.SetLayer(this.gameObject, 8);
            capCol.isTrigger = true;

            // インターバル
            yield return new WaitForSeconds(2.0f);
            capCol.enabled = true;

            // Blankレイヤー切り替え
            GameObjectExtension.SetLayer(this.gameObject, 9);
        }

        // ブロックレイヤーをデフォルトに変更
        private IEnumerator ChangeSaiseiBlock()
        {
            // インターバル
            yield return new WaitForSeconds(2.0f);
            capCol.enabled = true;
            capCol.isTrigger = false;

            // Defaultレイヤーに切り替え
            GameObjectExtension.SetLayer(this.gameObject, 0);
        }

        // 爆発エフェクト
        private void ExplosionGenerated(Collider2D col)
        {
            var scale = Random.Range(0.7f, 0.25f);
            var offset = Random.Range(-0.5f, 0.5f);
            var pos = new Vector3(
                col.transform.position.x + offset,
                col.transform.position.y + offset,
                col.transform.position.z
                );
            var explosionScale = new Vector3(scale, scale, scale);

            GameObject explosion = Instantiate(EffectManager.Instance.Explosion, pos, Quaternion.identity);
            explosion.transform.localScale = explosionScale;
            Destroy(explosion, 0.5f);
            EffectManager.IsExplosion = true;
            StartCoroutine(ExplosionInterval(EffectManager.Instance.ExplosionInterval));
        }

        // Saiseiエフェクト
        private void RepairGenerated(Collider2D col)
        {
            var scale = Random.Range(0.7f, 0.25f);
            var repairScale = new Vector3(scale, scale, scale);
            GameObject repair = Instantiate(EffectManager.Instance.Repair, col.transform.position, Quaternion.identity);
            repair.transform.localScale = repairScale;
            Destroy(repair, 0.5f);
            EffectManager.IsRepair = true;
            StartCoroutine(RepairInterval(EffectManager.Instance.RepairInterval));
        }

        // 爆発インターバル
        private IEnumerator ExplosionInterval(float interbal)
        {
            yield return new WaitForSeconds(interbal);
            if (EffectManager.IsExplosion)
                EffectManager.IsExplosion = false;
        }

        // Saiseiインターバル
        private IEnumerator RepairInterval(float interbal)
        {
            yield return new WaitForSeconds(interbal);
            if (EffectManager.IsRepair)
                EffectManager.IsRepair = false;
        }
    }
}