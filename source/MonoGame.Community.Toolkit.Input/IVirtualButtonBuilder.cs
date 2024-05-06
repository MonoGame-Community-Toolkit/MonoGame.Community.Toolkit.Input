// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Represents a builder interface for creating virtual buttons.
/// </summary>
public interface IVirtualButtonBuilder
{
    /// <summary>
    /// Registers a keyboard key for the virtual button.
    /// </summary>
    /// <param name="key">The keyboard key to register.</param>
    /// <returns>The <see cref="IVirtualButtonBuilder"/> instance.</returns>
    IVirtualButtonBuilder RegisterKeyboardKey(Keys key);

    /// <summary>
    /// Registers a game pad button for the virtual button.
    /// </summary>
    /// <param name="player">The player index.</param>
    /// <param name="button">The game pad button to register.</param>
    /// <returns>The <see cref="IVirtualButtonBuilder"/> instance.</returns>
    IVirtualButtonBuilder RegisterGamePadButton(PlayerIndex player, Buttons button);

    /// <summary>
    /// Registers a mouse button for the virtual button.
    /// </summary>
    /// <param name="button">The mouse button to register.</param>
    /// <returns>The <see cref="IVirtualButtonBuilder"/> instance.</returns>
    IVirtualButtonBuilder RegisterMouseButton(MouseButton button);
}

