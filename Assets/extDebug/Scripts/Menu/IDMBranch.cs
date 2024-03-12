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
        int ItemsCount { get; }
        int PageSize { get; }
        int PageStart { get; }
        int PageEnd { get; }
        void SendEvent(EventArgs openBranch);
        void RequestRepaint();
        bool CanRepaint();
        IReadOnlyList<DMItem> GetItems();
        IReadOnlyList<DMItem> GetPageItems();
        void CompleteRepaint();
    }
}