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
    public List<Tile> tile_array;
    public Tilemap tilemap;
    public String file_path;
    public List<int> tile_probs;

    private void Start() {
        tile_array = new List<Tile>();
        tilemap = GetComponent<Tilemap>();
        load_tiles();
        gen_terrain();

        for (int i = 1; i < tile_probs.Count; i++)
            tile_probs[i] += tile_probs[i - 1];
    }

    public void load_tiles() {
        var tiles = Resources.LoadAll<Tile>(file_path);
        tile_array.Clear();
        for (int i = 0;i < tiles.Length;i++) {
            tile_array.Add(tiles[i]);
        }
    }

    int SelectRandomCategory() {
        // �����ܺͣ���ȷ����������֮��Ϊ1
        
        float total = 0;
        foreach (float prob in tile_probs) {
            total += prob;
        }

        // ����һ��0���ܺ�֮��������
        float randomPoint = UnityEngine.Random.value * total;

        // �����������飬�ҵ��������Ӧ�����
        for (int i = 0; i < tile_probs.Count; i++) {
            if (randomPoint < tile_probs[i]) {
                return i;
            }
            else {
                randomPoint -= tile_probs[i];
            }
        }

        // ����������û�б�ѡ�У������ϲ�Ӧ�÷��������������һ�����
        return tile_probs.Count - 1;
    }
    public void gen_terrain() {

        for(int i = lower_left.x; i < upper_right.x;i++) {
            for(int j = lower_left.y; j < upper_right.y; j++) {
                int randomIx = SelectRandomCategory();
                tilemap.SetTile(new Vector3Int(i, j, 0), tile_array[randomIx]);
            }
        }
    }
}
