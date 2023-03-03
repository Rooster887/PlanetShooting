using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace J8N9.PlanetShooting
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public bool isController = false; // コントローラーを使用するか

        public static bool ISGameSet = false; // プレイ終了か

        public static bool ISWin = false; // Hakaiの勝利か

        [SerializeField]
        private GameObject saiseiUnit; // Saisei

        [SerializeField]
        private GameObject resultView; // 結果

        [SerializeField]
        private ChangeTimeCount changeTimeCount; // プレイタイム

        public enum Scene
        {
            Start,
            Play,
            GameSet
        }

        Scene currentScene = Scene.Start;

        public Scene CurrentScene => currentScene;

        private void Awake()
        {
            Initialized();

            if (SceneManager.GetActiveScene().name == "StartScene")
            {
                ChangeStateScene(Scene.Start);
            }
            else
            {
                ChangeStateScene(Scene.Play);
            }
            SwitchScene();
        }

        private void ChangeStateScene(Scene scene)
        {
            currentScene = scene;

            switch (currentScene)
            {
                case Scene.Start:
                    break;
                case Scene.Play:
                    ChangeGameSet();
                    StartCoroutine(ActiveSaiseiUnit(5.0f));
                    break;
                case Scene.GameSet:
                    StartCoroutine(ChangeResultView());
                    break;
                default:
                    break;
            }
        }

        // シーンの変更
        private void SwitchScene()
        {
            // StartSceneからGameSceneへの以降
            if (currentScene == Scene.Start)
            {
                this.UpdateAsObservable()
                .Where(x =>
                (Input.GetKey(KeyCode.S) || Input.GetButtonDown("Start"))
                )
                .Subscribe(x => LoadScene("GameScene"));
            }

            //if (currentScene == Scene.GameSet)
            //{
            //    this.UpdateAsObservable()
            //    .Where(x =>
            //    (Input.GetKey(KeyCode.Escape))
            //    )
            //    .Subscribe(x => LoadScene("StartScene"));
            //}

            // GameScene強制終了
            else if (currentScene == Scene.Play)
            {
                this.UpdateAsObservable()
                .Where(x =>
                (Input.GetKey(KeyCode.Escape))
                )
                .Subscribe(x => UnityEngine.Application.Quit());
            }
        }

        // プレイ終了
        private void ChangeGameSet()
        {
            this.ObserveEveryValueChanged(x => ISGameSet)
                 .Where(x => x)
                 .Subscribe(x => ChangeStateScene(Scene.GameSet));
        }

        // GameScene開始からwaitTime後にSaisei出現
        private IEnumerator ActiveSaiseiUnit(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            saiseiUnit.SetActive(true);
            yield break;
        }

        // リザルト画面に移行
        private IEnumerator ChangeResultView()
        {
            // SE全停止
            foreach (AudioSource audio in SoundManager.Instance.SeAudioSource)
            {
                audio.enabled = false;
            }

            // BGMフェードアウト
            SoundManager.Instance.BGMFadeOut();
            yield return new WaitForSeconds(5f);

            resultView.SetActive(true); // リザルト画面表示

            // Hakai勝利時のBGM
            if (ScoreManager.blockCount >= ScoreManager.Instance.clearScoreCount)
            {
                ISWin = true;
                SoundManager.Instance.PlayBgm("Win");
            }
            else
            {
                SoundManager.Instance.PlayBgm("Lose"); // Hakai敗北時のBGM
            }

            yield return new WaitForSeconds(15f);
            LoadScene("StartScene"); // 15秒後にStratSceneに移行

            yield break;
        }

        private void LoadScene(string name)
        {
            Initialized();
            SceneManager.LoadScene(name);
        }

        private void Initialized()
        {
            ISGameSet = false;
            ISWin = false;


            // スタートシーンでプレイタイムチェック
            if (currentScene == Scene.Start && changeTimeCount != null)
            {
                CheckPlayTime();
            }

            CountDownTimer.countTime = PlayTime.playTime;
        }

        // StartSceneチェックボックスON・OFFでプレイタイム変更
        private void CheckPlayTime()
        {
            if (changeTimeCount.toggle.isOn)
                PlayTime.playTime = 60;
            else
                PlayTime.playTime = 89;
        }
    }
}
