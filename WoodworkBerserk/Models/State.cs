using System.Collections.Generic;

namespace WoodworkBerserk.Models
{
    public class State
    {
        public int mapWidth;
        public int mapHeight;
        public int mapID;
        // TODO store maps locally, not in state.
        public int[,] terrain;
        public Dictionary<int, Entity> entities;
        public int playerId;

        public State(int mapID, int[,] terrain, Dictionary<int, Entity> entities, int playerId)
        {
            this.mapID = mapID;
            this.terrain = terrain;
            this.entities = entities;
            this.mapHeight = terrain.GetLength(0);
            this.mapWidth = terrain.GetLength(1);
            this.playerId = playerId;
        }
    }
}
