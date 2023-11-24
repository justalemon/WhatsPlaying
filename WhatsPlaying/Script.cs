using System;
using GTA;

namespace WhatsPlaying;

/// <summary>
/// Script that shows what is currently playing in your vehicle.
/// </summary>
public class WhatsPlaying : Script
{
    #region Constructors

    /// <summary>
    /// Creates a new script.
    /// </summary>
    public WhatsPlaying()
    {
        Tick += OnTick;
    }
    
    #endregion
    
    #region Event Functions

    private async void OnTick(object sender, EventArgs e)
    {
    }

    #endregion
}
