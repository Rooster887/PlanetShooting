using UniRx;
using UniRx.Triggers;
using UnityEngine;
using TMPro;

namespace J8N9.PlanetShooting
{
    public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
    {
        public static int blockCount; // 残りブロック数

        public TextMeshProUGUI scoreText;

        public TextMeshProUGUI percent; // %表記

        private const int CLEAR_SCORE_COUNT = 88; // クリアするスコア(Hakai側)

        public int clearScoreCount => CLEAR_SCORE_COUNT;

        private Color32 defaultColor = new Color32(255, 255, 255, 255); // 通常時のスコアカラー

        private Color32 clearColor = new Color32(191, 0, 20, 255); // クリア時のスコアカラー

        public Color32 WinCol => clearColor;

        private void Awake()
        {
            Initialized();
        }

        private void Start()
        {
            if (GameManager.Instance.CurrentScene == GameManager.Scene.Play)
            {
                StartScoreCount();
            }
        }

        // Scoreを更新
        private void StartScoreCount()
        {
            // ブロック数を反映
            this.UpdateAsObservable()
                .Subscribe(x =>
                {
                    scoreText.text = blockCount.ToString();
                });

            // クリア基準を満たしたらScoreTextを赤に
            this.UpdateAsObservable()
                .Where(x => blockCount >= CLEAR_SCORE_COUNT)
                .Subscribe(x =>
                {
                    ChangeText(clearColor);
                });

            // 通常時のScoreTextカラー
            this.UpdateAsObservable()
                .Where(x => blockCount <= CLEAR_SCORE_COUNT)
                .Subscribe(x =>
                {
                    ChangeText(defaultColor);
                });
        }

        private void ChangeText(Color32 color)
        {
            scoreText.faceColor = color;
            percent.faceColor = color;
        }

        private void Initialized()
        {
            blockCount = 0;
        }
    }
}