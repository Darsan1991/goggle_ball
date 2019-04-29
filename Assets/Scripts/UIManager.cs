using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameOverPanel _gameOverPanel;
        [SerializeField] private GamePlayPanel _gamePlayPanel;
        [SerializeField] private RollScorePanel _rollScorePanel;



        public GameOverPanel GameOverPanel => _gameOverPanel;

        public GamePlayPanel GamePlayPanel => _gamePlayPanel;
        public static UIManager Instance { get; private set; }
        public RollScorePanel RollScorePanel => _rollScorePanel;


        private void Awake()
        {
            Instance = this;
#if DAILY_REWARD
            if (MyGame.GameManager.HasPendingDailyReward)
            {
                var dailyRewardPanel = SharedUIManager.DailyRewardPanel;
                dailyRewardPanel.Show();
            }
#endif
        }

        private void OnEnable()
        {
            LevelManager.GameStarted += LevelManagerOnGameStarted;
            LevelManager.GameOver += LevelManagerOnGameOver;
        }


        private void OnDisable()
        {
            LevelManager.GameStarted -= LevelManagerOnGameStarted;
            LevelManager.GameOver -= LevelManagerOnGameOver;
        }

        private void LevelManagerOnGameOver()
        {
            StartCoroutine(GameOver());
        }

        IEnumerator GameOver()
        {
            yield return new WaitForSeconds(0.1f);
            _gameOverPanel.Show();
        }

        private void LevelManagerOnGameStarted()
        {
            GamePlayPanel.Show();
        }
    }
}