﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Board : MonoBehaviour
    {
        static Board board;
        void Start()
        {
            board = this;
        }
        public Tile tilePrefab;
        public float paddingToTileRatio;
        Tile[,] tiles;
        public Tile this[int a, int b]
        {
            get
            {
                return tiles[a, b];
            }
            set
            {
                tiles[a, b] = value;
            }
        }
        public static void Renew(int board_side_length)
        {
            Player.ReinitializePlayers(true);

            if (board_side_length % 2 == 0 || board_side_length <= 1) throw new System.Exception("Board size invalid.");
            if (board.tiles != null)
                for (int x = 0; x < board.tiles.GetLength(0); x++)
                    for (int y = 0; y < board.tiles.GetLength(1); y++)
                        Destroy(board[x, y].gameObject);

            float board_size = board.GetComponent<RectTransform>().sizeDelta.x;
            float tile_size = board_size / (board_side_length + 2);
            GridLayoutGroup grid = board.GetComponent<GridLayoutGroup>();

            grid.cellSize = Vector2.one * tile_size * (1.0f - board.paddingToTileRatio);
            grid.spacing = Vector2.one * tile_size * board.paddingToTileRatio;

            board.tiles = new Tile[board_side_length + 2, board_side_length + 2];
            int player_x_spawn = Mathf.CeilToInt(board_side_length / 2.0f);
            for (int x = 0; x < board_side_length + 2; x++)
            {
                for (int y = 0; y < board_side_length + 2; y++)
                {
                    Tile tile = Instantiate(board.tilePrefab, board.transform).GetComponent<Tile>();
                    tile.transform.localScale = Vector3.one;

                    if (x == player_x_spawn && (y == 0 || y == board_side_length + 1)) tile.Owner = Player.players[y == 0 ? 1 : 0];
                    else
                    if (x == 0 || x == board_side_length + 1 || y == 0 || y == board_side_length + 1) tile.Cost = int.MaxValue;
                    else tile.UpdateColor();
                    board[x, y] = tile;
                }
            }
        }

        void Update()
        {
            for (int i = 1; i <= 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    Debug.Log("Pressed " + i);
                    Board.Renew(i);
                }
            }
        }
    }
}