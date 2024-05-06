// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Represents a builder interface for creating virtual joysticks.
/// </summary>
/// <remarks>
/// This interface provides methods to register input sources such as keyboard keys, game pad buttons, and game pad sticks
/// for constructing virtual joysticks.
/// </remarks>
public interface IVirtualJoystickBuilder
{
    /// <summary>
    /// Registers keyboard keys for the virtual joystick.
    /// </summary>
    /// <param name="up">The key for moving the joystick up.</param>
    /// <param name="down">The key for moving the joystick down.</param>
    /// <param name="left">The key for moving the joystick left.</param>
    /// <param name="right">The key for moving the joystick right.</param>
    /// <param name="behavior">The overlap behavior of the registered keys.</param>
    /// <returns>The <see cref="IVirtualJoystickBuilder"/> instance.</returns>
    IVirtualJoystickBuilder RegisterKeyboardKeys(Keys up, Keys down, Keys left, Keys right, OverlapBehavior behavior);

    /// <summary>
    /// Registers game pad buttons for the virtual joystick.
    /// </summary>
    /// <param name="player">The player index.</param>
    /// <param name="up">The button for moving the joystick up.</param>
    /// <param name="down">The button for moving the joystick down.</param>
    /// <param name="left">The button for moving the joystick left.</param>
    /// <param name="right">The button for moving the joystick right.</param>
    /// <param name="behavior">The overlap behavior of the registered buttons.</param>
    /// <returns>The <see cref="IVirtualJoystickBuilder"/> instance.</returns>
    IVirtualJoystickBuilder RegisterGamePadButtons(PlayerIndex player, Buttons up, Buttons down, Buttons left, Buttons right, OverlapBehavior behavior);

    /// <summary>
    /// Registers the left stick of a game pad for the virtual joystick.
    /// </summary>
    /// <param name="player">The player index.</param>
    /// <returns>The <see cref="IVirtualJoystickBuilder"/> instance.</returns>
    IVirtualJoystickBuilder RegisterGamePadLeftStick(PlayerIndex player);

    /// <summary>
    /// Registers the left stick of a game pad for the virtual joystick with a specified dead zone.
    /// </summary>
    /// <param name="player">The player index.</param>
    /// <param name="deadZone">The dead zone value for stick input.</param>
    /// <returns>The <see cref="IVirtualJoystickBuilder"/> instance.</returns>
    IVirtualJoystickBuilder RegisterGamePadLeftStick(PlayerIndex player, float deadZone);

    /// <summary>
    /// Registers the right stick of a game pad for the virtual joystick.
    /// </summary>
    /// <param name="player">The player index.</param>
    /// <returns>The <see cref="IVirtualJoystickBuilder"/> instance.</returns>
    IVirtualJoystickBuilder RegisterGamePadRightStick(PlayerIndex player);

    /// <summary>
    /// Registers the right stick of a game pad for the virtual joystick with a specified dead zone.
    /// </summary>
    /// <param name="player">The player index.</param>
    /// <param name="deadZone">The dead zone value for stick input.</param>
    /// <returns>The <see cref="IVirtualJoystickBuilder"/> instance.</returns>
    IVirtualJoystickBuilder RegisterGamePadRightStick(PlayerIndex player, float deadZone);
}

