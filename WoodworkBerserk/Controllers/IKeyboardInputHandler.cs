using WoodworkBerserk.Models;
using Microsoft.Xna.Framework.Input;

namespace WoodworkBerserk.Controllers
{
    public delegate void InputDelegate();
    interface IKeyboardInputHandler
    {
        void BindInputToAction(Keys input, InputDelegate action);
        void UnbindInput(Keys input);
        void HandleInput(KeyboardState kstate);
    }
}
