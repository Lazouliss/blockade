using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using System.Linq;



public class AlgoBoard
{
    private uint CASE_GAGNANTE_playerJaune_1 = 36;
    private uint CASE_GAGNANTE_playerJaune_2 = 40;
    private uint CASE_GAGNANTE_playerRouge_1 = 113;
    private uint CASE_GAGNANTE_playerRouge_2 = 117;
    private Square[] cases = new Square[154];
    private Player playerJaune;
    private Player playerRouge;

    public AlgoBoard(Player _playerJaune, Player _playerRouge) {
        for (uint i = 0; i < cases.Length; i++)
        {
            Square caseAlo = new Square(i);
            caseAlo.setIdSquare(i);
            cases[i] = caseAlo;
        }
        this.playerJaune = _playerJaune;
        this.playerRouge = _playerRouge;
        AssignNeighbors();
    }
    private void AssignNeighbors()
    {
        for(uint i = 0; i < this.cases.Length; i++)
        {
            uint row = i / 11;
            uint col = i % 11;
            uint index = 0;

             // Lower Square
            if (row > 0 && row <= 13)
            {
                index = i - 11;
                if(index < 0)
                    this.cases[i].SetLowerSquare(cases[-index]);
                else
                    this.cases[i].SetLowerSquare(cases[index]);
            }
            else
                this.cases[i].SetLowerSquare(null);

            // Upper Square  
            if (row < 13 && row >= 0)
                {
                    this.cases[i].SetUpperSquare(cases[i + 11]);
                }  
            else
                this. cases[i].SetUpperSquare(null);

            // Left Square
            if (col > 0 && col <= 10)
                this.cases[i].SetLeftSquare(cases[i - 1]);
            else
                this.cases[i].SetLeftSquare(null);

            // Right Square
            if (col < 10 && col >= 0)
                this.cases[i].SetRightSquare(cases[i + 1]);
            else
                this.cases[i].SetRightSquare(null);
        }
    }

    public (bool, (uint, uint), List<Common.Direction>) IsPathPossible(Common.DTOPawn DTOPawn) {
        uint xCoordStart = DTOPawn.startPos.Item1;
        uint yCoordStart = DTOPawn.startPos.Item2;

        uint xCoordDest = (uint) DTOPawn.destPos.Item1;
        uint yCoordDest = (uint) DTOPawn.destPos.Item2;

        Square StartPositionSquare = getSquareById(getPosition(xCoordStart, yCoordStart));
        Square DestPositionSquare = getSquareById(getPosition(xCoordDest, yCoordDest));
        
        (bool, Square[]) verif = IsPathPossibleRecursive(StartPositionSquare, DestPositionSquare);
        return (verif.Item1, DTOPawn.startPos, GetPath(verif.Item2));
    }

    public void afficherBoard(List<uint> pos) {
        int count = 0;
        string tableau = "";
        foreach (Square square in cases) {
            if (count % 11 == 0) {
                tableau += "\n";
            }
            if (square.HasPlayer()) {
                if (pos.Contains(square.getIdCase())){
                    tableau += square.getPlayerID() + "*";
                } else {
                    tableau += square.getPlayerID() + " ";
                }
            }
            else if (pos.Contains(square.getIdCase())){
                tableau += "* ";
            } else if (square.getUpperSquare() == null || square.getLowerSquare() == null){
                tableau += "_ " ;
            } else if (square.getRightSquare() == null || square.getLeftSquare() == null){
                tableau += "| ";
            } else {
                tableau += "- ";
            }
            count++;
        }
        Debug.Log(tableau);
    }

    public (bool, List<Square>, int) getChemin((uint, uint) startPos, (uint, uint) endPos) {
        Square StartPositionSquare = getSquareById(getPosition(startPos.Item1, startPos.Item2));
        Square DestPositionSquare = getSquareById(getPosition(endPos.Item1, endPos.Item2));
        return getCheminRecursive(StartPositionSquare, DestPositionSquare, new List<Square>(), false, 0);
    }
    private (bool, List<Square>, int) getCheminRecursive(Square current, Square destination, List<Square> currentPath, bool isJumping, int moves) {
        //Debug.Log("Current: " + current.getIdCase() + " Destination: " + destination.getIdCase() + " Moves: " + moves + " IsJumping: " + isJumping);
        List<Square> path = new List<Square>(currentPath) { current };

        if (current.getIdCase() == destination.getIdCase()) {
            return (true, path, moves);  
        }

        if (moves >= 2) {
            return (false, new List<Square>(), moves);
        }

        List<Square> nextSquares = new List<Square> {
            current.getUpperSquare(),
            current.getLowerSquare(),
            current.getRightSquare(),
            current.getLeftSquare()
        };

        foreach (Square next in nextSquares) {
            if (next != null) {
                bool nextIsJumping = next.HasPlayer();
                int newMoves = 0;

                if ((isJumping && !nextIsJumping) || (!isJumping && nextIsJumping)) {
                    newMoves += 1;
                } else if (!isJumping && !nextIsJumping) { 
                    newMoves += 1;
                }

                var (found, resultPath, resultMoves) = getCheminRecursive(next, destination, path, nextIsJumping, moves + newMoves);
                if (found) {
                    return (true, resultPath, resultMoves); 
                }
            }
        }
        return (false, new List<Square>(), moves);
    }
    
    private (bool, Square[]) IsPathPossibleRecursive(Square current, Square destination) {
        if (current.getIdCase() == destination.getIdCase()) return (true, new Square[] { current });


        List<Square> nextSquares = new List<Square> {
            current.getUpperSquare(),
            current.getLowerSquare(),
            current.getRightSquare(),
            current.getLeftSquare()
        };

        foreach (Square next in nextSquares) {
            (bool, Square[]) verif = IsPathPossibleRecursive(next, destination);
            if (next != null && CanMoveOrJump(current, next) && verif.Item1) {
                Square[] newPath = new Square[verif.Item2.Length + 1];
                newPath[0] = current;
                verif.Item2.CopyTo(newPath, 1);
                return (true, newPath);
            }
        }

        return (false, new Square[0]);
    }

    private bool CanMoveOrJump(Square fromSquare, Square toSquare) {
        if (toSquare != null && !toSquare.HasPlayer()) return true;

        Common.Direction direction = GetDirection(fromSquare, toSquare);
        Square jumpOver = toSquare;
        Square jumpTo = GetNextSquareInDirection(jumpOver, direction); 

        return jumpOver != null && jumpOver.HasPlayer() && jumpTo != null;
    }

    private Common.Direction GetDirection(Square fromSquare, Square toSquare) {
        if (toSquare == fromSquare.getUpperSquare()) return Common.Direction.UP;
        if (toSquare == fromSquare.getLowerSquare()) return Common.Direction.DOWN;
        if (toSquare == fromSquare.getRightSquare()) return Common.Direction.RIGHT;
        if (toSquare == fromSquare.getLeftSquare()) return Common.Direction.LEFT;
        return Common.Direction.UP;
    }

    private Square GetNextSquareInDirection(Square square, Common.Direction direction) {
        switch (direction) {
            case Common.Direction.UP: return square.getUpperSquare();
            case Common.Direction.DOWN: return square.getLowerSquare();
            case Common.Direction.RIGHT: return square.getRightSquare();
            case Common.Direction.LEFT: return square.getLeftSquare();
            default: return null;
        }
    }

    public uint getPosition(uint xcoord ,uint ycoord) {
        return ycoord * 11 + xcoord;
    }
    public (uint, uint) getCoordinates(uint idCase) {
        uint y = idCase / 11;
        uint x = idCase % 11;
        return (x, y);
    }
    public Square getSquareById(uint id){
        return cases[id];
    }
    public void addWall((uint,uint) firstCoords, (uint,uint) secondCoords, Common.Direction direction) {
        (uint firstX, uint firstY) = firstCoords;
        Square firstSquare = getSquareById(getPosition(firstX, firstY));

        (uint secondX, uint secondY) = secondCoords;
        Square secondSquare = getSquareById(getPosition(secondX, secondY));

        switch(direction) {
            case Common.Direction.UP:
                Square firstSquareUp = getSquareById(getPosition(firstX, firstY) + 11);
                Square secondSquareUp = getSquareById(getPosition(secondX, secondY) + 11);
                firstSquare.SetUpperSquare(null);
                secondSquare.SetUpperSquare(null);
                firstSquareUp.SetLowerSquare(null);
                secondSquareUp.SetLowerSquare(null);
                break;
            case Common.Direction.DOWN:
                Square firstSquareDown = getSquareById(getPosition(firstX, firstY) - 11);
                Square secondSquareDown = getSquareById(getPosition(secondX, secondY) - 11);
                firstSquare.SetLowerSquare(null);
                secondSquare.SetLowerSquare(null);
                firstSquareDown.SetUpperSquare(null);
                secondSquareDown.SetUpperSquare(null);
                break;
            case Common.Direction.LEFT:
                Square firstSquareLeft = getSquareById(getPosition(firstX, firstY) - 1);
                Square secondSquareLeft = getSquareById(getPosition(secondX, secondY) - 1);
                firstSquare.SetLeftSquare(null);
                secondSquare.SetLeftSquare(null);
                firstSquareLeft.SetRightSquare(null);
                secondSquareLeft.SetRightSquare(null);
                break;
            case Common.Direction.RIGHT:
                Square firstSquareRight = getSquareById(getPosition(firstX, firstY) + 1);
                Square secondSquareRight = getSquareById(getPosition(secondX, secondY) + 1);
                firstSquare.SetRightSquare(null);
                secondSquare.SetRightSquare(null);
                firstSquareRight.SetLeftSquare(null);
                secondSquareRight.SetLeftSquare(null);
                break;
        }
    }
    public List<Common.Direction> GetPath(Square[] squares) 
    {
        List<Common.Direction> path = new List<Common.Direction>{};

        for(int i = 0; i < squares.Length - 1; i++)
        {
            Common.Direction dir = GetDirection(squares[i], squares[i + 1]);
            path.Add(dir);
        }
        return path;
    }
    public void placePawn(uint xCoord, uint yCoord , Player player) {
        foreach (Square square in cases) {
            if (square.getIdCase() == getPosition(xCoord, yCoord)) {
                square.SetPlayerID(player.getPlayerID());
            }
        }
    }

    public bool IsWallplaceable(Common.DTOWall DTOWall, Player playingPlayer)
    {
        uint xCoord1 = DTOWall.coord1.Item1;
        uint yCoord1 = DTOWall.coord1.Item2;

        uint xCoord2 = DTOWall.coord2.Item1;
        uint yCoord2 = DTOWall.coord2.Item2;

        Square firstSquare = getSquareById(getPosition(xCoord1, yCoord1));
        Square secondSquare = getSquareById(getPosition(xCoord2, yCoord2));

        Square[] winningSquares = playingPlayer.getWinningSquaresPosition()
            .Select(id => getSquareById(id))
            .ToArray();
        
        Square nextSquareFirstSquare = GetNextSquareInDirection(firstSquare, DTOWall.direction);
        Square nextSquareSecondSquare = GetNextSquareInDirection(secondSquare, DTOWall.direction);

        if (GetNextSquareInDirection(firstSquare, DTOWall.direction) == null || GetNextSquareInDirection(secondSquare, DTOWall.direction) == null ) {
            return false;
        }
        
        switch(DTOWall.direction) {
            case Common.Direction.UP:
                firstSquare.SetUpperSquare(null);
                secondSquare.SetUpperSquare(null);
                foreach (uint pionSquare in playingPlayer.getPositionsPions()) {
                    if (!IsPathPossibleRecursive(getSquareById(pionSquare), winningSquares[0]).Item1 && !IsPathPossibleRecursive(getSquareById(pionSquare), winningSquares[1]).Item1) {
                        return false;
                    }
                }
                firstSquare.SetUpperSquare(nextSquareFirstSquare);
                secondSquare.SetUpperSquare(nextSquareSecondSquare);
                if (playingPlayer.getHorizontalWallsCount() <= 0) {
                    return false;
                }
                break;
            case Common.Direction.DOWN:
                firstSquare.SetLowerSquare(null);
                secondSquare.SetLowerSquare(null);
                foreach (uint pionSquare in playingPlayer.getPositionsPions()) {
                    if (!IsPathPossibleRecursive(getSquareById(pionSquare), winningSquares[0]).Item1 && !IsPathPossibleRecursive(getSquareById(pionSquare), winningSquares[1]).Item1) {
                        return false;
                    }
                }
                firstSquare.SetLowerSquare(nextSquareFirstSquare);
                secondSquare.SetLowerSquare(nextSquareSecondSquare);
                if (playingPlayer.getHorizontalWallsCount() <= 0) {
                    return false;
                }
                break;
            case Common.Direction.LEFT:
                firstSquare.SetLeftSquare(null);
                secondSquare.SetLeftSquare(null);
                foreach (uint pionSquare in playingPlayer.getPositionsPions()) {
                    if (!IsPathPossibleRecursive(getSquareById(pionSquare), winningSquares[0]).Item1 && !IsPathPossibleRecursive(getSquareById(pionSquare), winningSquares[1]).Item1) {
                        return false;
                    }
                }
                firstSquare.SetLeftSquare(nextSquareFirstSquare);
                secondSquare.SetLeftSquare(nextSquareSecondSquare);
                if (playingPlayer.getVerticalWallsCount() <= 0) {
                    return false;
                }
                break;
            case Common.Direction.RIGHT:
                firstSquare.SetRightSquare(null);
                secondSquare.SetRightSquare(null);
                foreach (uint pionSquare in playingPlayer.getPositionsPions()) {
                    if (!IsPathPossibleRecursive(getSquareById(pionSquare), winningSquares[0]).Item1 && !IsPathPossibleRecursive(getSquareById(pionSquare), winningSquares[1]).Item1) {
                        return false;
                    }
                }
                firstSquare.SetRightSquare(nextSquareFirstSquare);
                secondSquare.SetRightSquare(nextSquareSecondSquare);
                if (playingPlayer.getVerticalWallsCount() <= 0) {
                    return false;
                }
                break;
        }
        
        return true;
    }

    public void initBoard()
    {
        foreach (Square square in cases)
        {
            if (square.getIdCase() == CASE_GAGNANTE_playerJaune_1 || square.getIdCase() == CASE_GAGNANTE_playerJaune_2)
            {
                square.SetWinningSquare(true);
                square.SetPlayerID(playerJaune.getPlayerID());
            }

            if (square.getIdCase() == CASE_GAGNANTE_playerRouge_1 || square.getIdCase() == CASE_GAGNANTE_playerRouge_2)
            {
                square.SetWinningSquare(true);
                square.SetPlayerID(playerRouge.getPlayerID());
            }
        }
    }

    public bool checkWin(Player player)
    {
        bool win = false;
        if (player.getPlayerID() == 1 && (this.cases[113].getPlayerID() == player.getPlayerID() || this.cases[117].getPlayerID() == player.getPlayerID()))
        {
            win = true;
        }
        else if (player.getPlayerID() == 2 && (this.cases[36].getPlayerID() == player.getPlayerID() || this.cases[40].getPlayerID() == player.getPlayerID()))
        {
            win = true;
        }

        return win;
    }
}
