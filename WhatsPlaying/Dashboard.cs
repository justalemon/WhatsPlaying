using LemonUI.Scaleform;

namespace WhatsPlaying;

/// <summary>
/// The dashboard of the vehicle.
/// </summary>
public class Dashboard : BaseScaleform
{
    #region Constructors

    /// <summary>
    /// Creates a new dashboard scaleform.
    /// </summary>
    public Dashboard() : base("DASHBOARD")
    {
    }
    
    #endregion
    
    #region Functions

    /// <summary>
    /// Sets the radio metadata.
    /// </summary>
    /// <param name="tuning">Unknown.</param>
    /// <param name="radio">The name of the radio.</param>
    /// <param name="artist">The artist of the song.</param>
    /// <param name="song">The song that is playing.</param>
    public void SetRadio(string tuning, string radio, string artist, string song)
    {
        CallFunction("SET_RADIO", tuning, radio, artist, song);
    }
    /// <summary>
    /// Does nothing.
    /// </summary>
    public override void Update()
    {
    }
    
    #endregion
}
