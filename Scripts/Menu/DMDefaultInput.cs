/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Menu
{
    internal class DMDefaultInput : IDMInput
    {
        public EventKey GetKey(bool isVisible, out bool shift)
        {
            shift = false;

            if (Input.GetKey(KeyCode.Q))
                return EventKey.ToggleMenu;

            if (isVisible)
            {
                shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

                if (Input.GetKey(KeyCode.W))
                    return EventKey.Up;
                if (Input.GetKey(KeyCode.S))
                    return EventKey.Down;
                if (Input.GetKey(KeyCode.A))
                    return EventKey.Left;
                if (Input.GetKey(KeyCode.D))
                    return EventKey.Right;
                if (Input.GetKey(KeyCode.E))
                    return EventKey.Back;
                if (Input.GetKey(KeyCode.F))
                    return EventKey.Submit;
                if (Input.GetKey(KeyCode.R))
                    return EventKey.Reset;
                if (Input.GetKey(KeyCode.T))
                    return EventKey.PageUp;
                if (Input.GetKey(KeyCode.G))
                    return EventKey.PageDown;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.A))
                    return EventKey.Left;
                if (Input.GetKey(KeyCode.D))
                    return EventKey.Right;
                if (Input.GetKey(KeyCode.R))
                    return EventKey.Reset;
            }

            return EventKey.None;
        }

        public int GetPageIncrementation()
        {
            return 10;
        }
    }
}