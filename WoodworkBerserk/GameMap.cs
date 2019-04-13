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
    private int mapWidth;
    private int mapHeight;
    Texture2D[] textures;

    Vector2 tileCenterPosition;

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
        this.tileCenterPosition = new Vector2(tileWidth/2, tileHeight/2);
        textures = new Texture2D[2];
        textures[0] = content.Load<Texture2D>("grass-tile-2");
        textures[1] = content.Load<Texture2D>("grass-tile-3");
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
            for (int j = 0; j < state.mapHeight; j++)
            {
                Texture2D tile = textures[state.terrain[i, j]];
                spriteBatch.Draw(tile, new Vector2(tilePos.X,tilePos.Y), null, Color.White, 0f,
                    tileCenterPosition, Vector2.One, SpriteEffects.None, BACKGROUND);
                tilePos.Y += tileHeight;
            }
            tilePos.Y = 0;
            tilePos.X += tileWidth;
            foreach (Entity e in state.entities.Values)
            {
                Texture2D graphic = e.Texture;
                Vector2 entityCenter = new Vector2(graphic.Width/2, graphic.Height/2);
                Vector2 position = new Vector2(e.position.X * tileWidth, e.position.Y * tileHeight);
                spriteBatch.Draw(graphic, position, null, Color.White, 0f, entityCenter,
                    Vector2.One, SpriteEffects.None, FOREGROUND);
            }
        }
        spriteBatch.End();
    }
}
