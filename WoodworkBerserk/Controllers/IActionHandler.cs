using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodworkBerserk.Controllers
{
    enum PlayerAction
    {
        Nothing,
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        Interact
    };

    interface IActionHandler
    {
        void MoveUp();
        void MoveDown();
        void MoveRight();
        void MoveLeft();
        void Interact();
    }
}
