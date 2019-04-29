using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuPanel : ShowHidable
    {

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public void OnClickPlay()
        {
            MyGame.GameManager.LoadScene("Main");
        }

        public void OnClickBuy()
        {
            var storePanel = UIManager.Instance.StorePanel;
            storePanel.Show();
        }

        public void OnClickHelp()
        {
            var tutorialPanel = UIManager.Instance.TutorialPanel;
            tutorialPanel.Show();
        }

        public void OnClickHighScore()
        {

        }

        public void OnClickRating()
        {

        }

        public void OnClickShare()
        {

        }
     
    }
}