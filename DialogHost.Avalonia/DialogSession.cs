﻿using System;

namespace DialogHostAvalonia;

/// <summary>
/// Allows an open dialog to be managed. Use is only permitted during a single display operation.
/// </summary>
public class DialogSession {
    private readonly DialogHost _owner;

    internal DialogSession(DialogHost owner, DialogOverlayPopupHost host) {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        Host = host ?? throw new ArgumentNullException(nameof(host));
    }

    /// <summary>
    /// Indicates if the dialog session has ended.  Once ended no further method calls will be permitted.
    /// </summary>
    /// <remarks>
    /// Client code cannot set this directly, this is internally managed.  To end the dialog session use <see cref="Close()"/>.
    /// </remarks>
    public bool IsEnded { get; internal set; }
        
    /// <summary>
    /// The parameter passed to the <see cref="DialogHost.CloseDialogCommand" /> and return by <see cref="DialogHost.Show(object)"/>
    /// </summary>
    internal object? CloseParameter { get; set; }

    /// <summary>
    /// Dialog Overlay Popup Host, To set content
    /// </summary>
    public DialogOverlayPopupHost Host { get; private set; }

    /// <summary>
    /// Gets the <see cref="DialogHost.DialogContent"/> which is currently displayed, so this could be a view model or a UI element.
    /// </summary>
    public object? Content => Host.Content;

    /// <summary>
    /// Update the current content in the dialog.
    /// </summary>
    /// <param name="content"></param>
    public void UpdateContent(object content) {
        Host.Content = content ?? throw new ArgumentNullException(nameof(content));
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the dialog session has ended, or a close operation is currently in progress.</exception>
    public void Close()
    {
        if (IsEnded) throw new InvalidOperationException("Dialog session has ended.");

        _owner.InternalClose(null);
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    /// <param name="parameter">Result parameter which will be returned in <see cref="DialogClosingEventArgs.Parameter"/> or from <see cref="DialogHost.Show(object)"/> method.</param>
    /// <exception cref="InvalidOperationException">Thrown if the dialog session has ended, or a close operation is currently in progress.</exception>
    public void Close(object? parameter)
    {
        if (IsEnded) throw new InvalidOperationException("Dialog session has ended.");

        _owner.InternalClose(parameter);
    }
}