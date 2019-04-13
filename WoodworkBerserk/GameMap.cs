using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using WoodworkBerserk.Models;

public class GameMap
{
    private readonly float BACKGROUND = 0f;
    private readonly float FOREGROUND = 0.1f;

    private int tileWidth;
    private int tileHeight;
    Texture2D spriteSheet;
    Rectangle[] spriteIndex;
    Point spriteSize = new Point(16, 16);

    Vector2 tileCenterPosition;

    private Camera2D camera;

    public Camera2D Camera
    {
        get
        {
            return camera;
        }
    }
   

    public GameMap(GraphicsDevice graphics, ContentManager content, int tileWidth, int tileHeight)
    {
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
        camera = new Camera2D(graphics);
        this.tileCenterPosition = new Vector2(tileWidth / 2, tileHeight / 2);
        spriteSheet = content.Load<Texture2D>("basictiles");
        spriteIndex = new Rectangle[2];
        spriteIndex[1] = new Rectangle(new Point(64 ,144), spriteSize);
        spriteIndex[0] = new Rectangle(new Point(0, 128), spriteSize);
        
    }

    public void Draw(SpriteBatch spriteBatch, State state)
    {
        Vector2 tilePos = Vector2.Zero;
        Entity player;
        if (state.entities.TryGetValue(state.playerId, out player))
        {
            camera.LookAt(player.position * tileHeight);
        }
        else
            camera.LookAt(Vector2.Zero);
        spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
        for (int i = 0; i < state.mapWidth; i++)
        {
            for (int j = 0; j < state.mapHeight; j++) {
                spriteBatch.Draw(spriteSheet, new Vector2(tilePos.X,tilePos.Y), spriteIndex[state.terrain[i, j]], Color.White, 0f,
                    tileCenterPosition, Vector2.One, SpriteEffects.None, BACKGROUND);
                tilePos.Y += tileHeight;
            }
            tilePos.Y = 0;
            tilePos.X += tileWidth;
            foreach (Entity e in state.entities.Values)
            {
                Vector2 position = new Vector2(e.position.X * tileWidth, e.position.Y * tileHeight);
                spriteBatch.Draw(spriteSheet, position, spriteIndex[e.textureId], Color.White, 0f, tileCenterPosition,
                    Vector2.One, SpriteEffects.None, FOREGROUND);
            }
        }
        spriteBatch.End();
    }
}
