using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public int textureId;
        public Entity(Vector2 position, int textureId)
        {
            this.position = position;
            this.textureId = textureId;
        }
        public Entity(int x, int y, int textureId)
        {
            this.position = new Vector2(x, y);
            this.textureId = textureId;
        }
        public override string ToString()
        {
            return "{textureId="+textureId+", position="+position.ToString()+"}";
        }
    }
}
