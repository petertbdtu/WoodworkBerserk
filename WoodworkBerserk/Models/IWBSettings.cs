using Microsoft.Xna.Framework.Input;
using WoodworkBerserk.Controllers;

namespace WoodworkBerserk.Models
{
    interface IWBSettings
    {
        void SetupKeyboardInputHandler(IWBKeyboardInputHandler kih);
    }
}
