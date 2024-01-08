using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTeam
{
    NONE = -1,
    WHITE,
    BLACK,
};

public class GameManager : MonoBehaviour
{
    BoardManager board;
    public PlayerTeam playerTurn;
    bool kingDead = false;
    public GameObject fromHighlight;
    public GameObject toHighlight;

    private static GameManager instance;    
    public static GameManager Instance
    {
        get { return instance; }
    }
    private bool isCoroutineExecuting = false;

    private void Awake()
    {
        if (instance == null)        
            instance = this;        
        else if (instance != this)        
            Destroy(this);    
    }    

    void Start()
    {
        board = BoardManager.Instance;        
        board.SetupBoard();
    }

    private void Update()
    {
        StartCoroutine(DoAIMove());
    }

    IEnumerator DoAIMove()
    {       
        if(isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        if (kingDead)                    
            Debug.Log(playerTurn + " wins!");        
        else if (!kingDead)
        {                     
            MoveFunction movement = new MoveFunction(board);
            MoveData move = null;
            for (int y = 0; y < 8; y++)                
                for (int x = 0; x < 8; x++)            
                {
                    TileData tile = board.GetTileFromBoard(new Vector2(x, y));
                    if(tile.CurrentPiece != null && tile.CurrentPiece.Team == playerTurn)
                    {
                        List<MoveData> pieceMoves = movement.GetMoves(tile.CurrentPiece, tile.Position);
                        if(pieceMoves.Count > 0)                        
                            move = pieceMoves[0];                        
                    }
                }
        
            RemoveObject("Highlight");
            ShowMove(move);

            yield return new WaitForSeconds(1);
            
            SwapPieces(move);  
            if(!kingDead)                
                UpdateTurn();     

            isCoroutineExecuting = false;                                                                                                         
        }
    }

    public void SwapPieces(MoveData move)
    {
        TileData firstTile = move.firstPosition;
        TileData secondTile = move.secondPosition;        

        firstTile.CurrentPiece.MovePiece(new Vector2(secondTile.Position.x, secondTile.Position.y));

        CheckDeath(secondTile);
                        
        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.chessPosition = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;            
    }   

    private void UpdateTurn()
    {     
        playerTurn = playerTurn == PlayerTeam.WHITE ? PlayerTeam.BLACK : PlayerTeam.WHITE;        
    }

    void CheckDeath(TileData _secondTile)
    {
        if (_secondTile.CurrentPiece != null)        
            if (_secondTile.CurrentPiece.Type == ChessPiece.PieceType.KING)           
                kingDead = true;                           
            else
                Destroy(_secondTile.CurrentPiece.gameObject);        
    }

    void ShowMove(MoveData move)
    {
        GameObject GOfrom = Instantiate(fromHighlight);
        GOfrom.transform.position = new Vector2(move.firstPosition.Position.x, move.firstPosition.Position.y);
        GOfrom.transform.parent = transform;

        GameObject GOto = Instantiate(toHighlight);
        GOto.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
        GOto.transform.parent = transform;
    }

    public void RemoveObject(string text)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(text);
        foreach (GameObject GO in objects)
            Destroy(GO);        
    }
}
