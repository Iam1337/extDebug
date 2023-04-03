using System.Collections.Generic;
using UnityEngine;

namespace extDebug.Menu
{
    public interface IDMBranch
    {
        string Name { get; }
        Color NameColor { get; }
        DMItem Current { get; }
        DMContainer Container { set; }
        void SendEvent(EventArgs openBranch);
        void RequestRepaint();
        bool CanRepaint();
        IReadOnlyList<DMItem> GetItems();
        void CompleteRepaint();
    }
}