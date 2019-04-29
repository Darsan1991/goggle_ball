using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class RollScorePanel : ShowHidable
    {
        [SerializeField] private Text _rollScoreText;
        [SerializeField] private Text _totalScoreText;
        private ViewModel _mViewModel;

        public ViewModel MViewModel
        {
            get { return _mViewModel; }
            set
            {
                _mViewModel = value;
                _rollScoreText.text = $"ROLL {value.roll} = {value.currentRollScore} POINTS";
                _totalScoreText.text = $"GAME POINTS = {value.totalScore} POINTS";
            }
        }

        public struct ViewModel
        {
            public int roll;
            public int currentRollScore;
            public int totalScore;
        }
    }
}