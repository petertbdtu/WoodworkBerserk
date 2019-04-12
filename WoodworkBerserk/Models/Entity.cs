using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodworkBerserk.Models
{
    public class Entity
    {
        public Vector2 position;
        public int tileTexture;
        public Boolean collision;

        public Entity(Vector2 position, int tileTexture, bool collision)
        {
            this.position = position;
            this.tileTexture = tileTexture;
            this.collision = collision;
        }
    }
}
