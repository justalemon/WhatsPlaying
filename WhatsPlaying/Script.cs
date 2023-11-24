using System;
using System.Threading.Tasks;
using Windows.Media.Control;
using GTA;

namespace WhatsPlaying;

/// <summary>
/// Script that shows what is currently playing in your vehicle.
/// </summary>
public class WhatsPlaying : Script
{
    #region Fields

    private static readonly Dashboard dashboard = new Dashboard();
    
    private static GlobalSystemMediaTransportControlsSessionMediaProperties properties = null;
    
    #endregion
    
    #region Constructors

    /// <summary>
    /// Creates a new script.
    /// </summary>
    public WhatsPlaying()
    {
        Task.Factory.StartNew(UpdateMediaProperties);
        Tick += OnTick;
    }
    
    #endregion
    
    #region Tools

    private static async Task UpdateMediaProperties()
    {
        GlobalSystemMediaTransportControlsSessionManager gmtcsm = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

        while (true)
        {
            GlobalSystemMediaTransportControlsSession session = gmtcsm.GetCurrentSession();
            properties = await session.TryGetMediaPropertiesAsync();
        }
    }
    
    #endregion
    
    #region Event Functions

    private async void OnTick(object sender, EventArgs e)
    {
    }

    #endregion
}
