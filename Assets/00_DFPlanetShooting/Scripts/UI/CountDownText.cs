using UnityEngine;
using UniRx;
using TMPro;

namespace J8N9.PlanetShooting
{
    public class CountDownText : MonoBehaviour
    {
        [SerializeField]
        private CountDownTimer countDownTimer;

        [SerializeField]
        private TextMeshProUGUI timeText;

        private Color32 colorRed = new Color32(246, 16, 16, 255);

        private void Awake()
        {
            timeText.text = CountDownTimer.countTime.ToString();
        }

        void Start()
        {
            // 残り時間更新
            countDownTimer
                .CountDownObservable
                .Subscribe(time => timeText.text = time.ToString());

            // 10秒前
            countDownTimer
                .CountDownObservable
                .First(timer => timer <= 10)
                .Subscribe(x => timeText.faceColor = colorRed);

            // 0秒
            countDownTimer
                .CountDownObservable
                .First(timer => timer <= 0)
                .Subscribe(x => GameManager.ISGameSet = true);
        }
    }
}