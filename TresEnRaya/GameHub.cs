using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TresEnRaya
{
    public class GameHub:Hub
    {
        private const int Total=3;
        private static int[,] _board = new int[Total, Total];
        private static int _numberMovements = 0;

        private readonly static Dictionary<string,string> _users=new Dictionary<string, string>();


        public void NextMovement(int row, int col, ChipType chipType)
        {
            _board[row, col] = (int)chipType;

            Clients.AllExcept(this.Context.ConnectionId).showNewMovement(row,col,chipType);

            var gameOver = CheckGameOver(row, col);

            switch (gameOver)
            {
                case 1:
                    _board = new int[Total, Total];
                    Clients.All.gameOver(_users[this.Context.ConnectionId]);
                    _users.Clear();
                    break;
                case -1:
                    _board = new int[Total, Total];
                    Clients.All.gameOver("");
                    _users.Clear();
                    break;
            }
        }

        private int CheckGameOver(int row, int col)
        {
            var rowTotal = 0;
            var colTotal = 0;
            var diagonal = 0;
            var antidiag = 0;

            _numberMovements++;

            for (int i = 0; i < Total; i++)
            {
                rowTotal = rowTotal + _board[row, i];
                colTotal = colTotal + _board[i, col];
                diagonal = diagonal + _board[i, i];
                antidiag = antidiag + _board[i, (Total -1)- i];
            }

            if ((rowTotal == Total || rowTotal == (int)ChipType.X * Total)
                 || (colTotal == Total || colTotal == (int)ChipType.X * Total)
                 || (diagonal == Total || diagonal == (int)ChipType.X * Total)
                 || (antidiag == Total || antidiag == (int)ChipType.X * Total)
                )
            {
                return 1;
            }

            return _numberMovements==Total*Total ? -1 : 0 ;
        }

        public void ConnectGame(string gamer)
        {
            if (_users.Count < 2)
            {
                _numberMovements = 0;
                _users.Add(this.Context.ConnectionId, gamer);
                this.Clients.Caller.chipType(GetChipType());
            }

            if (_users.Count == 2)
            {
                Clients.All.newGame();
            }
        }

        private ChipType GetChipType()
        {
            return (ChipType)_users.Count - 1;
        }
    }

    public enum ChipType
    {
        O=1,
        X=4
    }
}