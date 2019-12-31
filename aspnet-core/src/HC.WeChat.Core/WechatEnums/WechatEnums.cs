using System;
using System.Collections.Generic;
using System.Text;

namespace HC.WeChat.WechatEnums
{
    public enum MsgTypeEnum
    {
        文字消息 = 1,
        图文消息 = 2,
        纯图片 = 3
    }
    /// <summary>
    /// 匹配模式
    /// </summary>
    public enum MatchModeEnum
    {
        精准匹配 = 1,
        模糊匹配 = 2
    }
    /// <summary>
    /// 微信类型
    /// </summary>
    public enum AppTypeEnum
    {
        订阅号 = 1,
        认证订阅号 = 2,
        服务号 = 3,
        认证服务号 = 4
    }

    /// <summary>
    /// 活动状态
    /// </summary>
    public enum ActivityStatusEnum
    {
        草稿 = 1,
        已发布 = 2,
        已下架 = 3
    }

    /// <summary>
    /// 表单状态
    /// </summary>
    public enum FormStatusEnum
    {
        提交申请 = 1,
        初审通过 = 2,
        拒绝 = 3,
        资料回传已审核 = 4,
        取消 = 5,
        营销中心已审核 = 6
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserTypeEnum
    {
        零售客户 = 1,
        内部员工 = 2,
        消费者 = 4,
        取消关注 = 5
    }

    /// <summary>
    /// 用户职位
    /// </summary>
    public enum UserPositionEnum
    {
        营销人员 = 1,
        客户经理 = 2,
        营销中心 = 3,
        机关人员 = 4
    }

    /// <summary>
    /// 订货方式
    /// </summary>
    public enum OrderModeEnum
    {
        无 = 0,
        网上订货 = 1,
        电话订货 = 2,
        手机 = 3
    }

    /// <summary>
    /// 终端
    /// </summary>
    public enum TerminalTypeEnum
    {
        无 = 0,
        建议终端 = 1,
        普通终端 = 2,
        现代终端 = 3
    }

    /// <summary>
    /// 经营规模
    /// </summary>
    public enum ScaleEnum
    {
        小 = 1,
        中 = 2,
        大 = 3
    }

    /// <summary>
    /// 市场类型
    /// </summary>
    public enum MarketTypeEnum
    {
        无 = 0,
        乡村 = 1,
        城镇 = 2
    }

    /// <summary>
    /// 绑定状态
    /// </summary>
    public enum BindStatusEnum
    {
        未绑定 = 0,
        已绑定 = 1
    }

    /// <summary>
    /// 活动类型
    /// </summary>
    public enum ActivityTypeEnum
    {
        办事用烟 = 1
    }
    /// <summary>
    /// 收货人类型
    /// </summary>
    public enum DeliveryUserTypeEnum
    {
        消费者 = 1,
        推荐人 = 2
    }
    /// <summary>
    /// 文章类型
    /// </summary>
    public enum ArticleTypeEnum
    {
        营销活动 = 1,
        经验分享 = 2,
    }

    /// <summary>
    /// 链接类型
    /// </summary>
    public enum ArticleLinkTypeEnum
    {
        内部链接 = 1,
        外部链接 = 2,
    }
    /// <summary>
    /// 积分类型
    /// </summary>
    public enum IntegralTypeEnum
    {
        购买商品兑换 = 1,
        评价店铺赠送 = 2,
        抽奖消费 = 3,
        扫码积分赠送 = 4,
        首次注册赠送 = 5
    }
    /// <summary>
    /// 产品类型
    /// </summary>
    public enum ProductTypeEnum
    {
        卷烟类 = 1,
        特产类 = 2
    }
    /// <summary>
    /// 评价等级
    /// </summary>
    public enum ScoreLevelEmun
    {
        好 = 5,
        中 = 3,
        差 = 1
    }
    /// <summary>
    /// 计数类型
    /// </summary>
    public enum CountTypeEnum
    {
        阅读量 = 1,
        点赞 = 2,
        店铺人气 = 3
    }
    /// <summary>
    /// 投稿表处理状态
    /// </summary>
    public enum ProcessTypeEnum
    {
        未处理 = 0,
        已处理 = 1
    }
    /// <summary>
    /// 配置类型
    /// </summary>
    public enum DeployTypeEnum
    {
        积分配置 = 1,
        通知配置 = 2,
        job配置 = 3,
        预设商品配置 = 4,
        扫码限制配置 =5,
        抽奖配置 = 6
    }
    /// <summary>
    /// 配置代码
    /// </summary>
    public enum DeployCodeEnum
    {
        商品购买 = 1,
        商品评价 = 2,
        店铺扫码兑换 = 3,
        通知配置 = 4,
        首次注册 = 5,
        jo启动状态 = 6,
        预设商品搜索 = 7,
        次数限制 = 8,
        时间限制 = 9,
        店铺距离 = 10,
        抽奖活动管理员 = 11
    }
    /// <summary>
    /// 文章发布状态
    /// </summary>
    public enum ArticlePushStatusEnum
    {
        草稿 = 0,
        已发布 = 1
    }

    /// <summary>
    /// 用户审核状态
    /// </summary>
    public enum UserAuditStatus
    {
        未审核 = 0,
        已审核 = 1
    }

    /// <summary>
    /// 店铺审核状态
    /// </summary>
    public enum ShopAuditStatus
    {
        待审核 = 1,
        已审核 = 2,
        已拒绝 = 0
    }

    /// <summary>
    /// 抽奖方式
    /// </summary>
    public enum LotteryType
    {
        积分抽奖 = 1,
    }

    /// <summary>
    /// --奖品类型(默认积分)
    /// 年会抽奖
    /// </summary>
    public enum PrizeType
    {
        //积分 = 1,
        //实物商品 = 2,
        //未中奖 = 3
        一等奖 = 1,
        二等奖 = 2,
        三等奖 = 3,
        四等奖 = 4,
        安慰奖 = 5,
        参与奖 = 6
    }
    /// <summary>
    /// 兑换方式
    /// </summary>
    public enum ExchangeStyle
    {
        线上兑换 = 1,
        线下兑换 = 2,
        邮寄兑换 = 3
    }

    /// <summary>
    ///限量方式 
    /// </summary>
    public enum LimitStyle
    {
        不限量 = 1,
        每天 = 2,
        每月 = 3,
    }
    /// <summary>
    /// 中奖奖品状态
    /// </summary>
    public enum WinPrizeStatus
    {
        未兑换 = 1,
        已申领 = 2,
        已兑换 = 3,
        已过期 = 4,
    }
    /// <summary>
    /// 生产二维码场景值类型
    /// </summary>
    public enum SceneType
    {
        店铺 = 1
    }

    public enum QuestionType
    {
        客户服务评价=1,
        卷烟供应评价=2,
        市场管理评价=3,
        综合评价=4
    }

    public enum QuarterType
    {
        第一季度 = 1,
        第二季度 = 2,
        第三季度 = 3,
        第四季度 = 4
    }
}
