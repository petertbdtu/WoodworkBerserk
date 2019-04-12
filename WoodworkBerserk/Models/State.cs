using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodworkBerserk.Models
{
    public class State
    {
        public int mapWidth;
        public int mapHeight;
        public int mapID;
        public int[,] terrain;
        public Dictionary<int, Entity> entities;

        public State(int mapID, int[,] terrain, Dictionary<int, Entity> entities)
        {
            this.mapID = mapID;
            this.terrain = terrain;
            this.entities = entities;
            this.mapHeight = terrain.GetLength(0);
            this.mapWidth = terrain.GetLength(1);
        }
    }
}
