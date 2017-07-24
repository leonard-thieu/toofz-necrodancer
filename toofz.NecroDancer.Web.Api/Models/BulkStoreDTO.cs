namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// Represents the response of a bulk store operation.
    /// </summary>
    public sealed class BulkStoreDTO
    {
        /// <summary>
        /// The number of rows affected.
        /// </summary>
        public int RowsAffected { get; set; }
    }
}