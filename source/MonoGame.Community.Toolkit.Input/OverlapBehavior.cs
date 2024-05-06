// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Community.Toolkit.Input;

/// <summary>
/// Specifies the behavior of overlapping input actions.
/// </summary>
public enum OverlapBehavior
{
    /// <summary>
    /// The overlapping input action will cancel the previous action.
    /// </summary>
    Cancel,

    /// <summary>
    /// The overlapping input action will result in a positive action.
    /// </summary>
    Positive,

    /// <summary>
    /// The overlapping input action will result in a negative action.
    /// </summary>
    Negative
}
