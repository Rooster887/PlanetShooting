using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace J8N9.PlanetShooting
{
    public class MoveController : MonoBehaviour
    {
        private Rigidbody2D rb;

        public float moveSpeed;　// 機体の速度

        public Transform planet;　// 惑星

        private int playerNum;　// プレイヤーの判別

        private string X = "Horizontal";

        private string Y = "Vertical";

        private bool isController; // コントローラーを使用するか

        private SpriteRenderer sprite = null;

        [SerializeField]
        private Sprite[] sprites;

        private void Awake()
        {
            Initialized();
        }

        private void Start()
        {
            InputMove(playerNum);
        }

        // コントローラーの入力を取得
        private void InputMove(int num)
        {
            this.FixedUpdateAsObservable()
                .Where(_ =>
                (Input.GetAxis(X) != 0) ||
                (Input.GetAxis(Y) != 0)
                )
                .Subscribe(_ => PlayerMove());
        }

        // プレイヤーの動きと方向
        private void PlayerMove()
        {
            if (GameManager.Instance.CurrentScene != GameManager.Scene.Play)
                return;

            var h = Input.GetAxis(X);
            var v = Input.GetAxis(Y);

            PlayerFlip(playerNum);

            if (h != 0 || v != 0)
            {
                PlayMoveSe();
                // var velocity = new Vector3(h, v) * moveSpeed;
                //transform.localPosition += velocity;;
                Vector3 pos = new Vector3(h, v, 0);
                rb.AddForce(pos * moveSpeed, ForceMode2D.Force);
                ChangeDirection(playerNum);
            }
        }

        // プレイヤーが常に惑星の方向を向く
        private void ChangeDirection(int num)
        {
            var vec = (transform.position - planet.transform.position).normalized;

            if (num == 1)
                transform.rotation = Quaternion.FromToRotation(Vector3.right, vec);
            else
                transform.rotation = Quaternion.FromToRotation(Vector3.left, vec);
        }

        // プレイヤーの判別
        private void ChackActiveController()
        {
            if (isController)
            {
                X = X + playerNum;
                Y = Y + playerNum;
            }
        }

        private void Initialized()
        {
            playerNum = this.GetComponent<PlayerNumber>().PlayerNum();
            rb = this.GetComponent<Rigidbody2D>();
            sprite = this.GetComponent<SpriteRenderer>();
            isController = GameManager.Instance.isController;
            ChackActiveController();
        }

        // 惑星の中心を基準にプレイヤーを反転
        private void PlayerFlip(int num)
        {
            var lscale = this.gameObject.transform.localScale;

            if (this.gameObject.transform.position.x < 0.0f)
            {
                if (num == 1)
                {
                    sprite.sprite = sprites[1];
                    lscale.y = -0.4f;
                }
                else
                {
                    sprite.sprite = sprites[0];
                    lscale.y = 0.32f;
                }
            }
            else
            {
                if (num == 1)
                {
                    sprite.sprite = sprites[0];
                    lscale.y = 0.4f;
                }
                else
                {
                    sprite.sprite = sprites[1];
                    lscale.y = -0.32f;
                }
            }

            transform.localScale = lscale;

        }

        // Move時のSE
        private void PlayMoveSe()
        {
            if (this.gameObject.name == "Hakai")
            {
                SoundManager.Instance.PlaySe(SoundManager.Instance.SeAudioSource[1]);
            }
            else
            {
                SoundManager.Instance.PlaySe(SoundManager.Instance.SeAudioSource[3]);
            }
        }
    }
}