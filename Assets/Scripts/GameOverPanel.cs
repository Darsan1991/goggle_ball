using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameOverPanel : ShowHidable
    {
        [SerializeField] private Text _rollText;
        [SerializeField] private Text _gamePointsText;
        [SerializeField] private Text _bonusText;
        [SerializeField] private Text _totalPointsText;
        private ViewModel _mViewModel;

        public ViewModel MViewModel
        {
            get { return _mViewModel; }
            set
            {

                _mViewModel = value;

                _rollText.text = $"ROLL {value.roll}: {value.currentRollGamePoints} POINTS";
                _gamePointsText.text = $"GAME POINTS: {value.totalGamePoints} POINTS";
                _bonusText.text = $"{value.bonus} BONUS POINTS";
                _totalPointsText.text = $"TOTAL GAME POINTS: {value.totalPoints} POINTS";
            }
        }

        public void OnClickRestart()
        {
            MyGame.GameManager.LoadScene(SceneManager.GetActiveScene().name,false);
        }


        public override void Show(bool animate = true, Action completed = null)
        {
            base.Show(animate, completed);

        }


        public void OnClickMainMenu()
        {
            MyGame.GameManager.LoadScene("MainMenu",false);
        }

        public void OnClickLeadersboard()
        {

        }

        public struct ViewModel
        {
            public int roll;
            public int currentRollGamePoints;
            public int totalGamePoints;
            public int bonus;
            public int totalPoints;
        }
    }
}

public enum Tag
{

}

public enum Layer
{

}

public static class Extensions
{
    public static int GetMask(this Layer layer) => 
        LayerMask.NameToLayer(layer.ToString());

    public static T GetRandom<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        return list[Random.Range(0, list.Count)];
    }

    public static Vector2 GetSizeInScreenSpace(this RectTransform rectTransform)
    {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Vector2(Mathf.Abs((corners[1] - corners[2]).magnitude), Mathf.Abs((corners[1] - corners[0]).magnitude));
    }
}