using UnityEngine;
using TMPro;

namespace J8N9.PlanetShooting
{
    public class Result : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        public TextMeshProUGUI percent;

        [SerializeField]
        private GameObject _winEffect;

        [SerializeField]
        private GameObject _congratulation;

        void Start()
        {
            if (GameManager.Instance.CurrentScene == GameManager.Scene.GameSet)
            {
                ScoreView(); // GameSet時にスコア表示
            }
        }

        private void ScoreView()
        {
            scoreText.text = ScoreManager.blockCount.ToString();

            // Hakai勝利時
            if (GameManager.ISWin)
            {
                _winEffect.SetActive(true);
                _congratulation.SetActive(true);
                scoreText.faceColor = ScoreManager.Instance.WinCol;
                percent.faceColor = ScoreManager.Instance.WinCol;
            }
            else
            {
                _winEffect.SetActive(false);
                _congratulation.SetActive(false);
            }
        }
    }
}