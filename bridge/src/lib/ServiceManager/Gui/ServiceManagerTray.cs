/***
 * Licensed to the Austrian Association for Software Tool Integration (AASTI)
 * under one or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information regarding copyright
 * ownership. The AASTI licenses this file to you under the Apache License,
 * Version 2.0 (the "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using ServiceManager.Core;

namespace ServiceManager.Gui
{
    public class ServiceManagerTray
    {
        /// <summary>
        /// Callback for closing the application
        /// </summary>
        public delegate void ShutdownEventHandler();
        public event ShutdownEventHandler OnShutdown;

        /// <summary>
        /// The relative folder name, which contains the icon files for
        /// the system tray.
        /// </summary>
        private static string DIR_ICONS = "img";
        private static string FILENAME_ICON_CONNECTED = "connected.ico";
        private string FILENAME_ICON_DISCONNECTED = "disconnected.ico";

        private NotifyIcon _TrayIcon;
        private Icon _IconConnected;
        private Icon _IconDisconnected;
        private ServiceConnectorManager _ServiceConnector;

        public ServiceManagerTray()
        {
            Init();
        }

        private void Init()
        {
            InitIcons();
            _ServiceConnector = new ServiceConnectorManager();
            _TrayIcon = new NotifyIcon();
            _TrayIcon.ContextMenu = CreateContextMenu();
            _TrayIcon.Visible = true;
            RefreshIcon();
        }

        /// <summary>
        /// Creates the icons.
        /// Icons will be set to NULL if the corresponding files can't be found
        /// </summary>
        private void InitIcons()
        {
            string path = Path.Combine(DIR_ICONS, FILENAME_ICON_CONNECTED);
            if (File.Exists(path))
                _IconConnected = new Icon(path);
            else
                _IconConnected = null;

            path = Path.Combine(DIR_ICONS, FILENAME_ICON_DISCONNECTED);
            if (File.Exists(path))
                _IconDisconnected = new Icon(path);
            else
                _IconDisconnected = null;
        }

        /// <summary>
        /// Create menu entries for the tray.
        /// </summary>
        /// <returns></returns>
        private ContextMenu CreateContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            MenuItem item = this.CreateMenuItem("Close", 0);

            // handle close application event
            item.Click += (sender, e) => { Shutdown(); };

            menu.MenuItems.Add(item);

            item = this.CreateMenuItem("Disconnect", 1);
            item.Click += (sender, e) => { Disconnect(); };
            menu.MenuItems.Add(item);

            item = this.CreateMenuItem("Connect", 2);
            item.Click += (sender, e) => { Connect(); };
            menu.MenuItems.Add(item);

            return menu;
        }

        private MenuItem CreateMenuItem(string name, int index)
        {
            MenuItem item = new MenuItem(name);
            item.Index = index;
            return item;
        }

        private void Connect()
        {
            _ServiceConnector.Connect();
            RefreshIcon();
        }

        private void Disconnect()
        {
            _ServiceConnector.Disconnect();
            RefreshIcon();
        }

        /// <summary>
        /// Reset the icon corresponding to the state of ServiceConnector.
        /// </summary>
        private void RefreshIcon()
        {
            if (_ServiceConnector.IsConnected)
            {
                if (_IconConnected != null)
                    _TrayIcon.Icon = _IconConnected;
            }
            else
            {
                if (_IconDisconnected != null)
                    _TrayIcon.Icon = _IconDisconnected;
            }

        }

        /// <summary>
        /// Close Application requested.
        /// </summary>
        private void Shutdown()
        {
            Disconnect();
            OnShutdown();
        }
    }
}
