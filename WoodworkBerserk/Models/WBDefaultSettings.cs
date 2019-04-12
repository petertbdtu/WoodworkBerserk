using WoodworkBerserk.Controllers;
using Microsoft.Xna.Framework.Input;

namespace WoodworkBerserk.Models
{
    class WBDefaultSettings : IWBSettings
    {
        private Game1 game;
        private IWBKeyboardInputHandler kih;

        public WBDefaultSettings(Game1 game)
        {
            this.game = game;
        }
        public void SetupKeyboardInputHandler(IWBKeyboardInputHandler kih)
        {
            this.kih = kih;
            kih.BindInputToAction(Keys.Up, game.MoveUp);
            kih.BindInputToAction(Keys.Down, game.MoveDown);
            kih.BindInputToAction(Keys.Right, game.MoveRight);
            kih.BindInputToAction(Keys.Left, game.MoveLeft);
        }
    }
}
