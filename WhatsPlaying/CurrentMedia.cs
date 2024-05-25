using System;
using System.Drawing;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Tools;

namespace WhatsPlaying;

/// <summary>
/// Shows the currently playing media in the screen.
/// </summary>
public class CurrentMedia : IProcessable, IRecalculable
{
    #region Fields

    private readonly ScaledText title = new ScaledText(PointF.Empty, "", 0.5f)
    {
        ShadowStyle = new Shadow
        {
            UseClassic = true
        }
    };
    private readonly ScaledText artist = new ScaledText(PointF.Empty, "", 0.4f)
    {
        ShadowStyle = new Shadow
        {
            UseClassic = true
        },
        Color = Color.LightGray
    };

    private bool needsRecalculation = false;
    private PointF offset;
    private Corner corner;

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
    /// <summary>
    /// The offset from the corner.
    /// </summary>
    public PointF Offset
    {
        get => offset;
        set
        {
            offset = value;
            needsRecalculation = true;
        }
    }
    /// <summary>
    /// The corner to use.
    /// </summary>
    public Corner Corner
    {
        get => corner;
        set
        {
            corner = value;
            needsRecalculation = true;
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new media viewer.
    /// </summary>
    public CurrentMedia(PointF offset, Corner corner, float maxWidth, Alignment alignment)
    {
        this.offset = offset;
        this.corner = corner;

        if (maxWidth >= 0)
        {
            title.WordWrap = maxWidth;
            artist.WordWrap = maxWidth;
        }

        title.Alignment = alignment;
        artist.Alignment = alignment;

        needsRecalculation = true;
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
        PointF targetCorner;

        SafeZone.ResetAlignment();
        switch (corner)
        {
            case Corner.TopLeft:
                targetCorner = SafeZone.TopLeft;
                break;
            case Corner.TopRight:
                targetCorner = SafeZone.TopRight;
                break;
            case Corner.BottomLeft:
                targetCorner = SafeZone.BottomLeft;
                break;
            case Corner.BottomRight:
                targetCorner = SafeZone.BottomRight;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(corner), "Invalid corner side.");
        }

        PointF pos = new PointF(targetCorner.X + offset.X, targetCorner.Y + offset.Y);

        float artistLineHeight = artist.LineHeight;
        float artistLineCount = artist.LineCount;
        float artistOffset = artistLineHeight * 0.21f;
        float titleLineHeight = title.LineHeight;
        float titleLineCount = title.LineCount;
        float titleOffset = titleLineHeight * 0.08f;

        artist.Position = pos with {Y = pos.Y - (artistLineHeight * artistLineCount) - (artistOffset * artistLineCount) - artistOffset};
        title.Position = pos with {Y = artist.Position.Y - (titleLineHeight * titleLineCount) - (titleOffset * titleLineCount) - titleOffset};
    }
    
    #endregion
}
