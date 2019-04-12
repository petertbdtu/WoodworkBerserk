using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodworkBerserk.Controllers;

namespace WoodworkBerserk.Models
{
    class Animation : AnimationController
    {
        public float frameSpeed { get; set; }
        public bool loop = false;
        private float timer;
        public float updateTime;

        public Animation(Texture2D texture, int frames, int rows, int direction) : base(texture, frames, rows, direction) { }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(timer > updateTime)
            {
                timer -= updateTime;
                if(frame < rectangles.Length -1)
                {
                        frame++;
                }
                else if(loop == true)
                {
                    frame = 0;
                }
            }
        }


    }
}
