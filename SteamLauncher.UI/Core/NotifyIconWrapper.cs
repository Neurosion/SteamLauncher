using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SteamLauncher.UI.Core
{
    public class NotifyIconWrapper : INotifyIcon
    {
        private NotifyIcon _notifyIcon;

        public bool IsVisible
        {
            get { return _notifyIcon.Visible; }
            set { _notifyIcon.Visible = value; }
        }

        public IList<string> Items { get; private set; }

        public event Action<string> ItemSelected = delegate { };

        public NotifyIconWrapper(Icon icon)
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = icon;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.ItemClicked += (s, e) => ItemSelected(e.ClickedItem.Text);

            var itemsCollection = new ObservableCollection<string>();
            itemsCollection.CollectionChanged += (s, e) =>
                {
                    if (e.NewItems != null)
                        foreach (string currentItem in e.NewItems)
                            _notifyIcon.ContextMenuStrip.Items.Add(currentItem);

                    if (e.OldItems != null)
                        foreach (string currentItem in e.OldItems)
                            _notifyIcon.ContextMenuStrip.Items.RemoveByKey(currentItem);
                };
            
            Items = itemsCollection;
        }

        ~NotifyIconWrapper()
        {
            if (_notifyIcon != null)
                _notifyIcon.Dispose();
        }
    }
}