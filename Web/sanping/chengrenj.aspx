<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chengrenj.aspx.cs" Inherits="Web.sanping.chengrenj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"><head>


<meta content="text/html; charset=utf-8" http-equiv="Content-Type">
<title>成人价弹出框</title>
<link type="text/css" rel="stylesheet" href="../css/sytle.css">
</head><body>
                                <div style="background: url(../images/trbg.png) repeat;">
                                    <div class="dlprice">
                                        <%foreach (var v in sinfo[i].CustomerLevels)
                                          { %>
                                        <div class="left">
                                            <div class="title">
                                                <%=v.LevelName%>
                                                <input type="hidden" name="hd_cusStandType" value="<%=(int)((EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)(v.LevelType)) %>"/>
                                                <input type="hidden" name="hd_cusStandId" value="<%=v.LevelId %>"/>
                                                <input type="hidden" name="hd_cusStandName" value="<%=v.LevelName%>"/>
                                                </div>
                                            <div>
                                                成人<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal( v.AdultPrice )%>/
                                                儿童
                                                <%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(v.ChildrenPrice) %>
                                            </div>
                                        </div>
                                        <%} %>
                                        <div style="clear: both;">
                                        </div>
                                    </div>
                                    <div style="clear:both;"></div>
                                    </div>
</body></html>