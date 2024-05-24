using System.Drawing;
using LemonUI;
using LemonUI.Elements;

namespace WhatsPlaying;

/// <summary>
/// Shows the currently playing media in the screen.
/// </summary>
public class CurrentMedia : IProcessable, IRecalculable
{
    #region Fields

    private readonly ScaledText title = new ScaledText(PointF.Empty, "", 0.5f)
    {
        ShadowStyle =
        {
            UseClassic = true
        }
    };
    private readonly ScaledText artist = new ScaledText(PointF.Empty, "", 0.4f)
    {
        ShadowStyle =
        {
            UseClassic = true
        },
        Color = Color.LightGray
    };

    private bool needsRecalculation = false;
    
    #endregion
    
    #region Properties

    /// <summary>
    /// Whether the current media is visible or not.
    /// </summary>
    public bool Visible { get; set; } = true;
    /// <summary>
    /// The title of the current media.
    /// </summary>
    public string Title
    {
        get => title.Text;
        set
        {
            if (title.Text == value)
            {
                return;
            }
            
            title.Text = value;
            needsRecalculation = true;
        }
    }

    /// <summary>
    /// The artist of the current media.
    /// </summary>
    public string Artist
    {
        get => artist.Text;
        set
        {
            if (title.Text == value)
            {
                return;
            }
            
            artist.Text = value;
            needsRecalculation = true;
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new media viewer.
    /// </summary>
    public CurrentMedia()
    {
    }

    #endregion
    
    #region Functions
    
    /// <summary>
    /// Processes and draws the current media on the screen.
    /// </summary>
    public void Process()
    {
        if (!Visible || string.IsNullOrWhiteSpace(title.Text) || string.IsNullOrWhiteSpace(artist.Text))
        {
            return;
        }

        if (needsRecalculation)
        {
            Recalculate();
            needsRecalculation = false;
        }
        
        title.Draw();
        artist.Draw();
    }
    /// <summary>
    /// Recalculates the position of the UI elements.
    /// </summary>
    public void Recalculate()
    {
        Screen.SetElementAlignment(GFXAlignment.Left, GFXAlignment.Bottom);
        PointF pos = Screen.GetRealPosition(PointF.Empty);
        Screen.ResetElementAlignment();

        const float offset = 5;
        const float separation = 5;

        pos.X += 277;

        artist.Position = new PointF(pos.X, pos.Y - (artist.LineHeight * artist.LineCount) - offset);
        title.Position = new PointF(pos.X, artist.Position.Y - (title.LineHeight * title.LineCount) - separation);
    }
    
    #endregion
}
