using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using GTA.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WhatsPlaying.Converters;

namespace WhatsPlaying;

/// <summary>
/// The configuration of the mod.
/// </summary>
internal class Configuration
{
    #region Fields

    private static readonly string path = Path.ChangeExtension(new Uri(Assembly.GetAssembly(typeof(Configuration)).CodeBase).LocalPath, ".json");
    private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
    {
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        Converters = [
            new StringEnumConverter()
        ],
        Formatting = Formatting.Indented,
        Culture = CultureInfo.InvariantCulture
    };
    
    #endregion
    
    #region Properties

    /// <summary>
    /// The offset from the corner.
    /// </summary>
    [JsonProperty("offset")]
    [JsonConverter(typeof(PointFConverter))]
    public PointF Offset { get; set; } = new PointF(277, 0);
    /// <summary>
    /// The corner on the screen to use as a start.
    /// </summary>
    [JsonProperty("corner")]
    public Corner Corner { get; set; } = Corner.BottomLeft;
    /// <summary>
    /// The maximum width of any text.
    /// </summary>
    [JsonProperty("max_width")]
    public float MaxWidth { get; set; } = 400;
    /// <summary>
    /// Whether the UI should be shown on first person.
    /// </summary>
    [JsonProperty("ui_show_on_first_person")]
    public bool UiShowOnFirstPerson { get; set; } = false;
    /// <summary>
    /// Whether the UI should be shown when on foot.
    /// </summary>
    [JsonProperty("ui_show_on_foot")]
    public bool UiShowOnFoot { get; set; } = false;

    #endregion
    
    #region Functions
    
    /// <summary>
    /// Saves the configuration.
    /// </summary>
    public void Save()
    {
        string contents = JsonConvert.SerializeObject(this, settings);
        File.WriteAllText(path, contents);
    }
    /// <summary>
    /// Gets the current configuration.
    /// </summary>
    /// <returns>The current configuration, or a new configuration if is not present.</returns>
    public static Configuration Load()
    {
        try
        {
            string contents = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Configuration>(contents, settings);
        }
        catch (FileNotFoundException)
        {
            Configuration config = new Configuration();
            config.Save();
            return config;
        }
        catch (Exception e)
        {
            Notification.Show($"~r~Error~w~: Unable to load config: {e.Message}");
            return new Configuration();
        }
    }
    
    #endregion
}
