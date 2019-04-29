using System.Collections.Generic;
using UnityEngine;

public class StorePanel : ShowHidable
{
    [SerializeField]private  List<StoreTileUI> _tiles = new List<StoreTileUI>();

    private void Awake()
    {
        _tiles.ForEach(ui => ui.Clicked +=TileUIOnClicked);
    }

    private void TileUIOnClicked(StoreTileUI tile)
    {
        
    }
}