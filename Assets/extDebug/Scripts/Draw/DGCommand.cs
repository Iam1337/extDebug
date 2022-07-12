using System;
using UnityEngine;

namespace extDebug.Gizmos
{
    public class DGCommand
    {
        #region Public Vars

        public CommandType Type;

        public Matrix4x4 Matrix;
        
        public Vector3 Point0;

        public Vector3 Point1;

        public Color Color;

        public Vector3 Size;
        
        public int Lines;

        #endregion

        #region Private Vars

        #endregion

        #region Public Methods

        public void Draw(DGRender render)
        {
            switch (Type)
            {
                case CommandType.Line:
                    DrawLine(render);
                    break;
                case CommandType.Cube:
                    DrawBox(render);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Private Methods

        private void DrawLine(DGRender render)
        {
            render.PushLine(Point0, Point1);
        }

        private void DrawBox(DGRender render)
        {
            var size = Size * 0.5f;
            var from = Vector3.zero;
            var to = Vector3.zero;

            from.x = Point0.x - size.x;
            from.y = Point0.y - size.y;
            from.z = Point0.z - size.z;
            to.x = Point0.x + size.x;
            to.y = Point0.y - size.y;
            to.z = Point0.z - size.z;
            render.PushLine(from, to);

            from.x = Point0.x - size.x;
            from.y = Point0.y - size.y;
            from.z = Point0.z - size.z;
            to.x = Point0.x - size.x;
            to.y = Point0.y + size.y;
            to.z = Point0.z - size.z;
            render.PushLine(from, to);

            from.x = Point0.x - size.x;
            from.y = Point0.y - size.y;
            from.z = Point0.z - size.z;
            to.x = Point0.x - size.x;
            to.y = Point0.y - size.y;
            to.z = Point0.z + size.z;
            render.PushLine(from, to);

            from.x = Point0.x + size.x;
            from.y = Point0.y - size.y;
            from.z = Point0.z - size.z;
            to.x = Point0.x + size.x;
            to.y = Point0.y + size.y;
            to.z = Point0.z - size.z;
            render.PushLine(from, to);

            from.x = Point0.x + size.x;
            from.y = Point0.y - size.y;
            from.z = Point0.z - size.z;
            to.x = Point0.x + size.x;
            to.y = Point0.y - size.y;
            to.z = Point0.z + size.z;
            render.PushLine(from, to);

            from.x = Point0.x - size.x;
            from.y = Point0.y + size.y;
            from.z = Point0.z - size.z;
            to.x = Point0.x + size.x;
            to.y = Point0.y + size.y;
            to.z = Point0.z - size.z;
            render.PushLine(from, to);

            from.x = Point0.x - size.x;
            from.y = Point0.y + size.y;
            from.z = Point0.z - size.z;
            to.x = Point0.x - size.x;
            to.y = Point0.y + size.y;
            to.z = Point0.z + size.z;
            render.PushLine(from, to);

            from.x = Point0.x - size.x;
            from.y = Point0.y - size.y;
            from.z = Point0.z + size.z;
            to.x = Point0.x + size.x;
            to.y = Point0.y - size.y;
            to.z = Point0.z + size.z;
            render.PushLine(from, to);

            from.x = Point0.x - size.x;
            from.y = Point0.y - size.y;
            from.z = Point0.z + size.z;
            to.x = Point0.x - size.x;
            to.y = Point0.y + size.y;
            to.z = Point0.z + size.z;
            render.PushLine(from, to);

            from.x = Point0.x - size.x;
            from.y = Point0.y + size.y;
            from.z = Point0.z + size.z;
            to.x = Point0.x + size.x;
            to.y = Point0.y + size.y;
            to.z = Point0.z + size.z;
            render.PushLine(from, to);

            from.x = Point0.x + size.x;
            from.y = Point0.y - size.y;
            from.z = Point0.z + size.z;
            to.x = Point0.x + size.x;
            to.y = Point0.y + size.y;
            to.z = Point0.z + size.z;
            render.PushLine(from, to);

            from.x = Point0.x + size.x;
            from.y = Point0.y + size.y;
            from.z = Point0.z - size.z;
            to.x = Point0.x + size.x;
            to.y = Point0.y + size.y;
            to.z = Point0.z + size.z;
            render.PushLine(from, to);
        }


        #endregion
    }
}