using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.LuckySigns
{
    [Table("LuckySigns")]
    public class LuckySign : Entity<Guid>, IHasCreationTime
    {

        /// <summary>
        /// OpenId
        /// </summary>
        [Required]
        public virtual Guid UserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
    }
}
