using Chess.Model;
using System.IO;
using System.Text.Json;

public interface IModifierRepository
{
    ModifierData GetModifierData(ModifierType type);
}

public class ModifierRepository : IModifierRepository
{
    // The dictionary that lives in memory
    private readonly Dictionary<string, ModifierData> _modifierData;

    public ModifierRepository()
    {
        var assembly = typeof(Chess.Model.ModifierType).Assembly;

        string resourceName = "Chess.Model.JSON.Modifiers.json";

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new FileNotFoundException($"Could not find embedded resource: {resourceName}");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonText = reader.ReadToEnd();
                _modifierData = JsonSerializer.Deserialize<Dictionary<string, ModifierData>>(jsonText);
            }
        }
    }

    public ModifierData GetModifierData(ModifierType type)
    {
        string key = type.ToString();

        if (_modifierData.TryGetValue(key, out var data))
        {
            return data;
        }

        return new ModifierData { Description = "Data not found." };
    }
}
