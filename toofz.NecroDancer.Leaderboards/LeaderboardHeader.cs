using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using log4net;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Header that represents a Crypt of the NecroDancer leaderboard.
    /// </summary>
    /// <seealso cref="Leaderboard"/>
    [DebuggerDisplay("{Name}")]
    public sealed class LeaderboardHeader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LeaderboardHeader));
        private static readonly Regex DateRegex = new Regex(@"\d{1,2}/\d{1,2}/\d{4}", RegexOptions.Compiled); // Matches a date like 15/8/2015

        #region XML Properties

        /// <summary>
        /// The address to retrieve leaderboard entries from.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// A value that uniquely identifies the leaderboard.
        /// </summary>
        public int LeaderboardId { get; set; }

        /// <summary>
        /// The name of the leaderboard.
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _Name = value;
                OnNameChanged();
            }
        }
        private string _Name;

        #endregion

        #region Derived Properties

        /// <summary>
        /// The character associated with the leaderboard.
        /// </summary>
        public Character Character { get; private set; }
        /// <summary>
        /// The run associated with the leaderboard.
        /// </summary>
        public Run Run { get; private set; }
        /// <summary>
        /// The date associated with the leaderboard if it is a daily. This value is null for leaderboards 
        /// that are not dailies.
        /// </summary>
        public DateTime? Date { get; private set; }

        /// <summary>
        /// Indicates if the leaderboard is a production leaderboard.
        /// </summary>
        public bool IsProduction { get; private set; }
        /// <summary>
        /// Indicates if the leaderboard is a cooperative leaderboard.
        /// </summary>
        public bool IsCooperative { get; private set; }

        #endregion

        private void OnNameChanged()
        {
            if (Name == null)
                throw new ArgumentNullException(nameof(Name));

            var tokens = Name.Split(new char[] { ' ', '_' }, StringSplitOptions.RemoveEmptyEntries);

            Character = Characters.Cadence;

            var isScore = false;
            var isSeeded = false;
            var isSpeed = false;

            foreach (var token in tokens)
            {
                switch (token)
                {
                    case "HARDCORE": isScore = true; continue;
                    case "SPEEDRUN": isSpeed = true; continue;
                    case "SEEDED": isSeeded = true; continue;
                    case "DEATHLESS": Run = Runs.Deathless; continue;
                    case "PROD": IsProduction = true; continue;
                    case "CO-OP": IsCooperative = true; continue;
                    case "CUSTOM":      // Custom music
                    case "DEV":         // Development leaderboards
                    case "TEST":        // Test leaderboards
                    case "MUSIC":       // CUSTOM MUSIC
                    case "Chars":       // All Chars
                    case "Mode":        // Story Mode
                    case "Ghost":       // Removed character
                    case "Pacifist":    // Removed character
                    case "Thief":       // Removed character
                    case "Mage":
                    case "ZONES":
                    case "Battle":
                    case "Ranking":
                    case "Combo":
                    case "Challenge":
                    case "Feet":
                    case "Traveled":
                    case "IQ":
                    case "Puzzle":
                    case "Multi":
                    case "Point":
                    case "Push":
                    case "Quickest":
                    case "Win":
                    case "Stage":
                    case "test":
                    case "leaderboard":
                        continue;
                    case "DLC": continue;
                }

                if (token.EndsWith("RDCORE"))
                {
                    isScore = true;
                    continue;
                }

                Character character;
                if (Enumeration.TryParse(token, true, out character))
                {
                    Character = character;
                    continue;
                }

                if (DateRegex.IsMatch(token))
                {
                    Character = Characters.Cadence;
                    Run = Runs.Daily;

                    DateTime date;
                    if (DateTime.TryParseExact(token, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out date))
                    {
                        Date = date;
                    }
                    else
                    {
                        Log.Warn($"'{token}' is not a valid date.");
                    }

                    continue;
                }

                Log.Debug($"Unknown leaderboard attribute: {token} ({Name})");
            }

            Run = GetRun(isScore, isSpeed, isSeeded);
        }

        private Run GetRun(bool isScore, bool isSpeed, bool isSeeded)
        {
            if (Run != null)
                return Run;

            if (isScore)
            {
                if (!isSeeded)
                    return Runs.Score;
                return Runs.SeededScore;
            }

            if (isSpeed)
            {
                if (!isSeeded)
                    return Runs.Speed;
                return Runs.SeededSpeed;
            }

            Log.Debug($"Unknown run type for leaderboard '{Name}'.");
            return Runs.Score;
        }
    }
}
