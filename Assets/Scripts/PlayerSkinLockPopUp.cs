using UnityEngine;
using UnityEngine.UI;

public class PlayerSkinLockPopUp : ShowHidable
{

    [SerializeField] private Text _titleTxt, _valueTxt;
    [SerializeField] private Image _img;
    private ViewModel _mViewModel;

    public ViewModel MViewModel
    {
        get { return _mViewModel; }
        set
        {
            _titleTxt.text = value.Title;
            _valueTxt.text = value.ValueTxt;
            _img.sprite = value.Image;
            _mViewModel = value;
        }
    }


    public void OnClickOk()
    {
        Hide();
    }

    public struct ViewModel
    {
        public string Title { get; set; }
        public string ValueTxt { get; set; }
        public Sprite Image { get; set; }
    }

}