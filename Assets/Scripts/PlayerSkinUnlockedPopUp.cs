//using System;
//using System.Collections;
//using System.Collections.Generic;
//using MyGame;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class PlayerSkinUnlockedPopUp : ShowHidable
//{
//    public event Action<bool> Decided; 
//
//    [SerializeField] private Image _img;
//    private PlayerSkin _playerSkin;
//
//    public PlayerSkin PlayerSkin
//    {
//        get { return _playerSkin; }
//        set
//        {
//            _img.sprite = value.icon;
//            _playerSkin = value;
//        }
//    }
//
//    public void OnClickYes()
//    {
//        Hide();
//        ResourceManager.SetSelectedSkin(PlayerSkin.id);
//        MyGame.GameManager.LoadScene("Main",false);
//        Decided?.Invoke(true);
//    }
//
//    public void OnClickNo()
//    {
//        Decided?.Invoke(false);
//        Hide();
//    }
//}
