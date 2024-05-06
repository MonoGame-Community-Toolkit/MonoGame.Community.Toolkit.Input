// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Defines a service that provides information about the state of keyboard, mouse, and game pad input.
/// </summary>
public class InputService : IInputServices
{
    private readonly GamePadInfo[] _gamePads;
    private readonly List<VirtualInput> _virtualInputs;

    /// <summary>
    /// Gets the information about the state of keyboard input.
    /// </summary>
    public KeyboardInfo Keyboard { get; private set; }

    /// <summary>
    /// Gets the information about the state of mouse input.
    /// </summary>
    public MouseInfo Mouse { get; private set; }

    /// <summary>
    /// Initialize a new instance of the <see cref="InputService"/> class.
    /// </summary>
    public InputService()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();
        _gamePads = new GamePadInfo[4];
        for (int i = 0; i < 4; i++)
        {
            _gamePads[i] = new GamePadInfo((PlayerIndex)i);
        }
        _virtualInputs = new List<VirtualInput>();
    }

    /// <summary>
    /// updates the state information for each input.
    /// </summary>
    /// <param name="gameTime">
    /// A snapshot of the timing values for the game between the current and previous frames.
    /// </param>
    /// <exception cref="NotImplementedException"></exception>
    public virtual void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();
        for (int i = 0; i < 4; i++)
        {
            _gamePads[i].Update(gameTime);
        }
        for(int i = 0; i < _virtualInputs.Count; i++)
        {
            _virtualInputs[i].Update(gameTime);
        }
    }

    /// <summary>
    /// Returns the <see cref="GamePadInfo"/> specific to the player at the specified player index.
    /// </summary>
    /// <param name="index">The index of the player.</param>
    /// <returns>The <see cref="GamePadInfo"/> for the specified player.</returns>
    public GamePadInfo GetGamePad(PlayerIndex index) => _gamePads[(int)index];

    /// <summary>
    /// Registers a virtual button with the specified name.
    /// </summary>
    /// <param name="name">The name of the virtual button.</param>
    /// <returns>An instance of <see cref="IVirtualButtonBuilder"/> for further configuration.</returns>
    public IVirtualButtonBuilder RegisterVirtualButton(string name)
    {
        VirtualButton button = new VirtualButton(name, this);
        return button;
    }

    /// <summary>
    /// Registers a virtual joystick with the specified name, without snapping or normalization.
    /// </summary>
    /// <param name="name">The name of the virtual joystick.</param>
    /// <returns>An instance of <see cref="IVirtualJoystickBuilder"/> for further configuration.</returns>
    public IVirtualJoystickBuilder RegisterVirtualJoystick(string name) => RegisterVirtualJoystick(name, false, false);

    /// <summary>
    /// Registers a virtual joystick with the specified name and optional snap and normalize settings.
    /// </summary>
    /// <param name="name">The name of the virtual joystick.</param>
    /// <param name="snapped">Specifies whether the axis values should snap to cardinal or intercardinal angles.</param>
    /// <param name="normalized">Specifies whether the axis values should be normalized.</param>
    /// <returns>An instance of <see cref="IVirtualJoystickBuilder"/> for further configuration.</returns>
    public IVirtualJoystickBuilder RegisterVirtualJoystick(string name, bool snapped, bool normalized)
    {
        VirtualJoystick joystick = new VirtualJoystick(name, this, snapped, normalized);
        return joystick;
    }

    /// <summary>
    /// Retrieves the virtual input with the specified name.
    /// </summary>
    /// <param name="name">The name of the virtual input to retrieve.</param>
    /// <returns>
    /// The virtual input with the specified name, or <see langword="null"/>.  This method only returns
    /// <see langword="null"/> if no virtual input is found with a name that matches the given <paramref name="name"/>
    /// parameter value.
    /// </returns>
    public VirtualInput GetVirtualInput(string name)
    {
        for (int i = 0; i < _virtualInputs.Count; i++)
        {
            if (_virtualInputs[i].Name.Equals(name, StringComparison.Ordinal))
            {
                return _virtualInputs[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Removes the virtual input with the specified name.
    /// </summary>
    /// <param name="name">The name of the virtual input to remove.</param>
    /// <returns>
    /// <see langword="true"/> if the virtual input was successfully removed; otherwise, <see langword="false"/>.  This
    /// method only returns <see langword="false"/> if no virtual input is found with a name that matches the given
    /// <paramref name="name"/> parameter value.
    /// </returns>
    public bool RemoveVirtualInput(string name)
    {
        for (int i = 0; i < _virtualInputs.Count; i++)
        {
            if (_virtualInputs[i].Name.Equals(name, StringComparison.Ordinal))
            {
                _virtualInputs.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes all virtual inputs from the input service.
    /// </summary>
    public void ClearVirtualInputs() => _virtualInputs.Clear();
}
