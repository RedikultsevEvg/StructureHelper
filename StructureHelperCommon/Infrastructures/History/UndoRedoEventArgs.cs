using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.History
{
    public sealed class UndoRedoEventArgs : EventArgs
    {
        public UndoRedoEventArgs(UndoRedoState state, UndoRedoHistoryEntry? entry = null)
        {
            State = state;
            Entry = entry;
        }

        public UndoRedoState State { get; }
        public UndoRedoHistoryEntry? Entry { get; }
    }
}
