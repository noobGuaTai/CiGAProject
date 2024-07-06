using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ground_timemap : MonoBehaviour
{
    public Vector2Int lower_left;
    public Vector2Int upper_right;
    List<Tile> tile_array;
    Tilemap tilemap;

    private void Start() {
        tile_array = new List<Tile>();
        tilemap = GetComponent<Tilemap>();
    }

    public void load_tiles() {
        var tiles = Resources.LoadAll<Tile>("");
        tile_array.Clear();
        for (int i = 0;i < tiles.Length;i++) {
            tile_array.Append(tiles[i]);
        }
    }

    public void gen_terrain() { 
        for(int i = lower_left.x; i < upper_right.x;i++) {
            for(int j = lower_left.y; j < upper_right.y; j++) {
                int randomIx = UnityEngine.Random.Range(0, tile_array.Count);
                tilemap.SetTile(new Vector3Int(i, j, 0), tile_array[randomIx]);
            }
        }
    }
}
