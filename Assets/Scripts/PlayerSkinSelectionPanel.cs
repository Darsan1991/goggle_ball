//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Game;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class PlayerSkinSelectionPanel : ShowHidable
//{
//    [SerializeField] private SkinTileUI _skinTileUIPrefab;
//    [SerializeField] private RectTransform _content;
//
//    private readonly List<SkinTileUI> _tiles = new List<SkinTileUI>();
//    private SkinTileUI _selectedTile;
//
//    public SkinTileUI SelectedTile
//    {
//        get { return _selectedTile; }
//        set
//        {
//            if (_selectedTile != null)
//            {
//                _selectedTile.Selected = false;
//            }
//
//            _selectedTile = value;
//            _selectedTile.Selected = true;
//            ResourceManager.SetSelectedSkin(_selectedTile.MViewModel.Skin.id);
//        }
//    }
//
//    private void Awake()
//    {
//        foreach (var playerSkin in ResourceManager.PlayerSkins)
//        {
//            var skinTileUI = Instantiate(_skinTileUIPrefab, _content);
//            skinTileUI.MViewModel = new SkinTileUI.ViewModel
//            {
//                Skin = playerSkin,
//                Locked = ResourceManager.IsSkinLocked(playerSkin.id)
//            };
//            skinTileUI.Clicked += SkinTileUIOnClicked;
//            _tiles.Add(skinTileUI);
//        }
//
//        var skin = ResourceManager.GetSelectedSkin();
//        SelectedTile = _tiles.First(ui => ui.MViewModel.Skin.id == skin);
//    }
//
//    private void Start()
//    {
//        var gridLayout = _content.GetComponent<GridLayoutGroup>();
//        if (gridLayout != null)
//        {
//            if (gridLayout.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
//                return;
//            var rectTransform = (RectTransform) _content.transform;
//            var sizeX = rectTransform.rect.size.x;
//            var totalSize = gridLayout.cellSize.x * gridLayout.constraintCount + gridLayout.padding.left + gridLayout.padding.right;
//            var calSpace = (sizeX - totalSize)/(gridLayout.constraintCount+1);
//            if(gridLayout.spacing.x*2 < calSpace )
//                gridLayout.spacing = new Vector2(calSpace*0.8f,gridLayout.spacing.y);
//        }
//    }
//
//    // ReSharper disable once MethodTooLong
//    private void SkinTileUIOnClicked(SkinTileUI tileUI)
//    {
//        if (tileUI.MViewModel.Locked)
//        {
//            var playerSkin = tileUI.MViewModel.Skin;
//            var title = "";
//            var val = "";
//            switch (playerSkin.lockDetails.type)
//            {
//                case LockDetails.Type.BestScore:
//                    title = $"Reach Best Score {playerSkin.lockDetails.value}";
//                    val = $"{MyGame.GameManager.BEST_SCORE}/{playerSkin.lockDetails.value}";
//                    break;
////                case LockDetails.Type.TotalScore:
////                    title = $"Reach Total Score {playerSkin.lockDetails.value}";
////                    val = $"{MyGame.GameManager.TOTAL_SCORE}/{playerSkin.lockDetails.value}";
////                    break;
////                case LockDetails.Type.PlayCount:
////                    title = $"Play {playerSkin.lockDetails.value} Times";
////                    val = $"{MyGame.GameManager.TOTAL_PLAY_COUNT}/{playerSkin.lockDetails.value}";
////                    break;
////
////                case LockDetails.Type.Combo:
////                    title = $"Reach Combo up to {playerSkin.lockDetails.value}";
////                    val = $"{MyGame.GameManager.MAX_COMBO_COUNT}/{playerSkin.lockDetails.value}";
////                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//
//            //TODO:Show Popup
//            var skinLockPopUp = UIManager.Instance.PlayerSkinLockPopUp;
//            skinLockPopUp.MViewModel = new PlayerSkinLockPopUp.ViewModel
//            {
//                Title = title,
//                ValueTxt = val,
//                Image = playerSkin.image
//            };
//            skinLockPopUp.Show();
//            return;
//        }
//
//        SelectedTile = tileUI;
//    }
//
//    public void OnClickBack()
//    {
//        Hide();
//    }
//}