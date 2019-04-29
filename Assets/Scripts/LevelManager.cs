using System;
using UnityEngine;

namespace Game
{
    public partial class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        public static event Action GameOver;
        public static event Action GameStarted;
        public static event Action<int> ScoreChanged;
        public static event Action<int,int, int> BallsStopped; 
        public static event Action BallsThrew; 

        [SerializeField] private Player _player;
        [SerializeField] private Target _leftTarget;
        [SerializeField] private Target _rightTarget;

        public int RollsLeft { get; private set; }

        private int _score;
        private int _leftTargetScore;
        private int _rightTargetScore;


        public Player Player => _player;

        public int Score => LeftTargetScore + RightTargetScore;

        public int LeftTargetScore
        {
            get { return _leftTargetScore; }
            private set
            {
                _leftTargetScore = value;
                ScoreChanged?.Invoke(Score);
            }
        }

        public int RightTargetScore
        {
            get { return _rightTargetScore; }
            private set
            {
                _rightTargetScore = value; 
                ScoreChanged?.Invoke(Score);
            }
        }

        public bool BestScoreArchived => Score >= MyGame.GameManager.BEST_SCORE;
        public BallsState CurrentBallsState { get; private set; }

        public State CurrentState { get; private set; }


        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void Awake()
        {
            Instance = this;
        }

        public void StartTheGame()
        {
            CurrentState = State.Playing;
            MyGame.GameManager.TOTAL_GAME_COUNT++;
            ResetBalls();
            GameStarted?.Invoke();
        }

        private void Start()
        {
            StartTheGame();
        }

        public void ThrowBalls()
        {
            if(RollsLeft<=0)
                return;

            _leftTarget.Throw();
            _rightTarget.Throw();
            RollsLeft--;
            CurrentBallsState = BallsState.Moving;

            BallsThrew?.Invoke();
        }

        public void ResetBalls()
        {
            _leftTarget.ResetTarget();
            _rightTarget.ResetTarget();
            CurrentBallsState = BallsState.Ready;
        }

        public void StopBalls()
        {
            _leftTarget.Stop();
            _rightTarget.Stop();

            LeftTargetScore += _leftTarget.GetCurrentPoints();
            RightTargetScore += _rightTarget.GetCurrentPoints();
            CurrentBallsState = BallsState.Stopped;
            BallsStopped?.Invoke(RollsLeft+1,_leftTarget.GetCurrentPoints(),_rightTarget.GetCurrentPoints());
        }

        private void Update()
        {
            if (CurrentState == State.GameOver)
                return;
        }


        [ContextMenu("OverTheGame")]
        // ReSharper disable once MethodNameNotMeaningful
        private void OverTheGame()
        {
            if (CurrentState == State.GameOver)
                return;
            CurrentState = State.GameOver;
            if (MyGame.GameManager.BEST_SCORE < Score)
            {
                MyGame.GameManager.BEST_SCORE = Score;
            }

            GameOver?.Invoke();
        }

        public enum BallsState
        {
            None,Ready,Moving,Stopped
        }

        public enum State
        {
            None,
            Playing,
            GameOver
        }
    }

}