
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WoodworkBerserk.Controllers
{
    class AnimationController
    {
        protected Texture2D texture;
        public Vector2 position = Vector2.Zero;
        public Vector2 origin;
        public float rotation = 0f;
        public float scale = 1f;
        public SpriteEffects spriteEffect;
        protected Rectangle[] rectangles;
        protected int frame = 0;
        protected int rows = 0;
        protected int direction = 0;

        public AnimationController(Texture2D texture, int frames, int rows, int direction)
        {
            this.texture = texture;
            int width = texture.Width / frames;
            int heigth = texture.Height / rows;
            rectangles = new Rectangle[frames];

            for (int i = 0; i < frames; i++)
            {
                    rectangles[i] = new Rectangle(i * width, direction, width, heigth);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangles[frame], Color.White, 0f, origin, scale, spriteEffect, 0f);
        }
    }
}
