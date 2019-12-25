

using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.WeChat.LuckyDraws;

namespace HC.WeChat.LuckyDraws.Dtos
{
    public class GetLuckyDrawForEditOutput
    {

        public LuckyDrawEditDto LuckyDraw { get; set; }

    }

    public class ApiResultRef { 
    
        public int iSsuccess { get; set; }

    }

    public class WXLuckyDrawOutput { 
    
        public DateTime CreationTime { get; set; }

        public string Name { get; set; }

        public Guid? Id { get; set; }

        public Boolean IsPublish { get; set; }
    }
}