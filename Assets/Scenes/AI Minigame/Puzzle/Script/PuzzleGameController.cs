using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGameController : MonoBehaviour
{
    public int[][] board;
    
    [SerializeField] int boardSizeX = 5;
    [SerializeField] int boardSizeY = 6;

    private void Start()
    {
        board = new int[boardSizeX][];
        for (int i = 0; i < boardSizeX; i++)
        {
            board[i] = new int[boardSizeY];
            for (int j = 0; j < boardSizeY; j++)
            {
                board[i][j] = Random.Range(1, 5); // ‘S‚Ä‚Ìƒ}ƒX‚É1‚©‚ç4‚ðŠ„‚è“–‚Ä‚é
            }
        }
    }



}
