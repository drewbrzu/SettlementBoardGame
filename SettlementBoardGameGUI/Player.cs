using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBoardGameGUI
{
    enum PlayerType
    {
        AI,
        Human
    }

    enum DevelopmentCard
    {
        Knight,
        VictoryPoint,
        Monopoly,
        RoadBuilding,
        YearOfPlenty
    }
    class Player
    {
        public Player(PlayerType playerType, string name, int id)
        {
            this.playerType = playerType;
            this.playerName = name;
            this.playerId = id;
            this.resourceCards = new List<ResourceType>();
            this.developmentCards = new List<DevelopmentCard>();
            this.victoryPoints = 0;
            var rand = new Random();
            rand.Next(0, 4);
            for(int i = 0; i < 6; i++)
            {
                int a = rand.Next(0, 6);
                if(a > 0)
                {
                    resourceCards.Add((ResourceType)a);
                }
            }
            resourceCards.Add(ResourceType.Lumber);
            resourceCards.Add(ResourceType.Brick);
            resourceCards.Add(ResourceType.Wool);
            resourceCards.Add(ResourceType.Grain);
            resourceCards.Add(ResourceType.Lumber);
            resourceCards.Add(ResourceType.Brick);
            resourceCards.Add(ResourceType.Wool);
            resourceCards.Add(ResourceType.Grain);
            resourceCards.Add(ResourceType.Lumber);
            resourceCards.Add(ResourceType.Brick);
            resourceCards.Add(ResourceType.Lumber);
            resourceCards.Add(ResourceType.Brick);
        }

        public PlayerType playerType { get; private set; }
        public string playerName { get; private set; }
        public int playerId { get; private set; }
        public List<ResourceType> resourceCards { get; set; }
        public List<DevelopmentCard> developmentCards { get; set; }
        public int victoryPoints { get; set; }
    }
}
