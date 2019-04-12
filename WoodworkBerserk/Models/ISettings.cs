using Microsoft.Xna.Framework.Input;
using WoodworkBerserk.Controllers;

namespace WoodworkBerserk.Models
{
    interface ISettings
    {
        void SetupKeyboardInputHandler(IKeyboardInputHandler kih);
    }
}
