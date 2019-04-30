using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GamePlayPanel : ShowHidable
    {

        [SerializeField] private Button _actionButton;
        [SerializeField]private Sprite[] _startStopSprites = new Sprite[2];
        [SerializeField] private Text _leftScore, _rightScore;
        [SerializeField] private NumberImage _rollNumberImage;

        private ActionButtonState _currentActionButtonState;

        private LevelManager LevelManager=>LevelManager.Instance;

        private ActionButtonState CurrentActionButtonState
        {
            get { return _currentActionButtonState; }
            set
            {
                _currentActionButtonState = value;
                _actionButton.image.sprite = _startStopSprites[value == ActionButtonState.Start ? 0 : 1];
                _actionButton.gameObject.SetActive(value != ActionButtonState.None);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
//            LevelManager.BallsStopped +=LevelManagerOnBallsStopped;
        }


        protected override void OnDisable()
        {
            base.OnDisable();
//            LevelManager.BallsStopped -= LevelManagerOnBallsStopped;
        }



//        private void LevelManagerOnBallsStopped(int roll, int leftScore, int rightScore)
//        {
//            StartCoroutine(ShowCurrentRollPanel(roll, leftScore, rightScore));
//        }
//
//        private IEnumerator ShowCurrentRollPanel(int roll, int leftScore, int rightScore)
//        {
//            var rollScorePanel = UIManager.Instance.RollScorePanel;
//            rollScorePanel.MViewModel = new RollScorePanel.ViewModel
//            {
//                roll = roll,
//                currentRollScore = leftScore + rightScore,
//                totalScore = LevelManager.Score
//            };
//            rollScorePanel.Show();
//            yield return new WaitForSeconds(1);
//            rollScorePanel.Hide();
//            yield return new WaitUntil(() => rollScorePanel.CurrentShowState == ShowState.Hide);
//            LevelManager.ResetBalls();
//        }


        private void Update()
        {
            _leftScore.text = LevelManager.LeftTargetScore.ToString();
            _rightScore.text = LevelManager.RightTargetScore.ToString();
            _rollNumberImage.Number = LevelManager.RollsLeft;
            CurrentActionButtonState = LevelManager.CurrentBallsState == LevelManager.BallsState.Ready
                ?
                ActionButtonState.Start
                : LevelManager.CurrentBallsState == LevelManager.BallsState.Moving
                    ? ActionButtonState.Stop
                    : ActionButtonState.None;
        }

        public void OnClickActionButton()
        {
            if (CurrentActionButtonState == ActionButtonState.Start)
            {
                LevelManager.ThrowBalls();
            }
            else
            {
                LevelManager.StopBalls();
            }
        }

        public void OnClickSlowBall()
        {
            if(ResourceManager.SlowBalls<=0)
                return;

            ResourceManager.SlowBalls--;
            LevelManager.SlowMove = true;
        }

        private enum ActionButtonState
        {
            None,Start,Stop
        }
    }
}