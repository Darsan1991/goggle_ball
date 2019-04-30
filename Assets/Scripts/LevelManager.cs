using System;
using UnityEngine;

namespace Game
{
    public partial class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        public static event Action<GameOverData> GameOver;
        public static event Action GameStarted;
        public static event Action<int> ScoreChanged;
        public static event Action<int,int, int> BallsStopped; 
        public static event Action BallsThrew; 

        [SerializeField] private Player _player;
        [SerializeField] private Target _leftTarget;
        [SerializeField] private Target _rightTarget;
        [SerializeField] private AudioSource _ballRollAudioSource;
        [SerializeField] private AudioClip _rollFinishClip;

        public int RollsLeft => TotalRollCount - CurrentRoll;
        public int TotalRollCount => 5;
        public int CurrentRoll { get; private set; }

        private int _score;
        private BallsState _currentBallsState;

        public bool SlowMove
        {
            get { return Time.timeScale < 1f;}
            set { Time.timeScale = value ? 0.3f : 1f; }
        }

        public Player Player => _player;

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value; 
                ScoreChanged?.Invoke(value);
            }
        }

        public int LeftTargetScore { get; private set; }

        public int RightTargetScore { get; private set; }

        public bool BestScoreArchived => Score >= MyGame.GameManager.BEST_SCORE;

        public BallsState CurrentBallsState
        {
            get { return _currentBallsState; }
            private set
            {
                _currentBallsState = value;

                if (AudioManager.IsSoundEnable)
                {
                    if (value == BallsState.Moving && _ballRollAudioSource &&!_ballRollAudioSource.isPlaying)
                    {
                        _ballRollAudioSource.Play();
                    }
                    else if(value!=BallsState.Moving && _ballRollAudioSource && _ballRollAudioSource.isPlaying)
                    {
                        _ballRollAudioSource.Stop();
                    }
                }
            }
        }

        public State CurrentState { get; private set; }


  

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
            CurrentRoll++;
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

            if (SlowMove)
                SlowMove = false;

            LeftTargetScore += _leftTarget.GetCurrentPoints();
            RightTargetScore += _rightTarget.GetCurrentPoints();
            Score += _leftTarget.GetCurrentPoints() + _rightTarget.GetCurrentPoints();
            CurrentBallsState = BallsState.Stopped;

            if (AudioManager.IsSoundEnable && _rollFinishClip)
            {
                AudioSource.PlayClipAtPoint(_rollFinishClip,Camera.main.transform.position);
            }

            BallsStopped?.Invoke(CurrentRoll,_leftTarget.GetCurrentPoints(),_rightTarget.GetCurrentPoints());

            if (RollsLeft == 0)
            {
                OverTheGame(_leftTarget.GetCurrentPoints(),_rightTarget.GetCurrentPoints());
            }
        }

        private void Update()
        {
            if (CurrentState == State.GameOver)
                return;
        }


        [ContextMenu("OverTheGame")]
        // ReSharper disable once MethodNameNotMeaningful
        private void OverTheGame(int leftScore,int rightScore)
        {
            if (CurrentState == State.GameOver)
                return;

            var bonus = CalculateBonus(Score);
            Score += bonus;

            CurrentState = State.GameOver;
            if (MyGame.GameManager.BEST_SCORE < Score)
            {
                MyGame.GameManager.BEST_SCORE = Score;
            }

           
            GameOver?.Invoke(new GameOverData
            {
                LeftScore = leftScore,
                RightScore = rightScore,
                Bonus = bonus
            });
        }

        private static int CalculateBonus(int score)
        {
            return (score / 100) * 50;
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

public struct GameOverData
{
    public int LeftScore { get; set; }
    public int RightScore { get; set; }
    public int Bonus { get; set; }
}