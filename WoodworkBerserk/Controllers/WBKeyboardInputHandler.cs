using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using WoodworkBerserk.Models;

namespace WoodworkBerserk.Controllers
{

    public class WBKeyboardInputHandler : IWBKeyboardInputHandler
    {
        private Dictionary<Keys, WBInputDelegate> controls;
        public WBKeyboardInputHandler()
        {
            controls = new Dictionary<Keys, WBInputDelegate>();
        }
        public void BindInputToAction(Keys input, WBInputDelegate action)
        {
            controls.Add(input, action);
        }

        public void UnbindInput(Keys k)
        {
            controls.Remove(k);
        }

        public void HandleInput(KeyboardState kstate, float elapsedTimeInSeconds)
        {
            // Doesn't do key combinations.
            foreach(Keys k in kstate.GetPressedKeys())
            {
                WBInputDelegate action;
                if (controls.TryGetValue(k, out action))
                {
                    action.Invoke(elapsedTimeInSeconds);
                }
            }
        }
    }
}
