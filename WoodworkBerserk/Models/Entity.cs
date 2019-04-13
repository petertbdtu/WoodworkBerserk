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
        public Texture2D Texture { get; set; }
        public Entity(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.Texture = texture;
        }


        
    }
}
