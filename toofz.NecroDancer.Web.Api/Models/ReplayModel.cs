using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer replay.
    /// </summary>
    public sealed class ReplayModel
    {
        /// <summary>
        /// A unique value provided by Steam to identify the replay.
        /// </summary>
        [Required]
        public long ReplayId { get; set; }

        /// <summary>
        /// An error code indicating what error was encountered while processing the replay.
        /// </summary>
        public int? ErrorCode { get; set; }

        /// <summary>
        /// The seed of the replay.
        /// </summary>
        public int? Seed { get; set; }
        /// <summary>
        /// The version of the replay format.
        /// </summary>
        public int? Version { get; set; }
        /// <summary>
        /// The name of the entity that killed the player. This may be null.
        /// </summary>
        public string KilledBy { get; set; }

        private sealed class ReplayModelValidator : AbstractValidator<ReplayModel>
        {
            public ReplayModelValidator()
            {
                RuleFor(x => x.Seed).NotNull().When(x => x.ErrorCode == null);
                RuleFor(x => x.Version).NotNull().When(x => x.ErrorCode == null);
                RuleFor(x => x.ErrorCode).NotNull().When(x => x.Seed == null || x.Version == null);
            }
        }
    }
}