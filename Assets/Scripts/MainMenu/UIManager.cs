using Game;
using UnityEngine;

namespace MainMenu
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private StorePanel _storePanel;
        [SerializeField] private TutorialPanel _tutorialPanel;

        public TutorialPanel TutorialPanel => _tutorialPanel;
        public MenuPanel MenuPanel => _menuPanel;
        public StorePanel StorePanel => _storePanel;

        private void Awake()
        {
            Instance = this;
//            AdsManager.Instance.ShowBanner = true;
        }
    }
}