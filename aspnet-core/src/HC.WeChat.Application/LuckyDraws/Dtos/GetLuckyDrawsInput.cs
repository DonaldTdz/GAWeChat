
using Abp.Runtime.Validation;
using HC.WeChat.Dto;
using HC.WeChat.LuckyDraws;
using HC.WeChat.WechatEnums;
using System;
using System.Collections.Generic;

namespace HC.WeChat.LuckyDraws.Dtos
{
    public class GetLuckyDrawsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

    }

    public class WeiXinCreateInput {

        public string Name { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public Boolean IsPublish { get; set; }

        public List<WeiXinPriceInput> List { get; set; }

    }
    public class WeiXinPriceInput {

        public string Name { get; set; }

        public PrizeType Type { get; set; }

        public int Num { get; set; }

    }

    public class WeiXinUpdatePubInput { 
    
        public Guid Id { get; set; }
    }
}
