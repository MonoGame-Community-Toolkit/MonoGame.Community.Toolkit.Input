// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using Microsoft.Xna.Framework;

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Defines an implementation of the <see cref="InputService"/> class that also implements <see cref="IUpdateable"/> and
/// <see cref="IGameComponent"/> so that it can be used automatically with a <see cref="GameComponentCollection"/>.
/// This class cannot be inherited.
/// </summary>
/// <remarks>
/// If you are not using the <see cref="GameComponentCollection"/> functionality, then you should use the base
/// <see cref="InputService"/> class instead.
/// </remarks>
public class InputServiceComponent : InputService, IUpdateable, IGameComponent
{
    private bool _enabled;
    private int _updateOrder;

    /// <summary>
    /// Gets or Sets a value that indicates whether this <see cref="InputServiceComponent"/> is enabled.
    /// </summary>
    /// <remarks>
    /// When <see langword="false"/>, this component will not be updated.
    /// </remarks>
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled == value) { return; }
            _enabled = value;
            EnabledChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets or Sets a value indicating the order in which this instance of the <see cref="InputServiceComponent"/>
    /// class should be updated relative to other <see cref="IUpdateable"/> components.
    /// </summary>
    public int UpdateOrder
    {
        get => _updateOrder;
        set
        {
            if (_updateOrder == value) { return; }
            _updateOrder = value;
            UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// An event that is triggered when the value of the <see cref="Enabled"/> property is changed.
    /// </summary>
    public event EventHandler<EventArgs> EnabledChanged;

    /// <summary>
    /// An event that is triggered when the value of the <see cref="UpdateOrder"/> property is changed.
    /// </summary>
    public event EventHandler<EventArgs> UpdateOrderChanged;

    /// <summary>
    /// Initialize a new instance of the <see cref="InputServiceComponent"/> class.
    /// </summary>
    public InputServiceComponent() { }

    /// <inheritdoc/>
    public override void Update(GameTime gameTime)
    {
        if (Enabled) { base.Update(gameTime); }
    }

    /// <summary>
    /// Initializes this component.
    /// </summary>
    public void Initialize() { }
}
