using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDisplayer : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private ITilemap iTileMap;

    [ContextMenu("Set Layer Order")]
    public void SetLayerOrder()
    {
        BoundsInt bounds = tileMap.cellBounds;

        TileBase[] allTiles = tileMap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileData data = new TileData();

                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    tile.GetTileData(new Vector3Int(x, y, 0), iTileMap, ref data);

                    Debug.Log(data.gameObject);

                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                }
            }
        }
    }
}
