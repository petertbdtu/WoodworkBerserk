using WoodworkBerserk.Controllers;
using Microsoft.Xna.Framework.Input;

namespace WoodworkBerserk.Models
{
    class DefaultSettings : ISettings
    {
        private IActionHandler ah;
        private IKeyboardInputHandler kih;

        public DefaultSettings(IActionHandler ah)
        {
            this.ah = ah;
        }
        public void SetupKeyboardInputHandler(IKeyboardInputHandler kih)
        {
            this.kih = kih;
            kih.BindInputToAction(Keys.Up, ah.MoveUp);
            kih.BindInputToAction(Keys.Down, ah.MoveDown);
            kih.BindInputToAction(Keys.Right, ah.MoveRight);
            kih.BindInputToAction(Keys.Left, ah.MoveLeft);
            kih.BindInputToAction(Keys.Space, ah.Interact);
            kih.BindInputToAction(Keys.Escape, ah.Shutdown);
        }
    }
}
