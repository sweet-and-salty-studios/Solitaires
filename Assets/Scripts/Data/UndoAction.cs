using System;

namespace SweetAndSaltyStudios
{
    public struct UndoAction
    {
        private readonly Action undoAction;

        public UndoAction(Action undoAction)
        {
            this.undoAction = undoAction;
        }

        public void RedoAction()
        {
            undoAction?.Invoke();
        }
    }
}