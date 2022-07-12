using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace extDebug.Gizmos
{
    public class DGRender : IDisposable
    {
        #region Private Vars

        private Camera _renderCamera;
        
        private int _renderLayer;

        private readonly Mesh _lineMesh;

        private readonly Material _renderMaterialFront;
        
        private readonly Material _renderMaterialBack;
        
        private readonly Vector3[] _lineVertices;

        private readonly Color32[] _lineColors;

        private readonly int[] _lineIndices;

        private readonly int _maxLines;

        private Color32 _currentColor;
        
        private Matrix4x4 _currentMatrix;

        private int _currentLine;

        private int _previousLines;

        #endregion
        
        #region Public Methods

        public DGRender(int maxLines)
        {
            _maxLines = maxLines;
            
            // Setup materials.
            _renderMaterialFront = Resources.Load<Material>("extDebug/Materials/Line Material Front");;
            _renderMaterialBack = Resources.Load<Material>("extDebug/Materials/Line Material Back");;

            // Setup line settings.
            _lineMesh = new Mesh();
            _lineMesh.name = "extDebug Draw Line";
            
            var count = maxLines * 2;
            
            _lineVertices = new Vector3[count];
            _lineColors = new Color32[count];
            _lineIndices = new int[count];

            for (var i = 0; i < count; ++i) 
                _lineIndices[i] = i;

            _currentLine = 0;
            _previousLines = 0;
        }

        public void Dispose()
        {
            // Destroy line mesh.
            if (_lineMesh != null)
                Object.Destroy(_lineMesh); ;
        }

        public void Repaint(Camera camera, int renderLayer, IReadOnlyList<DGCommand> commands)
        {
            var lines = 0;

            _renderCamera = camera;
            _renderLayer = renderLayer;

            // Start draw line.
            _currentMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            _currentColor = Color.white;
            _currentLine = 0;

            for (var i = 0; i < commands.Count; i++)
            {
                var command = commands[i];
                if (command.Type == CommandType.Color)
                {
                    // Set color.
                    _currentColor = command.Color;
                }
                else if (command.Type == CommandType.Matrix)
                {
                    // Break draw.
                    if (lines > 0)
                        Result();

                    // Set new matrix.
                    _currentMatrix = command.Matrix;
                    _currentLine = 0;
                }
                else
                {
                    if (command.Lines + lines >= _maxLines)
                        continue;

                    command.Draw(this);

                    lines += command.Lines;
                }
            }

            // Complete draw.
            if (lines > 0)
                Result();
        }

        public void PushLine(Vector3 from, Vector3 to)
        {
            var index = _currentLine * 2;

            // Set line geometry
            _lineVertices[index] = from;
            _lineVertices[index + 1] = to;

            // Set line color
            _lineColors[index] = _currentColor;
            _lineColors[index + 1] = _currentColor;
            
            _currentLine++;
        }

        #endregion

        #region Private Methods
        
        public void Result()
        {
            const int kSubmeshIndex = 0;

            // Clean up previous unused lines.
            for (var i = _currentLine; i < _previousLines; ++i)
            {
                var index = i * 2;
                
                // Clean line geometry;
                _lineVertices[index] = Vector3.zero;
                _lineVertices[index + 1] = Vector3.zero;
                
                // Clean line color
                _lineColors[index] = Color.clear;
                _lineColors[index + 1] = Color.clear;
            }
            
            // Setup mesh.
            _lineMesh.Clear();
            _lineMesh.vertices = _lineVertices;
            _lineMesh.colors32 = _lineColors;
            _lineMesh.SetIndices(_lineIndices, MeshTopology.Lines, kSubmeshIndex);

            Graphics.DrawMesh(_lineMesh, _currentMatrix, _renderMaterialBack, _renderLayer, _renderCamera, kSubmeshIndex); // Draw back.
            Graphics.DrawMesh(_lineMesh, _currentMatrix, _renderMaterialFront, _renderLayer, _renderCamera, kSubmeshIndex); // Draw front.

            _previousLines = _currentLine;
        }

        #endregion
    }
}