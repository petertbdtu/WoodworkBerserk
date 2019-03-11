using System;

public class Map
{

    private int tileWidth;
    private int tileHeight;
    private int mapWidth;
    private int mapHeight;

    public Map(int tileWidth, int tileHeight, int mapWidth, int mapHeight)
	{
        this.tileWidtht = tileWidth;
        this.tileHeight = tileHeight;
        this.mapWidth = mapWidth;
        this.tileHeight = tileHeight;
	}

    public void draw(SpriteBatch spriteBatch)
    {
        Vector2 tilePos = Vector2.Zero;

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                spriteBatch.FillRectangle(tilePos, new Size2(tileWidth, tileHeight), ConsoleColor.Green);
                tilePos.Y += tileHeight;
            }
            tilePos.Y = 0;
            tilePos.X += tileWidth;
        }
    }
}
