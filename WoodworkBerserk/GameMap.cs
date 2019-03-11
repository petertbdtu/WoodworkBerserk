using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

public class GameMap
{

    private int tileWidth;
    private int tileHeight;
    private int mapWidth;
    private int mapHeight;
    Texture2D grass;

    private Camera2D camera;

    public Camera2D Camera
    {
        get
        {
            return camera;
        }
    }
   

    public GameMap(GraphicsDevice graphics, ContentManager content, int tileWidth, int tileHeight, int mapWidth, int mapHeight)
    {
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        camera = new Camera2D(graphics);
        grass = content.Load<Texture2D>("grass-tile-2");
    }

    public void draw(SpriteBatch spriteBatch)
    {
        Vector2 tilePos = Vector2.Zero;
        spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                spriteBatch.Draw(grass, tilePos, null, Color.White, 0f,
                new Vector2(grass.Width / 2, grass.Height / 2),
                Vector2.One, SpriteEffects.None, 0f);
                tilePos.Y += tileHeight;
            }
            tilePos.Y = 0;
            tilePos.X += tileWidth;
        }
        spriteBatch.End();
    }
}
