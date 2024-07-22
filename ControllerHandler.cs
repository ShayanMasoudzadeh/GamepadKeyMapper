using System;
using SharpDX.DirectInput;
using System.Diagnostics;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace ControllerToKeyboardAPP
{
    public class ControllerHandler
    {
        private DirectInput _directInput;
        private Joystick _joystick;
        private InputSimulator _inputSimulator;

        public ControllerHandler()
        {
            Debug.WriteLine("Initializing DirectInput...");
            _directInput = new DirectInput();
            _inputSimulator = new InputSimulator();
            var joystickGuid = Guid.Empty;

            // Look for a Gamepad device
            foreach (var deviceInstance in _directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                joystickGuid = deviceInstance.InstanceGuid;
                Debug.WriteLine($"Found gamepad: {deviceInstance.InstanceName}");
            }

            // If no Gamepad is found, look for a Joystick device
            if (joystickGuid == Guid.Empty)
            {
                foreach (var deviceInstance in _directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid = deviceInstance.InstanceGuid;
                    Debug.WriteLine($"Found joystick: {deviceInstance.InstanceName}");
                }
            }

            // If no suitable device is found, output a message
            if (joystickGuid == Guid.Empty)
            {
                Debug.WriteLine("No joystick/gamepad found.");
                return;
            }

            // Initialize the joystick
            _joystick = new Joystick(_directInput, joystickGuid);
            _joystick.Properties.BufferSize = 128;
            _joystick.Acquire();

            Debug.WriteLine("Joystick/gamepad found and acquired.");
        }

        public void Update()
        {
            if (_joystick == null)
            {
                Debug.WriteLine("No joystick/gamepad to update.");
                return;
            }

            _joystick.Poll();
            var data = _joystick.GetBufferedData();
            foreach (var state in data)
            {
                // Check if a specific button is pressed (example: button 0)
                if (state.Offset == JoystickOffset.Buttons0 && state.Value == 128)
                {
                    // Simulate pressing the 'X' key
                    _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                    Debug.WriteLine("Button A mapped to Key X");
                }

                //checking what button is pressed
                if (state.Offset >= JoystickOffset.Buttons0 && state.Offset <= JoystickOffset.Buttons31)
                {
                    Debug.WriteLine($"Button {(state.Offset - JoystickOffset.Buttons0)} pressed, value: {state.Value}");
                }
            }
        }
    }
}