using WoodworkBerserk.Controllers;
using Microsoft.Xna.Framework.Input;

namespace WoodworkBerserk.Models
{
    class WBDefaultSettings : IWBSettings
    {
        private IWBKeyboardInputHandler kih;
        public void SetupKeyboardInputHandler(IWBKeyboardInputHandler kih)
        {
            this.kih = kih;
        }

        public void TestingAddAction(Keys k, WBInputDelegate id)
        {
            kih.BindInputToAction(k, id);
        }
    }
}
