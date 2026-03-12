using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess.View
{
    // TODO: Add all modifiers
    public class ModifierDisplayInfo
    {
        public string Icon { get; set; }
        public Brush Color { get; set; }
        public FontFamily FontFamily { get; set; }
        public string Family { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
    }

    public static class ModifierDetails
    {
        private static readonly Dictionary<ModifierType, ModifierDisplayInfo> ModifierData = new()
        {
            {
                ModifierType.KingPromotion, new ModifierDisplayInfo
                {
                    Icon = "\uE734",
                    Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3AF37")),
                    FontFamily = new FontFamily("Segoe Fluent Icons"),
                    Family = CamelCaseToNormalText(ModifierFamily.WinConditions.ToString()),
                    Duration = "On game end",
                    Description = "When a Player's King reaches a Promotion Rank, the player wins. Normal Chess rules still apply - so be careful of Checkmate!"
                }
            },
            {
                ModifierType.TimeLimit, new ModifierDisplayInfo
                {
                    Icon = "\u23F2",
                    Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEFD5")),
                    FontFamily = new FontFamily("Segoe UI Emoji"),
                    Family = CamelCaseToNormalText(ModifierFamily.WinConditions.ToString()),
                    Duration = "Throughout all game",
                    Description = "Each Player gets a Timer that's active while it's his turn. Once a Player's time runs out, they will lose."
                }
            },
            {
                ModifierType.Poof, new ModifierDisplayInfo
                {
                    Icon = "\u2728",
                    Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cb353d")),
                    FontFamily = new FontFamily("Segoe Emoji"),
                    Family = CamelCaseToNormalText(ModifierFamily.BoardChangers.ToString()),
                    Duration = "Every 5 moves",
                    Description = "A random piece that isn't the King/Queen, or a Pawn, is removed every 5 moves. (both Players)"
                }
            },
            {
                ModifierType.FogOfWar, new ModifierDisplayInfo
                {
                    Icon = "\uE753",
                    Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888")),
                    FontFamily = new FontFamily("Segoe Fluent Icons"),
                    Family = CamelCaseToNormalText(ModifierFamily.BoardChangers.ToString()),
                    Duration = "Throughout all game",
                    Description = "."
                }
            },
            {
                ModifierType.Wormholes, new ModifierDisplayInfo
                {
                    Icon = "\uE895",
                    Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00A8FF")),
                    FontFamily = new FontFamily("Segoe Fluent Icons"),
                    Family = CamelCaseToNormalText(ModifierFamily.BoardChangers.ToString()),
                    Duration = "Throughout all game",
                    Description = "."
                }
            },
            {
                ModifierType.DoubleMoves, new ModifierDisplayInfo
                {
                    Icon = "X2",
                    Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C5CFF")),
                    FontFamily = new FontFamily("Segoe UI"),
                    Family = CamelCaseToNormalText(ModifierFamily.PlayerBoosters.ToString()),
                    Duration = "Throughout all game",
                    Description = "Each Player gets 2 moves in a row."
                }
            },
        };

        public static string GetIcon(ModifierType modifier) => ModifierData.TryGetValue(modifier, out var data) ? data.Icon : null;

        public static Brush GetIconColor(ModifierType modifier) => ModifierData.TryGetValue(modifier, out var data) ? data.Color : null;

        public static FontFamily GetFontFamily(ModifierType modifier) => ModifierData.TryGetValue(modifier, out var data) ? data.FontFamily : new FontFamily("Segoe UI");

        public static string GetModifierFamily(ModifierType modifier) => ModifierData.TryGetValue(modifier, out var data) ? data.Family : "Unknown Family";

        public static string GetDuration(ModifierType modifier) => ModifierData.TryGetValue(modifier, out var data) ? data.Duration : "Unknown Duration";

        public static string GetDescription(ModifierType modifier) => ModifierData.TryGetValue(modifier, out var data) ? data.Description : "Undiscovered Modifier";

        private static string CamelCaseToNormalText(string camelCase) // helper
        {
            return Regex.Replace(camelCase, "(?<=[a-z])(?=[A-Z])", " ");
        }
    }

    public enum ModifierFamily
    {
        WinConditions,
        BoardChangers,
        PlayerBoosters
    }
}
