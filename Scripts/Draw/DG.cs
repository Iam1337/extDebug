/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

namespace extDebug.Gizmos
{
    public static class DG
    {
        #region Static Public Vars

        public static Camera Camera
        {
            get => _camera;
            set
            {
                if (_renderPipeline == RenderPipeline.Legacy)
                {
                    if (_cameraHooks != null)
                    {
                        _cameraHooks.BeginRendering = null;
                        Object.Destroy(_cameraHooks);
                        _cameraHooks = null;
                    }
                    
                    _camera = value;

                    if (_camera != null)
                    {
                        _cameraHooks = _camera.gameObject.AddComponent<DGHooks>();
                        _cameraHooks.BeginRendering += BeginRenderingCallback;
                    }
                }
                else
                {
                    _camera = value;
                }
            }
        }

        public static Color Color
        {
            get => _renderColor;
            set
            {
                var command = new DGCommand();
                command.Type = CommandType.Color;
                command.Color = value;

                _commands.Add(command);
                _renderColor = value;
            }
        }

        public static Matrix4x4 Matrix
        {
            get => _renderMatrix;
            set
            {
                var command = new DGCommand();
                command.Type = CommandType.Matrix;
                command.Matrix = value;

                _commands.Add(command);
                _renderMatrix = value;
            }
        }

        #endregion

        #region Static Private Vars

        private static Camera _camera;

        private static DGHooks _cameraHooks;
        
        private static readonly RenderPipeline _renderPipeline;

        private static readonly DGRender _render;

        private static Matrix4x4 _renderMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        private static Color _renderColor = Color.white;
        
        private static List<DGCommand> _commands = new List<DGCommand>();

        #endregion

        #region Public Methods

        static DG()
        {
            // Detect pipeline.
            var pipelineAsset = GraphicsSettings.renderPipelineAsset;
            if (pipelineAsset != null)
            {
                var pipelineTypeName = pipelineAsset.GetType().ToString();
                if (pipelineTypeName == "UniversalRenderPipelineAsset") // Detect URP
                {
                    _renderPipeline = RenderPipeline.URP;
                }
                else if (pipelineTypeName == "HDRenderPipelineAsset") // Detect HDRP
                {
                    _renderPipeline = RenderPipeline.HDRP;
                }
                else
                {
                    _renderPipeline = RenderPipeline.Unknown;
                }
            }
            else // Detect Legacy
            {
                _renderPipeline = RenderPipeline.Legacy;
            }
            
            // Setup hooks.
            Hooks.Update += Update;
            
            // Setup pipeline hooks.
            if (_renderPipeline != RenderPipeline.Legacy)
            {
                RenderPipelineManager.beginCameraRendering += (_, camera) =>
                {
                    if (camera != _camera)
                        return;

                    BeginRenderingCallback();
                };
            }

            // Setup render.
            _render = new DGRender(1000);
        }

        // COMMANDS
        public static void Line(Vector3 from, Vector3 to)
        {
            var command = new DGCommand();
            command.Type = CommandType.Line;
            command.Lines = 1;
            command.Point0 = from;
            command.Point1 = to;

            if (Camera == null)
                Camera = Camera.main;

            _commands.Add(command);
        }

        public static void Cube(Vector3 center, Vector3 size)
        {
            var command = new DGCommand();
            command.Type = CommandType.Cube;
            command.Lines = 8;
            command.Point0 = center;
            command.Size = size;
            
            if (Camera == null)
                Camera = Camera.main;

            _commands.Add(command);
        }

        #endregion

        #region Private Methods

        private static void Update()
        {
            
        }
        
        // Callbacks
        private static void BeginRenderingCallback()
        {
            _render.Repaint(_camera, 0, _commands);
            
            _commands.Clear();
        }

        #endregion
    }
}
