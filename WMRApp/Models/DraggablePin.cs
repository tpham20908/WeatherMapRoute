using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WMRApp.Models
{
    public delegate void PushPinDroppedDelegate(DraggablePin pin);


    public class DraggablePin : Pushpin
    {
        public Stop _stop;
        private Map _map;
        private bool isDragging = false;
        Location _center;
        private PushPinDroppedDelegate _pushPinDroppedDelegate;

        public DraggablePin(Map map, PushPinDroppedDelegate pushPinDroppedDelegate, Stop stop)
        {
            _pushPinDroppedDelegate = pushPinDroppedDelegate;
            _map = map;
            _stop = stop;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (_map != null)
            {
                _center = _map.Center;

                _map.ViewChangeOnFrame += _map_ViewChangeOnFrame;
                _map.MouseUp += ParentMap_MouseLeftButtonUp;
                _map.MouseMove += ParentMap_MouseMove;
            }

            // Enable Dragging
            this.isDragging = true;

            base.OnMouseLeftButtonDown(e);
        }

        void _map_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            if (isDragging)
            {
                _map.Center = _center;
            }
        }

        #region "Mouse Event Handler Methods"

        void ParentMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Left Mouse Button released, stop dragging the Pushpin
            if (_map != null)
            {
                _map.ViewChangeOnFrame -= _map_ViewChangeOnFrame;
                _map.MouseUp -= ParentMap_MouseLeftButtonUp;
                _map.MouseMove -= ParentMap_MouseMove;
            }

            this.isDragging = false;
            if (_pushPinDroppedDelegate != null)
            {
                _pushPinDroppedDelegate(this);
            }
        }

        void ParentMap_MouseMove(object sender, MouseEventArgs e)
        {
            var map = sender as Microsoft.Maps.MapControl.WPF.Map;
            // Check if the user is currently dragging the Pushpin
            if (this.isDragging)
            {
                // If so, the Move the Pushpin to where the Mouse is.
                var mouseMapPosition = e.GetPosition(map);
                var pinLocation = map.ViewportPointToLocation(mouseMapPosition);
                this.Location = pinLocation;
            }
        }
        #endregion

    }
}
