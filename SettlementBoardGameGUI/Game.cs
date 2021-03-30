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
                    // 
                    foreach (var edge in gameBoard.edges[edgeId].point0.connectedEdges)
                    {
                        if (edge.ownedBy == currentPlayer().playerId)
                        {
                            valid = true;
                        }
                    }

                    foreach (var edge in gameBoard.edges[edgeId].point1.connectedEdges)
                    {
                        if (edge.ownedBy == currentPlayer().playerId)
                        {
                            valid = true;
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

        private int[] getEdgesConnectedToVertex(int vertexId)
        {
            var edges = gameBoard.edges.Where(e => e.point0 == gameBoard.vertices[vertexId] || e.point1 == gameBoard.vertices[vertexId]).ToList();
            int[] edgeIndices = new int[edges.Count];
            for(int i = 0; i < edges.Count; i++)
            {
                edgeIndices[i] = gameBoard.edges.IndexOf(edges[i]);
            }
            return edgeIndices;
        }

        public bool buildSettlement(int vertexId)
        {
            // Check if the selected vertex is a valid place for the current player to build a road.
            bool valid = false;
            if (gameBoard.vertices[vertexId].ownedBy == 0)
            {
                foreach(var edge in getEdgesConnectedToVertex(vertexId))
                {
                    if (gameBoard.edges[edge].ownedBy == currentPlayer().playerId)
                    {
                        valid = true;
                    }
                }
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
                return true;
            }
             return false;
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
