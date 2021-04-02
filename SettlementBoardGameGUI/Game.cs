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
            gameInitialRound = true;

        }

        public List<Player> players { get; private set; }
        public Board gameBoard { get; private set; }

        private int playersTurn;
        private bool gameInitialRound = true;

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


        public bool buildRoad(int edgeId)
        {
            // Check if the selected edge is valid place for the current player to build a road.
            bool valid = false;
            if (gameBoard.edges[edgeId].ownedBy == 0)
            {
                if(gameBoard.edges[edgeId].point0.ownedBy == playersTurn || 
                    gameBoard.edges[edgeId].point1.ownedBy == playersTurn)
                {
                    valid = true;
                }
                else
                {
                    // Check all edges connected to this edgeId to see if any are owned by the current player.
                    foreach (var edge in gameBoard.edges[edgeId].point0.connectedEdges.Concat(gameBoard.edges[edgeId].point1.connectedEdges))
                    {
                        if (edge.ownedBy == currentPlayer().playerId)
                        {
                            valid = true;
                            break;
                        }
                    }
                }
            }

            // Make sure the player has enough resources to build this road.
            if (valid && currentPlayer().resourceCards.Contains(ResourceType.Lumber) &&
            currentPlayer().resourceCards.Contains(ResourceType.Brick))
            {
                gameBoard.edges[edgeId].ownedBy = playersTurn;
                currentPlayer().resourceCards.Remove(ResourceType.Lumber);
                currentPlayer().resourceCards.Remove(ResourceType.Brick);
                return true;
            }
            return false;
        }

        public Tuple<bool, string> buildSettlement(int vertexId)
        {
            // Check if the selected vertex is a valid place for the current player to build a road.
            bool valid = false;
            if (gameBoard.vertices[vertexId].ownedBy == 0)
            {
                foreach(var edge in gameBoard.vertices[vertexId].connectedEdges)
                {
                    // Make sure there are no settlements on adjacent vertices. (There must be at least 2 roads between each settlement).
                    if (edge.point0.ownedBy != 0 || edge.point1.ownedBy != 0)
                    {
                        return new Tuple<bool, string>(false, "Settlements must be placed at least 2 roads away from another settlement.");
                    }
                    // Make sure there is at least 1 adjacent edge which is owned by the current player. (Settlements must connect to roads).
                    if (edge.ownedBy == currentPlayer().playerId)
                    {
                        valid = true;
                    }
                    // For the initial setup of the game, settlements can be placed anywhere. They don't need to be connected to an existing road.
                    if (gameInitialRound)
                    {
                        valid = true;
                    }
                }
                if (!valid)
                {
                    return new Tuple<bool, string>(false, "A new settlement must be connected to a road you own.");
                }
            }
            else
            {
                return new Tuple<bool, string>(false, "A settlement is already built on this space.");
            }

            // Make sure the player has enough resources to build this settlement.
            if (valid && currentPlayer().resourceCards.Contains(ResourceType.Lumber) && 
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
                return new Tuple<bool, string>(true, "You successfully built a settlement!");
            }
            return new Tuple<bool, string>(false, "You do not have enough resources to built a settlement.");
        }

        public bool buildCity(int vertexId)
        {
            // Check if the selected vertex is owned by the current player and that the current player has enough resource cards.
            if (gameBoard.vertices[vertexId].ownedBy == currentPlayer().playerId &&
                currentPlayer().resourceCards.Where(i => i == ResourceType.Grain).Count() >= 2 &&
                currentPlayer().resourceCards.Where(i => i == ResourceType.Ore).Count() >= 3)
            {
                gameBoard.vertices[vertexId].settlementSize = 2;
                currentPlayer().resourceCards.Remove(ResourceType.Grain);
                currentPlayer().resourceCards.Remove(ResourceType.Grain);
                currentPlayer().resourceCards.Remove(ResourceType.Ore);
                currentPlayer().resourceCards.Remove(ResourceType.Ore);
                currentPlayer().resourceCards.Remove(ResourceType.Ore);
                return true;
            }
            return false;
        }
    }
}
