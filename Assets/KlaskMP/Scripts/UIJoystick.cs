using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KlaskMP
{
    /// <summary>
    /// Joystick component for controlling player movement and actions using Unity UI events.
    /// There can be multiple joysticks on the screen at the same time, implementing different callbacks.
    /// </summary>
    public class UIJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        /// <summary>
        /// Callback triggered when joystick starts moving by user input.
        /// </summary>
        public event Action onDragBegin;
        
        /// <summary>
        /// Callback triggered when joystick is moving or hold down.
        /// </summary>
        public event Action<Vector2> onDrag;
        
        /// <summary>
        /// Callback triggered when joystick input is being released.
        /// </summary>
        public event Action onDragEnd;
                
        /// <summary>
        /// Current position of the target object on the x and y axis in 2D space.
        /// Values are calculated in the range of [-1, 1] translated to left/down right/up.
        /// </summary>
        public Vector2 movementDelta;
        public Vector2 lastPosition;
        
        //keeping track of current drag state
        private bool isDragging = false;


        /// <summary>
        /// Event fired by UI Eventsystem on drag start.
        /// </summary>
        public void OnPointerDown(PointerEventData data)
        {
            movementDelta = Vector2.zero;
            lastPosition = data.position;
            isDragging = true;
            if(onDragBegin != null)
                onDragBegin();
        }


        /// <summary>
        /// Event fired by UI Eventsystem on drag.
        /// </summary>
        public void OnDrag(PointerEventData data)
        {            
            movementDelta = data.position - lastPosition;
            lastPosition = data.position;
        }
        
        
        //set joystick thumb position to drag position each frame
        void Update()
        {
            //check for actual drag state and fire callback. We are doing this in Update(),
            //not OnDrag, because OnDrag is only called when the joystick is moving. But we
            //actually want to keep moving the player even though the jostick is being hold down
            if(isDragging && onDrag != null)
                onDrag(movementDelta);
            movementDelta = Vector2.zero;
        }


        /// <summary>
        /// Event fired by UI Eventsystem on drag end.
        /// </summary>
        public void OnPointerUp(PointerEventData data)
        {
            //we aren't dragging anymore, reset to default position
            movementDelta = Vector2.zero;
            
            //set dragging to false and fire callback
            isDragging = false;
            if (onDragEnd != null)
                onDragEnd();
        }
    }
}