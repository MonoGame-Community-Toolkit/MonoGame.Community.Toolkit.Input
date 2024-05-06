// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Represents a base class for a virtual input.
/// </summary>
public class VirtualInput
{
    /// <summary>
    /// Gets the input service associated with this virtual input.
    /// </summary>
    protected InputService Input { get; }

    /// <summary>
    /// Gets the name of the virtual input.
    /// </summary>
    public string Name { get; }

    internal VirtualInput(string name, InputService input) => (Name, Input) = (name, input);

    internal virtual void Update(GameTime gameTime) { }
}

