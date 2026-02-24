using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePuzzleController : MonoBehaviour
{
    public static SlidePuzzleController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public class SlidePiece
    {
        public GameObject pieceObject;
        public Vector2Int currentPosition;
        public int[] connectableFlag = new int[4]; // 上、右、下、左 の順で接続可能かを示すフラグ（0: 接続不可, 1: 接続可能）
        
        public SlidePiece(GameObject obj, Vector2Int currentPos, int[] connectableFlag)
        {
            Debug.Assert(connectableFlag.Length == 4, "connectableFlagの要素は4つでなくてはいけない。");
            
            currentPosition = currentPos;
            pieceObject = obj;
            this.connectableFlag = connectableFlag;
        }
    }

    private void Start()
    {

    
    
    }

}
