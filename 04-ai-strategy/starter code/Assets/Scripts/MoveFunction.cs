using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFunction
{
    BoardManager board;
    List<MoveData> moves = new List<MoveData>();
    private ChessPiece piece;
    private Vector2 position;

    public List<MoveData> GetMoves(ChessPiece piece, Vector2 position)
    {
        this.piece = piece;
        this.position = position;

        switch(piece.Type)
        {
            case ChessPiece.PieceType.PAWN:
                GetPawnMoves();
            break;
            case ChessPiece.PieceType.KNIGHT:
                GetKnightMoves();
            break;
            case ChessPiece.PieceType.BISHOP:
                GetBishopMoves();
            break;
            case ChessPiece.PieceType.ROOK:
                GetRookMoves();
            break;
            case ChessPiece.PieceType.QUEEN:
                GetQueenMoves();
            break;
            case ChessPiece.PieceType.KING:
                GetKingMoves();
            break;
        }

        return moves;
    }

    void GetPawnMoves()
    {
        if (piece.Team == PlayerTeam.BLACK)
        {
            int limit = piece.HasMoved ? 2 : 3;
            GenerateMove(limit, new Vector2(0, -1));

            Vector2 diagLeft = new Vector2(position.x - 1, position.y - 1);
            Vector2 diagRight = new Vector2(position.x + 1, position.y - 1);

            TileData dl = null;
            TileData dr = null;

            if (IsOnBoard(diagLeft))            
                dl = board.GetTileFromBoard(diagLeft);
            
            if (IsOnBoard(diagRight))            
                dr = board.GetTileFromBoard(diagRight);            

            if (dl != null && ContainsPiece(dl) && IsEnemy(dl))            
                CheckAndStoreMove(diagLeft);
            
            if (dr != null && ContainsPiece(dr) && IsEnemy(dr))            
                CheckAndStoreMove(diagRight);            
        }
        else if (piece.Team == PlayerTeam.WHITE)
        {
            int limit = piece.HasMoved ? 2 : 3;
            GenerateMove(limit, new Vector2(0, 1));
            
            Vector2 diagLeft = new Vector2(position.x - 1, position.y + 1);
            Vector2 diagRight = new Vector2(position.x + 1, position.y + 1);

            TileData dl = null;
            TileData dr = null;

            if (IsOnBoard(diagLeft))            
                dl = board.GetTileFromBoard(diagLeft);            
            
            if (IsOnBoard(diagRight))            
                dr = board.GetTileFromBoard(diagRight);            

            if (dl != null && ContainsPiece(dl) && IsEnemy(dl))            
                CheckAndStoreMove(diagLeft);

            if (dr != null && ContainsPiece(dr) && IsEnemy(dr))            
                CheckAndStoreMove(diagRight);            
        }
    }

    void GetRookMoves()
    {
        GenerateMove(9, new Vector2(0, 1));
        GenerateMove(9, new Vector2(0, -1));
        GenerateMove(9, new Vector2(1, 0));
        GenerateMove(9, new Vector2(-1, 0));
    }

    void GetKnightMoves()
    {
        Vector2 move;

        move = new Vector2(position.x + 1, position.y - 2);
        CheckAndStoreMove(move);
        move = new Vector2(position.x + 1, position.y + 2);
        CheckAndStoreMove(move);
        move = new Vector2(position.x - 1, position.y + 2);
        CheckAndStoreMove(move);
        move = new Vector2(position.x - 1, position.y - 2);
        CheckAndStoreMove(move);

        move = new Vector2(position.x + 2, position.y + 1);
        CheckAndStoreMove(move);
        move = new Vector2(position.x + 2, position.y - 1);
        CheckAndStoreMove(move);
        move = new Vector2(position.x - 2, position.y + 1);
        CheckAndStoreMove(move);
        move = new Vector2(position.x - 2, position.y - 1);
        CheckAndStoreMove(move);
    }

    void GetBishopMoves()
    {
        GenerateMove(9, new Vector2(1, 1));
        GenerateMove(9, new Vector2(1, -1));
        GenerateMove(9, new Vector2(-1, 1));
        GenerateMove(9, new Vector2(-1, -1));
    }

    void GetQueenMoves()
    {
        GetBishopMoves();
        GetRookMoves();
    }

    void GetKingMoves()
    {
        for (int x = -1; x <= 1; x++)        
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)                
                    continue;
                
                CheckAndStoreMove(new Vector2(position.x + x, position.y + y));
            }        
    }
    
    void GenerateMove(int limit, Vector2 direction)
    {
        for (int i = 1; i < limit; ++i)
        {
            Vector2 move = position + direction * i;
            if (IsOnBoard(move) && ContainsPiece(board.GetTileFromBoard(move)))
            {
                if (IsEnemy(board.GetTileFromBoard(move)) && piece.Type != ChessPiece.PieceType.PAWN)                
                    CheckAndStoreMove(move);
                
                break;
            }
            CheckAndStoreMove(move);
        }
    }

    void CheckAndStoreMove(Vector2 move)
    {
        if (IsOnBoard(move) && (!ContainsPiece(board.GetTileFromBoard(move)) || 
            IsEnemy(board.GetTileFromBoard(move))))
        {
            MoveData M = new MoveData
            {
                firstPosition = board.GetTileFromBoard(position),
                pieceMoved = piece,
                secondPosition = board.GetTileFromBoard(move)
            };

            if (M.secondPosition != null)            
                M.pieceKilled = M.secondPosition.CurrentPiece;            

            moves.Add(M);
        }
    }

    public MoveFunction(BoardManager board)
    {
        this.board = board;
    }

    bool IsOnBoard(Vector2 point)
    {
        if (point.x >= 0 && point.y >= 0 && point.x < 8 && point.y < 8)        
            return true;                        
        
        return false;        
    }

    bool ContainsPiece(TileData tile)
    {
        if (!IsOnBoard(tile.Position))        
            return false;        

        if (tile.CurrentPiece != null)        
            return true;        
                
        return false;        
    }

    bool IsEnemy(TileData tile)
    {
        if (piece.Team != tile.CurrentPiece.Team)        
            return true;        
                
        return false;        
    }
}
