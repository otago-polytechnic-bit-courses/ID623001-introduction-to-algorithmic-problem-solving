using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData
{
    public TileData firstPosition = null;
    public TileData secondPosition = null;
    public ChessPiece pieceMoved = null;
    public ChessPiece pieceKilled = null;
    public int score = int.MinValue;
}
