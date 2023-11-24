using System;
using System.Threading.Tasks;
using Windows.Media.Control;
using GTA;
using LemonUI;

namespace WhatsPlaying;

/// <summary>
/// Script that shows what is currently playing in your vehicle.
/// </summary>
public class WhatsPlaying : Script
{
    #region Fields

    private static readonly Dashboard dashboard = new Dashboard();

    private readonly ObjectPool pool = new ObjectPool();
    private readonly CurrentMedia currentMedia = new CurrentMedia();
    
    private static GlobalSystemMediaTransportControlsSessionMediaProperties properties = null;
    
    #endregion
    
    #region Constructors

    /// <summary>
    /// Creates a new script.
    /// </summary>
    public WhatsPlaying()
    {
        pool.Add(currentMedia);
        
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

    private void OnTick(object sender, EventArgs e)
    {
        if (properties == null)
        {
            return;
        }

        currentMedia.Title = properties.Title;
        currentMedia.Artist = properties.Artist;
        
        pool.Process();
        
        dashboard.SetRadio("", "Media Player", properties.Artist, properties.Title);
    }

    #endregion
}
