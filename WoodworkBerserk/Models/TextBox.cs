using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WoodworkBerserk.Models
{
    class TextBox
    {
        Texture2D tbTexture;
        SpriteFont font;

        int x { get; set; }
        int y { get; set; }
        int width { get; set; }
        int height { get; set; }
        bool Userclicked { get; set; }
        bool passClicked { get; set; }
        string text = "";
        MouseState mouseState;

        public TextBox(Texture2D tbTexture, SpriteFont font)
        {
            this.tbTexture = tbTexture;
            this.font = font;
            mouseState = Mouse.GetState();
        }
    }
}
