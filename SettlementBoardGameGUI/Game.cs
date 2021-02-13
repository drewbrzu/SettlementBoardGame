using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SettlementBoardGameGUI
{
    class Game
    {
        public Game(Point centerpoint, int numberHumanPlayers, int numberAIPlayers)
        {
            players = new List<Player>();
            for (int i = 0; i < numberHumanPlayers; i++)
            {
                players.Add(new Player(PlayerType.Human, "PLAYER_" + (i + 1).ToString(), (players.Count + 1)));
            }
            for (int i = 0; i < numberAIPlayers; i++)
            {
                players.Add(new Player(PlayerType.AI, "CPU_" + (i + 1).ToString(), (players.Count + 1)));
            }

            gameBoard = new Board(centerpoint);

            // TODO: Roll to see order of players

            // TODO: Set turn to player 1
            playersTurn = 1;

            // TODO: Have players place initial settlements

        }

        public List<Player> players { get; private set; }
        public Board gameBoard { get; private set; }

        private int playersTurn;

        public Player currentPlayer()
        {
            return players.Where(p => p.playerId == playersTurn).First();
        }

        public int rollDice()
        {
            var rand = new Random();
            var dice1 = rand.Next(1, 7);
            var dice2 = rand.Next(1, 7);
            var roll = dice1 + dice2;
            return roll;
        }

        public void nextTurn()
        {
            if(playersTurn >= players.Count)
            {
                playersTurn = 1;
            }
            else
            {
                playersTurn++;
            }
        }

        public bool buildSettlement(int vertexId)
        {
            if(gameBoard.vertices[vertexId].ownedBy == 0 && 
                currentPlayer().resourceCards.Contains(ResourceType.Lumber) && 
                currentPlayer().resourceCards.Contains(ResourceType.Brick) && 
                currentPlayer().resourceCards.Contains(ResourceType.Wool) &&
                currentPlayer().resourceCards.Contains(ResourceType.Grain))
            {
                gameBoard.vertices[vertexId].ownedBy = playersTurn;
                gameBoard.vertices[vertexId].settlementSize = 1;
                currentPlayer().resourceCards.Remove(ResourceType.Lumber);
                currentPlayer().resourceCards.Remove(ResourceType.Brick);
                currentPlayer().resourceCards.Remove(ResourceType.Wool);
                currentPlayer().resourceCards.Remove(ResourceType.Grain);
                return true;
            }
             return false;
        }
    }
}
