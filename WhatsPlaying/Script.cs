﻿using System;
using System.Threading.Tasks;
using Windows.Media.Control;
using GTA;
using GTA.Native;
using GTA.UI;
using LemonUI;

namespace WhatsPlaying;

/// <summary>
/// Script that shows what is currently playing in your vehicle.
/// </summary>
public class WhatsPlaying : Script
{
    #region Fields
    
    private static Configuration config = Configuration.Load();
    
    private static GlobalSystemMediaTransportControlsSessionMediaProperties properties = null;
    private static Exception exception = null;

    private readonly ObjectPool pool = new ObjectPool();
    private readonly Dashboard dashboard = new Dashboard();
    private readonly CurrentMedia currentMedia = new CurrentMedia(config.Offset, config.Corner, config.MaxWidth, config.Alignment);

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
            try
            {
                GlobalSystemMediaTransportControlsSession session = gmtcsm.GetCurrentSession();

                if (session == null)
                {
                    properties = null;
                    continue;
                }
                
                properties = await session.TryGetMediaPropertiesAsync();
            }
            catch (Exception e)
            {
                exception = e;
                return;
            }
        }
    }
    
    #endregion
    
    #region Event Functions

    private void OnTick(object sender, EventArgs e)
    {
        if (exception != null)
        {
            Notification.Show($"Exception while attempting to fetch song: {exception.Message}", true);
            exception = null;
        }

        bool isUsingVehicle = Game.Player.Character.CurrentVehicle != null;
        bool isFirstPerson = Function.Call<int>(isUsingVehicle ? Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE : Hash.GET_FOLLOW_PED_CAM_VIEW_MODE) == 4;

        string title = properties == null ? "" : properties.Title;
        string artist = properties == null ? "" : properties.Artist;
        
        currentMedia.Visible = !Game.IsCutsceneActive && ((config.UiShowOnFirstPerson && isFirstPerson) || (config.UiShowOnFoot && !isUsingVehicle) || (!isFirstPerson && isUsingVehicle));
        currentMedia.Title = title;
        currentMedia.Artist = artist;
        
        pool.Process();
        
        dashboard.SetRadio("", "Media Player", artist, title);
    }

    #endregion
}
