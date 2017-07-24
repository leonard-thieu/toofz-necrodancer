using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using toofz.NecroDancer.Saves;

namespace toofz.NecroDancer.Replays
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer replay.
    /// </summary>
    /// <remarks>
    /// This type is compatible with Crypt of the NecroDancer v1.09 (Steam) (replay version: 75). It may be 
    /// compatible with previous, future, or GOG versions but does not guarantee this.
    /// </remarks>
    /// <seealso cref="ReplaySerializer"/>
    public sealed class ReplayData
    {
        /// <summary>
        /// The header for the replay. This contains information such as the starting zone, the game mode, and 
        /// the duration of the replay.
        /// </summary>
        public Header Header { get; set; }

        /// <summary>
        /// The collection of level data.
        /// </summary>
        public ICollection<LevelData> Levels { get; } = new Collection<LevelData>();

        /// <summary>
        /// The save data associated with the replay. This may be null. This is only written by the game 
        /// for single-zone mode replays.
        /// </summary>
        public SaveData SaveData { get; set; }

        /// <summary>
        /// Tries to get the seed for the replay.
        /// </summary>
        /// <param name="seed">If successful, returns the seed of the replay; otherwise, returns 0.</param>
        /// <returns>
        /// Returns true if the seed was found. Returns false if the first level does not exist.
        /// </returns>
        // http://braceyourselfgames.com/forums/viewtopic.php?f=5&t=3240
        public bool TryGetSeed(out int seed)
        {
            var level = Levels.FirstOrDefault();
            if (level != null)
            {
                seed = (int)(492935547 * ((level.Seed - 6) % ((long)uint.MaxValue + 1)));
                return true;
            }
            else
            {
                seed = 0;
                return false;
            }
        }
    }
}
