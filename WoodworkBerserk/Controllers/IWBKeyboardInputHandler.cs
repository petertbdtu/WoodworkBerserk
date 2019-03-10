using WoodworkBerserk.Models;
using Microsoft.Xna.Framework.Input;

namespace WoodworkBerserk.Controllers
{
    public delegate void WBInputDelegate(float elapsedTimeInSeconds);
    interface IWBKeyboardInputHandler
    {
        void BindInputToAction(Keys input, WBInputDelegate action);
        void UnbindInput(Keys input);
        void HandleInput(KeyboardState kstate, float elapsedTimeInSeconds);
    }
}
