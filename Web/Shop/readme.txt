1.此目录为组团端入口，T1,T2....TN分别为不同入口模板。
2.域名请指向不同模板default.aspx页。
3.新增模板时统一放入/shop/目录，模板目录命名务必使用TN命名方式。各模板默认页务必使用default.aspx。
4.新增模板请增加EyouSoft.Model.EnumType.SysStructure.SiteTemplate模板枚举，枚举值必须与TN中的N值对应。
5.新增模板请调整模板验证方法：EyouSoft.Common.Utils.GetShopTemplateVirtualDirectory()。

汪奇志 2012-04-03
