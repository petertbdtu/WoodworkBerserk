using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using WoodworkBerserk.Models;

namespace WoodworkBerserk.Controllers
{

    public class KeyboardInputHandler : IKeyboardInputHandler
    {
        private Dictionary<Keys, InputDelegate> controls;
        public KeyboardInputHandler()
        {
            controls = new Dictionary<Keys, InputDelegate>();
        }
        public void BindInputToAction(Keys input, InputDelegate action)
        {
            controls.Add(input, action);
        }

        public void UnbindInput(Keys k)
        {
            controls.Remove(k);
        }

        public void HandleInput(KeyboardState kstate)
        {
            // Doesn't do key combinations.
            foreach(Keys k in kstate.GetPressedKeys())
            {
                InputDelegate action;
                if (controls.TryGetValue(k, out action))
                {
                    action.Invoke();
                }
            }
        }
    }
}
