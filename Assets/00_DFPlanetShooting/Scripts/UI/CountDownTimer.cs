using UnityEngine;
using UniRx;
using System;

namespace J8N9.PlanetShooting
{
    public class CountDownTimer : MonoBehaviour
    {
        public IObservable<int> CountDownObservable => _countDounObservable.AsObservable();

        public static int countTime;

        private IConnectableObservable<int> _countDounObservable;

        private void Awake()
        {
            countTime = PlayTime.playTime;
            _countDounObservable = CreateCountDownObservable(countTime).Publish();
        }

        void Start()
        {
            if (GameManager.Instance.CurrentScene == GameManager.Scene.Play)
                _countDounObservable.Connect();
        }

        // カウントダウン
        private IObservable<int> CreateCountDownObservable(int CountTime)
        {
            return Observable
                .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
                .Select(x => (int)(CountTime - x))
                .TakeWhile(x => x > -1);
        }
    }
}