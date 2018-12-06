﻿using System.Collections;
using System.Collections.Generic;

using Gameplay.Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace Gameplay
{
    public class Board : MonoBehaviour
    {
        public static Board board;
        void Start()
        {
            board = this;

            Transform parent = new GameObject("Tiles").transform;
            parent.transform.SetParent(this.transform);

            Tilemap renderer = GetComponentInChildren<Tilemap>();
            Vector3Int size = renderer.size;
            Vector3Int origin = renderer.origin;

            tiles = new Tiles.Tile[size.x, size.y];
            System.Action post_initializer = () => { };

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int tilemap_position = origin + new Vector3Int(x, y, 0);
                    UnityEngine.Tilemaps.Tile render_tile = renderer.GetTile<UnityEngine.Tilemaps.Tile>(tilemap_position);

                    if (render_tile != null)
                    {
                        Tiles.Tile tile = new Tiles.Tile(
                            position: new Vector2Int(x, y),
                            prefab: tilePrefabs.First(candidate => candidate.name == render_tile.name).gameObject,
                            parent: parent,
                            post_initializer: ref post_initializer
                        );
                        tiles[x, y] = tile;
                    }
                }
            }

            post_initializer();
        }
        public Tiles.TileObject[] tilePrefabs;
        public Tiles.Tile[,] tiles;
        public Tiles.Tile this[int a, int b]
        {
            get
            {
                return tiles[a, b];
            }
        }
    }
}