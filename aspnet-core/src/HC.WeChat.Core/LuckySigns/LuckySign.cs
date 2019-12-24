using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.WeChat.LuckySigns
{
    [Table("LuckySigns")]
    public class LuckySign : AuditedEntity<Guid>, IHasCreationTime
    {

        /// <summary>
        /// OpenId
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string OpenId { get; set; }
    }
}
