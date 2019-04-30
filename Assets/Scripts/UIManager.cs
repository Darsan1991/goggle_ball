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
            LevelManager.BallsStopped += LevelManagerOnBallsStopped;
        }


        private void OnDisable()
        {
            LevelManager.GameStarted -= LevelManagerOnGameStarted;
            LevelManager.GameOver -= LevelManagerOnGameOver;
            LevelManager.BallsStopped -= LevelManagerOnBallsStopped;
        }

        private void LevelManagerOnBallsStopped(int roll, int leftScore, int rightScore)
        {
            if(LevelManager.Instance.RollsLeft<=0)
                return;
            StartCoroutine(ShowCurrentRollPanel(roll, leftScore, rightScore));
        }

        private IEnumerator ShowCurrentRollPanel(int roll, int leftScore, int rightScore)
        {
            var rollScorePanel = RollScorePanel;
            rollScorePanel.MViewModel = new RollScorePanel.ViewModel
            {
                roll = roll,
                currentRollScore = leftScore + rightScore,
                totalScore = LevelManager.Instance.Score
            };
            rollScorePanel.Show();
            yield return new WaitForSeconds(1);
            rollScorePanel.Hide();
            yield return new WaitUntil(() => rollScorePanel.CurrentShowState == ShowState.Hide);
            LevelManager.Instance.ResetBalls();
        }

        private void LevelManagerOnGameOver(GameOverData gameOverData)
        {
            StartCoroutine(GameOver(gameOverData));
        }

        IEnumerator GameOver(GameOverData gameOverData)
        {
            yield return new WaitForSeconds(0.1f);
            _gameOverPanel.MViewModel = new GameOverPanel.ViewModel
            {
                roll = LevelManager.Instance.TotalRollCount,
                bonus = gameOverData.Bonus,
                currentRollGamePoints = gameOverData.LeftScore+gameOverData.RightScore,
                totalGamePoints = LevelManager.Instance.Score - gameOverData.Bonus,
                totalPoints = LevelManager.Instance.Score
            };
            _gameOverPanel.Show();
        }

        private void LevelManagerOnGameStarted()
        {
            GamePlayPanel.Show();
        }
    }
}