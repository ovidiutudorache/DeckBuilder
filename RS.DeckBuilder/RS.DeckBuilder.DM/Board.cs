using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM
{
    public class Board
    {
        public List<Player> Players { get; set; }
        public List<PlayerBoard> PlayerBoards { get; set; }

        public Board(Player player1, Player player2, PlayerBoard board1 = null, PlayerBoard board2 = null)
        {
            Players = new List<Player>(2);
            Players.Add(player1);
            Players.Add(player2);

            PlayerBoards = new List<PlayerBoard>(2);

            player1.Board = board1 ?? new PlayerBoard();
            PlayerBoards.Add(player1.Board);

            player2.Board = board2 ?? new PlayerBoard();
            PlayerBoards.Add(player2.Board);
        }
    }
}