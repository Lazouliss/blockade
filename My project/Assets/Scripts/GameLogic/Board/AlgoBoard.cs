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

    public AlgoBoard(Player _playerJaune, Player _playerRouge)
    {
        for (uint i = 0; i < cases.Length; i++)
        {
            Square caseAlo = new Square(i);
            caseAlo.setIdSquare(i);
            cases[i] = caseAlo;
        }
        this.playerJaune = _playerJaune;
        this.playerJaune.setWinningSquaresPosition(new uint[] { CASE_GAGNANTE_playerRouge_1, CASE_GAGNANTE_playerRouge_2 });
        this.playerJaune.setPositionsPions(new uint[] { CASE_GAGNANTE_playerJaune_1, CASE_GAGNANTE_playerJaune_2 });
        this.playerRouge = _playerRouge;
        this.playerRouge.setWinningSquaresPosition(new uint[] { CASE_GAGNANTE_playerJaune_1, CASE_GAGNANTE_playerJaune_2 });
        this.playerRouge.setPositionsPions(new uint[] { CASE_GAGNANTE_playerRouge_1, CASE_GAGNANTE_playerRouge_2 });
        AssignNeighbors();
    }
    private void AssignNeighbors()
    {
        for (uint i = 0; i < this.cases.Length; i++)
        {
            uint row = i / 11;
            uint col = i % 11;
            uint index = 0;

            // Lower Square
            if (row > 0 && row <= 13)
            {
                index = i - 11;
                if (index < 0)
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
                this.cases[i].SetUpperSquare(null);

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

    public bool IsPathPossible((uint, uint) coordStart, (uint, uint) coordDest)
    {
        uint xCoordStart = coordStart.Item1;
        uint yCoordStart = coordStart.Item2;

        uint xCoordDest = coordDest.Item1;
        uint yCoordDest = coordDest.Item2;

        Square StartPositionSquare = getSquareById(getPosition(xCoordStart, yCoordStart));
        Square DestPositionSquare = getSquareById(getPosition(xCoordDest, yCoordDest));

        (bool, List<Square>) verif = IsPathPossibleRecursive(StartPositionSquare, DestPositionSquare, new List<Square>());
        return verif.Item1;
    }

    public void afficherBoard(List<uint> pos)
    {
        int count = 0;
        string tableau = "";
        foreach (Square square in cases)
        {
            if (count % 11 == 0)
            {
                tableau += "\n";
            }
            if (square.HasPlayer())
            {
                if (pos.Contains(square.getIdCase()))
                {
                    tableau += square.getPlayerID() + "*";
                }
                else
                {
                    tableau += square.getPlayerID() + " ";
                }
            }
            else if (pos.Contains(square.getIdCase()))
            {
                tableau += "* ";
            }
            else if (square.getUpperSquare() == null || square.getLowerSquare() == null)
            {
                tableau += "_ ";
            }
            else if (square.getRightSquare() == null || square.getLeftSquare() == null)
            {
                tableau += "| ";
            }
            else
            {
                tableau += "- ";
            }
            count++;
        }
        Debug.Log(tableau);
    }

    public (bool, List<Square>, int) getChemin((uint, uint) startPos, (uint, uint) endPos)
    {
        Square StartPositionSquare = getSquareById(getPosition(startPos.Item1, startPos.Item2));
        Square DestPositionSquare = getSquareById(getPosition(endPos.Item1, endPos.Item2));
        return getCheminRecursive(StartPositionSquare, DestPositionSquare, new List<Square>(), false, 0);
    }
    private (bool, List<Square>, int) getCheminRecursive(Square current, Square destination, List<Square> currentPath, bool isJumping, int moves)
    {
        //Debug.Log("Current: " + current.getIdCase() + " Destination: " + destination.getIdCase() + " Moves: " + moves + " IsJumping: " + isJumping);
        List<Square> path = new List<Square>(currentPath) { current };

        if (current.getIdCase() == destination.getIdCase())
        {
            return (true, path, moves);
        }

        if (moves >= 2)
        {
            return (false, new List<Square>(), moves);
        }

        List<Square> nextSquares = new List<Square> {
            current.getUpperSquare(),
            current.getLowerSquare(),
            current.getRightSquare(),
            current.getLeftSquare()
        };

        foreach (Square next in nextSquares)
        {
            if (next != null && next != path[0])
            {
                bool nextIsJumping = next.HasPlayer();
                int newMoves = 0;

                if ((isJumping && !nextIsJumping) || (!isJumping && nextIsJumping))
                {
                    newMoves += 1;
                }
                else if (!isJumping && !nextIsJumping)
                {
                    newMoves += 1;
                }

                var (found, resultPath, resultMoves) = getCheminRecursive(next, destination, path, nextIsJumping, moves + newMoves);
                if (found)
                {
                    return (true, resultPath, resultMoves);
                }
            }
        }
        return (false, new List<Square>(), moves);
    }

    private (bool, List<Square>) IsPathPossibleRecursive(Square current, Square destination, List<Square> currentPath)
    {
        List<Square> path = new List<Square>(currentPath) { current };
        if (current.getIdCase() == destination.getIdCase()) return (true, currentPath);


        List<Square> nextSquares = new List<Square> {
            current.getUpperSquare(),
            current.getLowerSquare(),
            current.getRightSquare(),
            current.getLeftSquare()
        };

        foreach (Square next in nextSquares)
        {
            if (next != null && !path.Contains(next))
            {
                (bool, List<Square>) verif = IsPathPossibleRecursive(current, next, path);
                if (verif.Item1)
                {
                    return (true, verif.Item2);
                }
            }
        }

        return (false, path);
    }
    public bool IsPathPossibleAStar((uint, uint) coordStart, (uint, uint) coordDest)
    {
        uint xCoordStart = coordStart.Item1;
        uint yCoordStart = coordStart.Item2;

        uint xCoordDest = coordDest.Item1;
        uint yCoordDest = coordDest.Item2;

        Square StartPositionSquare = getSquareById(getPosition(xCoordStart, yCoordStart));
        Square DestPositionSquare = getSquareById(getPosition(xCoordDest, yCoordDest));

        List<Square> openList = new List<Square> { StartPositionSquare };
        List<Square> closedList = new List<Square>();

        while (openList.Count > 0)
        {
            Square currentSquare = openList[0];
            openList.RemoveAt(0);
            closedList.Add(currentSquare);

            if (currentSquare.getIdCase() == DestPositionSquare.getIdCase())
            {
                return true;
            }

            List<Square> nextSquares = new List<Square> {
                currentSquare.getUpperSquare(),
                currentSquare.getLowerSquare(),
                currentSquare.getRightSquare(),
                currentSquare.getLeftSquare()
            };

            foreach (Square next in nextSquares)
            {
                if (next != null && !closedList.Contains(next) && !openList.Contains(next))
                {
                    openList.Add(next);
                }
            }
        }

        return false;
    }

    private bool CanMoveOrJump(Square fromSquare, Square toSquare)
    {
        if (toSquare != null && !toSquare.HasPlayer()) return true;

        Common.Direction direction = GetDirection(fromSquare, toSquare);
        Square jumpOver = toSquare;
        Square jumpTo = GetNextSquareInDirection(jumpOver, direction);

        return jumpOver != null && jumpOver.HasPlayer() && jumpTo != null;
    }

    private Common.Direction GetDirection(Square fromSquare, Square toSquare)
    {
        if (toSquare == fromSquare.getUpperSquare()) return Common.Direction.UP;
        if (toSquare == fromSquare.getLowerSquare()) return Common.Direction.DOWN;
        if (toSquare == fromSquare.getRightSquare()) return Common.Direction.RIGHT;
        if (toSquare == fromSquare.getLeftSquare()) return Common.Direction.LEFT;
        return Common.Direction.UP;
    }

    private Square GetNextSquareInDirection(Square square, Common.Direction direction)
    {
        switch (direction)
        {
            case Common.Direction.UP: return square.getUpperSquare();
            case Common.Direction.DOWN: return square.getLowerSquare();
            case Common.Direction.RIGHT: return square.getRightSquare();
            case Common.Direction.LEFT: return square.getLeftSquare();
            default: return null;
        }
    }

    public uint getPosition(uint xcoord, uint ycoord)
    {
        return ycoord * 11 + xcoord;
    }
    public (uint, uint) getCoordinates(uint idCase)
    {
        uint y = idCase / 11;
        uint x = idCase % 11;
        return (x, y);
    }
    public Square getSquareById(uint id)
    {
        return cases[id];
    }
    public void addWall((uint, uint) firstCoords, (uint, uint) secondCoords, Common.Direction direction)
    {
        (uint firstX, uint firstY) = firstCoords;
        Square firstSquare = getSquareById(getPosition(firstX, firstY));

        (uint secondX, uint secondY) = secondCoords;
        Square secondSquare = getSquareById(getPosition(secondX, secondY));

        switch (direction)
        {
            case Common.Direction.UP:
                Square firstSquareUp = GetNextSquareInDirection(firstSquare, Common.Direction.UP);
                Square secondSquareUp = GetNextSquareInDirection(secondSquare, Common.Direction.UP);
                firstSquare.SetUpperSquare(null);
                secondSquare.SetUpperSquare(null);
                firstSquareUp.SetLowerSquare(null);
                secondSquareUp.SetLowerSquare(null);
                break;
            case Common.Direction.DOWN:
                Square firstSquareDown = GetNextSquareInDirection(firstSquare, Common.Direction.DOWN);
                Square secondSquareDown = GetNextSquareInDirection(secondSquare, Common.Direction.DOWN);
                firstSquare.SetLowerSquare(null);
                secondSquare.SetLowerSquare(null);
                firstSquareDown.SetUpperSquare(null);
                secondSquareDown.SetUpperSquare(null);
                break;
            case Common.Direction.LEFT:
                Square firstSquareLeft = GetNextSquareInDirection(firstSquare, Common.Direction.LEFT);
                Square secondSquareLeft = GetNextSquareInDirection(secondSquare, Common.Direction.LEFT);
                firstSquare.SetLeftSquare(null);
                secondSquare.SetLeftSquare(null);
                firstSquareLeft.SetRightSquare(null);
                secondSquareLeft.SetRightSquare(null);
                break;
            case Common.Direction.RIGHT:
                Square firstSquareRight = GetNextSquareInDirection(firstSquare, Common.Direction.RIGHT);
                Square secondSquareRight = GetNextSquareInDirection(secondSquare, Common.Direction.RIGHT);
                firstSquare.SetRightSquare(null);
                secondSquare.SetRightSquare(null);
                firstSquareRight.SetLeftSquare(null);
                secondSquareRight.SetLeftSquare(null);
                break;
        }
    }
    public List<Common.Direction> GetPath(Square[] squares)
    {
        List<Common.Direction> path = new List<Common.Direction> { };

        for (int i = 0; i < squares.Length - 1; i++)
        {
            Common.Direction dir = GetDirection(squares[i], squares[i + 1]);
            path.Add(dir);
        }
        return path;
    }
    public void placePawn(uint xCoord, uint yCoord, Player player)
    {
        foreach (Square square in cases)
        {
            if (square.getIdCase() == getPosition(xCoord, yCoord))
            {
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

        uint[] winningSquares = playingPlayer.getWinningSquaresPosition();

        Square nextSquareFirstSquare = GetNextSquareInDirection(firstSquare, DTOWall.direction);
        Square nextSquareSecondSquare = GetNextSquareInDirection(secondSquare, DTOWall.direction);

        if (GetNextSquareInDirection(firstSquare, DTOWall.direction) == null || GetNextSquareInDirection(secondSquare, DTOWall.direction) == null)
        {
            return false;
        }

        if (checkPlacementWall(DTOWall.coord1, DTOWall.coord2, DTOWall.direction) == false)
        {
            return false;
        }

        switch (DTOWall.direction)
        {
            case Common.Direction.UP:
                firstSquare.SetUpperSquare(null);
                secondSquare.SetUpperSquare(null);
                nextSquareFirstSquare.SetLowerSquare(null);
                nextSquareSecondSquare.SetLowerSquare(null);
                foreach (uint pionSquare in playingPlayer.getPositionsPions())
                {
                    if (!IsPathPossibleAStar(getCoordinates(pionSquare), getCoordinates(winningSquares[0])) || !IsPathPossibleAStar(getCoordinates(pionSquare), getCoordinates(winningSquares[1])))
                    {
                        firstSquare.SetUpperSquare(nextSquareFirstSquare);
                        secondSquare.SetUpperSquare(nextSquareSecondSquare);
                        nextSquareFirstSquare.SetLowerSquare(firstSquare);
                        nextSquareSecondSquare.SetLowerSquare(secondSquare);
                        return false;
                    }
                }
                firstSquare.SetUpperSquare(nextSquareFirstSquare);
                secondSquare.SetUpperSquare(nextSquareSecondSquare);
                nextSquareFirstSquare.SetLowerSquare(firstSquare);
                nextSquareSecondSquare.SetLowerSquare(secondSquare);
                if (playingPlayer.getHorizontalWallsCount() <= 0)
                {
                    return false;
                }
                break;
            case Common.Direction.DOWN:
                firstSquare.SetLowerSquare(null);
                secondSquare.SetLowerSquare(null);
                nextSquareFirstSquare.SetUpperSquare(null);
                nextSquareSecondSquare.SetUpperSquare(null);
                foreach (uint pionSquare in playingPlayer.getPositionsPions())
                {
                    if (!IsPathPossibleAStar(getCoordinates(pionSquare), getCoordinates(winningSquares[0])) || !IsPathPossibleAStar(getCoordinates(pionSquare), getCoordinates(winningSquares[1])))
                    {
                        firstSquare.SetLowerSquare(nextSquareFirstSquare);
                        secondSquare.SetLowerSquare(nextSquareSecondSquare);
                        nextSquareFirstSquare.SetUpperSquare(firstSquare);
                        nextSquareSecondSquare.SetUpperSquare(secondSquare);
                        return false;
                    }
                }
                firstSquare.SetLowerSquare(nextSquareFirstSquare);
                secondSquare.SetLowerSquare(nextSquareSecondSquare);
                nextSquareFirstSquare.SetUpperSquare(firstSquare);
                nextSquareSecondSquare.SetUpperSquare(secondSquare);
                if (playingPlayer.getHorizontalWallsCount() <= 0)
                {
                    return false;
                }
                break;
            case Common.Direction.LEFT:
                firstSquare.SetLeftSquare(null);
                secondSquare.SetLeftSquare(null);
                nextSquareFirstSquare.SetRightSquare(null);
                nextSquareSecondSquare.SetRightSquare(null);
                foreach (uint pionSquare in playingPlayer.getPositionsPions())
                {
                    if (!IsPathPossibleAStar(getCoordinates(pionSquare), getCoordinates(winningSquares[0])) || !IsPathPossibleAStar(getCoordinates(pionSquare), getCoordinates(winningSquares[1])))
                    {
                        firstSquare.SetLeftSquare(nextSquareFirstSquare);
                        secondSquare.SetLeftSquare(nextSquareSecondSquare);
                        nextSquareFirstSquare.SetRightSquare(firstSquare);
                        nextSquareSecondSquare.SetRightSquare(secondSquare);
                        return false;
                    }
                }
                firstSquare.SetLeftSquare(nextSquareFirstSquare);
                secondSquare.SetLeftSquare(nextSquareSecondSquare);
                nextSquareFirstSquare.SetRightSquare(firstSquare);
                nextSquareSecondSquare.SetRightSquare(secondSquare);
                if (playingPlayer.getVerticalWallsCount() <= 0)
                {
                    return false;
                }
                break;
            case Common.Direction.RIGHT:
                firstSquare.SetRightSquare(null);
                secondSquare.SetRightSquare(null);
                nextSquareFirstSquare.SetLeftSquare(null);
                nextSquareSecondSquare.SetLeftSquare(null);
                foreach (uint pionSquare in playingPlayer.getPositionsPions())
                {
                    if (!IsPathPossibleAStar(getCoordinates(pionSquare), getCoordinates(winningSquares[0])) || !IsPathPossibleAStar(getCoordinates(pionSquare), getCoordinates(winningSquares[1])))
                    {
                        firstSquare.SetRightSquare(nextSquareFirstSquare);
                        secondSquare.SetRightSquare(nextSquareSecondSquare);
                        nextSquareFirstSquare.SetLeftSquare(firstSquare);
                        nextSquareSecondSquare.SetLeftSquare(secondSquare);
                        return false;
                    }
                }
                firstSquare.SetRightSquare(nextSquareFirstSquare);
                secondSquare.SetRightSquare(nextSquareSecondSquare);
                nextSquareFirstSquare.SetLeftSquare(firstSquare);
                nextSquareSecondSquare.SetLeftSquare(secondSquare);
                if (playingPlayer.getVerticalWallsCount() <= 0)
                {
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
                square.SetPlayerID(playerRouge.getPlayerID());
            }

            if (square.getIdCase() == CASE_GAGNANTE_playerRouge_1 || square.getIdCase() == CASE_GAGNANTE_playerRouge_2)
            {
                square.SetWinningSquare(true);
                square.SetPlayerID(playerJaune.getPlayerID());
            }
        }
    }

    public bool checkWin(Player player)
    {
        foreach (uint winningSquare in player.getWinningSquaresPosition())
        {
            foreach (uint pionSquare in player.getPositionsPions())
            {
                if (winningSquare == pionSquare)
                {
                    return true;
                }
                Square pionSquareSquare = getSquareById(pionSquare);
                if ((pionSquareSquare.getUpperSquare() != null && pionSquareSquare.getUpperSquare().getIdCase() == winningSquare) ||
                    (pionSquareSquare.getLowerSquare() != null && pionSquareSquare.getLowerSquare().getIdCase() == winningSquare) ||
                    (pionSquareSquare.getRightSquare() != null && pionSquareSquare.getRightSquare().getIdCase() == winningSquare) ||
                    (pionSquareSquare.getLeftSquare() != null && pionSquareSquare.getLeftSquare().getIdCase() == winningSquare))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void deplacerPion((uint, uint) startPos, (uint, uint) endPos)
    {
        Square startSquare = getSquareById(getPosition(startPos.Item1, startPos.Item2));
        Square endSquare = getSquareById(getPosition(endPos.Item1, endPos.Item2));

        endSquare.SetPlayerID((uint)startSquare.getPlayerID());
        endSquare.SetWinningSquare(true);
        startSquare.SetWinningSquare(false);
        startSquare.SetPlayerID(0);
    }

    public bool checkPlacementWall((uint, uint) firstCoords, (uint, uint) secondCoords, Common.Direction direction)
    {
        afficherBoard(new List<uint> { getPosition(firstCoords.Item1, firstCoords.Item2), getPosition(secondCoords.Item1, secondCoords.Item2) });
        (uint firstX, uint firstY) = firstCoords;
        Square firstSquare = getSquareById(getPosition(firstX, firstY));

        (uint secondX, uint secondY) = secondCoords;
        Square secondSquare = getSquareById(getPosition(secondX, secondY));

        if (firstSquare == null || secondSquare == null)
        {
            return false;
        }

        Common.Direction direction1;
        Common.Direction direction2;
        if (direction == Common.Direction.UP || direction == Common.Direction.DOWN)
        {
            direction1 = Common.Direction.RIGHT;
            direction2 = Common.Direction.LEFT;
        }
        else
        {
            direction1 = Common.Direction.UP;
            direction2 = Common.Direction.DOWN;
        }

        Debug.Log(GetNextSquareInDirection(firstSquare, direction1));
        if (GetNextSquareInDirection(firstSquare, direction1) == null && GetNextSquareInDirection(secondSquare, direction2) == null)
        {
            Square otherSquare1 = GetNextSquareInDirection(firstSquare, direction);
            Square otherSquare2 = GetNextSquareInDirection(secondSquare, direction);
            Debug.Log(GetNextSquareInDirection(otherSquare1, direction1));
            if (GetNextSquareInDirection(otherSquare1, direction1) == null && GetNextSquareInDirection(otherSquare2, direction2) == null)
            {
                return false;
            }
        }
        return true;
    }
}
