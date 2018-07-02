using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using HC.WeChat.Exhibitions;

namespace HC.WeChat.Exhibitions.Dtos
{
    public class ExhibitionEditDto : Entity<Guid?>
    {
        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime? BeginTime { get; set; }


        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// TopTotal
        /// </summary>
        [Required(ErrorMessage = "TopTotal不能为空")]
        public int TopTotal { get; set; }


        /// <summary>
        /// Frequency
        /// </summary>
        public int? Frequency { get; set; }





        ////BCC/ BEGIN CUSTOM CODE SECTION

        ////ECC/ END CUSTOM CODE SECTION
    }
}