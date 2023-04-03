/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace extDebug.Menu
{
    public class DMLogs : DMItem, IDMBranch
    {
        #region Extensions
        
        private class DMInternalItem : DMItem
        {
            public DMInternalItem(DMBranch parent, string path, string value, string description, int order) : base(parent, path, value, description, order)
            { }

            protected override void OnEvent(EventArgs eventArgs)
            { }
        }
        
        #endregion
        
        #region Private Vars

        public DMItem Current => null;
        
        DMContainer IDMBranch.Container
        {
            set => Container = value;
        }
        
        public Action<DMLogs> OnOpen;

        public Action<DMLogs> OnClose;
        
        #endregion

        #region Private Vars
        
        private bool _canRepaint;

        private float _canRepaintUntil = float.MinValue;

        private float _autoRepaintPeriod;

        private float _autoRepaintAt = float.MaxValue;

        private int _logOffset;

        private readonly DMItem[] _items;

        private readonly DMInternalItem[] _itemsLogs;
        
        private readonly IDMLogsContainer _logsContainer;
        
        #endregion

        #region Public Methods

        public DMLogs(DMBranch parent, string path, string description, IDMLogsContainer logsContainer, int size, int order) : base(parent, path, string.Empty, description, order)
        {
            _logsContainer = logsContainer;
            
            _items = new DMItem[size + 1];
            _itemsLogs = new DMInternalItem[size];

            for (var i = 0; i < size; i++)
            {
                var logItem = new DMInternalItem(null, string.Empty, string.Empty, string.Empty, i);
                logItem.SetEnabled(false);
                
                _items[i] = logItem;
                _itemsLogs[i] = logItem;
            }

            _items[size] = new DMString(null, string.Empty, GetLogsPagination, int.MaxValue);
        }

        public void RequestRepaint() => _canRepaint = true;

        public void RequestRepaint(float duration) => _canRepaintUntil = Time.unscaledTime + duration;

        public bool CanRepaint() => _logsContainer.IsDirty() || _canRepaint || _canRepaintUntil > Time.unscaledTime || Time.unscaledTime > _autoRepaintAt;
        
        public void CompleteRepaint()
        {
            if (_autoRepaintPeriod > 0)
                _autoRepaintAt = Time.unscaledTime + _autoRepaintPeriod;

            _canRepaint = false;
        }
        
        public IReadOnlyList<DMItem> GetItems()
        {
            RepaintItems();
            return _items;
        }

        #endregion

        #region Protected Methods

        protected override void OnEvent(EventArgs eventArgs)
        {
            if (eventArgs.Tag == EventTag.OpenBranch)
            {
                OnOpen?.Invoke(this);
            }
            else if (eventArgs.Tag == EventTag.CloseBranch)
            {
                OnClose?.Invoke(this);
            }
            else if (eventArgs.Tag == EventTag.Input)
            {
                if (eventArgs.Key == EventKey.Up)
                {
                    _logOffset--;

                    if (_logOffset < 0)
                        _logOffset = 0;

                    RequestRepaint();
                }
                else if (eventArgs.Key == EventKey.Down)
                {
                    _logOffset++;

                    if (_logOffset >= _logsContainer.GetLogsCount())
                        _logOffset = _logsContainer.GetLogsCount() - 1;

                    RequestRepaint();
                }
                else if (eventArgs.Key == EventKey.Left)
                {
                    _logOffset -= _itemsLogs.Length;

                    if (_logOffset < 0)
                        _logOffset = 0;

                    RequestRepaint();
                }
                else if (eventArgs.Key == EventKey.Right)
                {
                    _logOffset += _itemsLogs.Length;

                    if (_logOffset >= _logsContainer.GetLogsCount())
                        _logOffset = _logsContainer.GetLogsCount() - 1;

                    RequestRepaint();
                }
                else if (eventArgs.Key == EventKey.Reset)
                {
                    _logOffset = 0;

                    RequestRepaint();
                }
                else if (eventArgs.Key == EventKey.Back)
                {
                    if (Container.IsVisible)
                        Container.Back();
                }
            }
        }

        #endregion

        #region Private Vars

        private string GetLogsPagination()
        {
            return $"{_logOffset} - {Mathf.Clamp(_logOffset + _itemsLogs.Length, 0, _logsContainer.GetLogsCount())} / {_logsContainer.GetLogsCount()}";
        }
        
        private void RepaintItems()
        {
            var logsCount = _logsContainer.GetLogsCount();
            for (var i = 0; i < _itemsLogs.Length; i++)
            {
                var itemLog = _itemsLogs[i];
                
                var index = _logOffset + i;
                if (_logsContainer.GetLog(index, out var tag, out var tagColor, out var message, out var messageColor) && index < logsCount)
                {
                    itemLog.Name = tag;
                    itemLog.NameColor = tagColor;
                    itemLog.Value = message;
                    itemLog.ValueColor = messageColor;
                }
                else
                {
                    itemLog.Name = string.Empty;
                    itemLog.Value = string.Empty;
                }
            }
        }

        #endregion
    }
}
